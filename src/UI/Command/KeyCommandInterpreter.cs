/**
using Enums;
using Microsoft.Extensions.Hosting;

namespace UI.Command {

  // Sadly, I can't map '$' or '^' as this class
  // src/libraries/System.Console/src/System/IO/KeyParser.cs
  // states that it is a limitation of the Parser.
  // If i'm really bugged by this down the line I could
  // A. Implement my own Key Parser, or B. Use a different language
  // for key processing and send the keys over to this process.
 
  public class KeySequenceEqual : EqualityComparer<List<ConsoleKey>>
  {
      public override bool Equals(List<ConsoleKey> kl1, List<ConsoleKey> kl2)
      {
          if (object.ReferenceEquals(kl1, kl2))
              return true;

          if (kl1 is null || kl2 is null)
              return false;

          return kl1.SequenceEqual(kl2);

      }

      public override int GetHashCode(List<ConsoleKey> kl) {
        int hash = 41 * kl.Count();
        foreach ( ConsoleKey key in kl ) {
          hash*=((int)key);
        }
        return hash;
      }
  }


  public class KeyCommandInterpreter {

    private static String KeySequenceToString(List<ConsoleKey> keyList) {
      String result = "";
      foreach ( ConsoleKey key in keyList  ) {
        result += " " + key.ToString();
      }
      return result;
    }

    // This is a quick cheat to expose mode to other view components
    // not sure of a more elegant solution. There should only 
    // ever be one of these anyway.
    private static Mode mode = Mode.NORMAL;
    public static Mode GetMode() {
      return mode;
    }

    private Dictionary<List<ConsoleKey>,Command> commandMap;
    private Thread interpreterThread ;
    private bool running = true;
    private CommandDispatcher commandDispatcher;
    private List<ConsoleKey> keyPresses = new List<ConsoleKey>();


    public KeyCommandInterpreter(IApplicationLifetime applicationLifetime,CommandDispatcher commandDispatcher) {
      this.commandDispatcher = commandDispatcher;
      applicationLifetime.ApplicationStopping.Register(stop);
      Trace.WriteLine("KeyCommandInterpreter starting ...");
      buildCommandMap();
      interpreterThread = new Thread(processIO);
      interpreterThread.Start();
    }

    private void triggerCommand(Command command) {
      commandDispatcher.DispatchCommand(new CommandEventArgs(command));
    }

    private void triggerCommand(Command command,ConsoleKey key) {
      commandDispatcher.DispatchCommand(new CommandEventArgs(command,key));
    }

    private void buildCommandMap() {
      commandMap = new Dictionary<List<ConsoleKey>, Command>(new KeySequenceEqual());
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.Escape},Command.NORMAL_MODE);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.I},Command.INSERT_MODE);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.Tab},Command.SWAP_PANE);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.H},Command.MOVE_LEFT);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.L},Command.MOVE_RIGHT);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.K},Command.MOVE_UP);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.J},Command.MOVE_DOWN);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.D6},Command.MOVE_WORD_START);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.D4},Command.MOVE_WORD_END);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.Spacebar},Command.SWAP_ORIENTATION);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.Backspace},Command.DEL_CHAR);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.D,ConsoleKey.W},Command.DEL_WORD);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.Enter},Command.CONFIRM);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.F1},Command.TOGGLE_HELP);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.Q},Command.EXIT);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.W},Command.MOVE_WORD);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.B},Command.MOVE_BACK_WORD);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.Z},Command.CHECK_WORD);
      commandMap.Add(new List<ConsoleKey>(){ConsoleKey.N},Command.INSTALL);
    }

     //@Return V1 : the command mapped to the key sequnce
     //@Return V2 : whether the key sequence is a partial match
     
    private (Command?,bool) tryGetCommand(List<ConsoleKey> keys) {

      //exact match?
      if ( commandMap.ContainsKey(keys) ) {
        Command command = commandMap[keys];
        return (command,true);
      } 

      //partial match?
      foreach ( List<ConsoleKey> keyList in commandMap.Keys ) {
        if ( keyList.Count >= keys.Count ) {
          bool pmatch = true;
          int k = 0;
          while ( k < keys.Count && pmatch ) {
            pmatch = keyList[k].Equals(keys[k]);
            k++;
          }
          if ( pmatch ) {
            return (null,true);
          }
        }
      }

      //no partial match
      return (null,false);

    }

    void processKey(ConsoleKey key) {

      //insert mode

      if ( mode == Mode.INSERT ) {
        int keyNum = ((int)key);
        if ( 65 <= keyNum && keyNum <= 90 ) {
          triggerCommand(Command.INSERT_CHAR,key);
          return;
        } else if ( 97 <= keyNum && keyNum <= 122 ) {
          triggerCommand(Command.INSERT_CHAR,key);
          return;
        } else if ( key == ConsoleKey.Backspace ) {
          triggerCommand(Command.DEL_CHAR,key);
          return;
        }
      }


      keyPresses.Add(key);
      //Trace.WriteLine($" key sequence is : {KeySequenceToString(keyPresses)}");

      (Command? possibleCommand,bool partialMatch) = tryGetCommand(keyPresses);

      if ( !partialMatch) {
        Trace.WriteLine($" no key sequence match : {KeySequenceToString(keyPresses)}");
        keyPresses.Clear();
        return;
      }

      if (!possibleCommand.HasValue) {
        Trace.WriteLine($" key sequence partial match : {KeySequenceToString(keyPresses)}");
        return;
      }

      Trace.WriteLine($" key sequence match : {KeySequenceToString(keyPresses)}");
      Command command = possibleCommand.Value;

      //meta ( mode switch )
      if ( Commands.Meta.Contains(command) ) {
        Trace.WriteLine(" meta mode command hit ");
        if ( command == Command.NORMAL_MODE && mode == Mode.INSERT ) {
          mode = Mode.NORMAL;
          keyPresses.Clear();
          return;
        } else if ( command == Command.INSERT_MODE && mode == Mode.NORMAL ) {
          mode = Mode.INSERT;
          keyPresses.Clear();
          return;
        }
      }

      //normal
      if ( mode == Mode.NORMAL ) {
          triggerCommand(command);
          keyPresses.Clear();
          return;
      }      

    }

    void processIO() {
      while ( running ) {
        if ( Console.KeyAvailable ) {
          ConsoleKeyInfo keyInfo = Console.ReadKey(true);
          processKey(keyInfo.Key);
        }
      }
    }

    void stop() {
      Trace.WriteLine("CommandInterpeter stopping ...");
      running = false;
    }


  }

}
*/
