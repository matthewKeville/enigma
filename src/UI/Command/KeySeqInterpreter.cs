using Enums;

namespace UI.Commands {

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

  public class KeySeqInterpreter {

    // command    : command to execute
    // propagate  : propagate the keys
    // sequence   : the sequence to propagate
    public class KeySeqResponse {
      public Command? Command;
      public bool Propagate;
      public List<ConsoleKey>? Sequence;
      public KeySeqResponse(Command? command,bool propagate,List<ConsoleKey>? sequence) {
        Command = command;
        Propagate = propagate;
        Sequence = sequence;
      }

      // key sequence consumed, but no command to return : internal command
      public static KeySeqResponse Noop() { 
        return new KeySeqResponse(null,false,null);
      }

    }

    private static String KeySequenceToString(List<ConsoleKey> keyList) {
      String result = "";
      foreach ( ConsoleKey key in keyList  ) {
        result += " " + key.ToString();
      }
      return result;
    }

    private Mode mode = Mode.NORMAL;
    private List<ConsoleKey> keyPresses = new List<ConsoleKey>();
    private Dictionary<List<ConsoleKey>,Command> CommandMap;

    public KeySeqInterpreter(Dictionary<List<ConsoleKey>,Command> CommandMap) 
    {
      //this.CommandMap = CommandMap;
      this.CommandMap = new Dictionary<List<ConsoleKey>, Command>(CommandMap,new KeySequenceEqual());
    }

    /**
     * @Return V1 : the command mapped to the key sequnce
     * @Return V2 : whether the key sequence is a partial match
     */
    private (Command?,bool) tryGetCommand(List<ConsoleKey> keys) {

      if ( CommandMap.Count() == 0 ) {
        return (null,false);
      }

      //exact match?
      if ( CommandMap.ContainsKey(keys) ) {
        Command command = CommandMap[keys];
        return (command,true);
      } 

      //partial match?
      foreach ( List<ConsoleKey> keyList in CommandMap.Keys ) {
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

    public KeySeqResponse ProcessKey(ConsoleKey key) {

      //insert mode (special)

      if ( mode == Mode.INSERT ) {
        int keyNum = ((int)key);
        if ( 65 <= keyNum && keyNum <= 90 ) {

          return new KeySeqResponse(
              new Command(CommandMode.INSERT,CommandType.INSERT_CHAR,key),
              false,
              null
          );

        } else if ( 97 <= keyNum && keyNum <= 122 ) {

          return new KeySeqResponse(
              new Command(CommandMode.INSERT,CommandType.INSERT_CHAR,key),
              false,
              null
          );

        } else if ( key == ConsoleKey.Backspace ) {

          return new KeySeqResponse(
              new Command(CommandMode.INSERT,CommandType.DEL_CHAR,key),
              false,
              null
          );

        }
      }

      keyPresses.Add(key);

      (Command? possibleCommand,bool partialMatch) = tryGetCommand(keyPresses);

      if (!partialMatch) {
        Trace.WriteLine($" no key sequence match : {KeySequenceToString(keyPresses)}");
        KeySeqResponse response = new KeySeqResponse(null,true,new List<ConsoleKey>(keyPresses));
        keyPresses.Clear();
        return response;
      }

      if (possibleCommand is null) {
        Trace.WriteLine($" key sequence partial match : {KeySequenceToString(keyPresses)}");
        return new KeySeqResponse(
            null,
            false,
            null
        );
      }

      Trace.WriteLine($" key sequence match : {KeySequenceToString(keyPresses)}");
      Command command = (Command) possibleCommand;

      //meta ( mode switch )
      if ( command.Mode == CommandMode.META ) {

        if ( command.Type == CommandType.NORMAL_MODE && mode == Mode.INSERT ) {
          mode = Mode.NORMAL;
          keyPresses.Clear();
          return KeySeqResponse.Noop();
        } else if ( command.Type == CommandType.INSERT_MODE && mode == Mode.NORMAL ) {
          mode = Mode.INSERT;
          keyPresses.Clear();
          return KeySeqResponse.Noop();
        }

      }

      //normal
      if ( mode == Mode.NORMAL && command.Mode == CommandMode.NORMAL) {
          keyPresses.Clear();
          return new KeySeqResponse(command,false,null);
      } 

      Trace.WriteLine(" unexpected outcome in KeySeqInterpreter ");
      return KeySeqResponse.Noop();

    }

  }

}
