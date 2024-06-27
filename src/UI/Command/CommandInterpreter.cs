using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace UI.Command {

  // Sadly, I can't map '$' or '^' as this class
  // src/libraries/System.Console/src/System/IO/KeyParser.cs
  // states that it is a limitation of the Parser.
  // If i'm really bugged by this down the line I could
  // A. Implement my own Key Parser, or B. Use a different language
  // for key processing and send the keys over to this process.

  public class CommandInterpreter {

    enum Mode {
      NORMAL,
      INSERT
    }

    public event EventHandler<UI.Command.CommandEventArgs> raiseCommandEvent;
    private Dictionary<ConsoleKey,Command> commandMap;
    private Mode mode = Mode.NORMAL;
    private Thread interpreterThread ;
    private bool running = true;

    public CommandInterpreter(IApplicationLifetime applicationLifetime) {
      applicationLifetime.ApplicationStopping.Register(stop);
      Trace.WriteLine("CommandInterpreter starting ...");
      buildCommandMap();
      interpreterThread = new Thread(processIO);
      interpreterThread.Start();
    }

    private void triggerCommand(Command command) {
      raiseCommandEvent(this, new CommandEventArgs(command));
    }

    private void buildCommandMap() {
      commandMap = new Dictionary<ConsoleKey,Command>();
      commandMap.Add(ConsoleKey.Escape,Command.NORMAL_MODE);
      commandMap.Add(ConsoleKey.I,Command.INSERT_MODE);
      commandMap.Add(ConsoleKey.Tab,Command.SWITCH_VIEW);
      commandMap.Add(ConsoleKey.H,Command.MOVE_LEFT);
      commandMap.Add(ConsoleKey.L,Command.MOVE_RIGHT);
      commandMap.Add(ConsoleKey.K,Command.MOVE_UP);
      commandMap.Add(ConsoleKey.J,Command.MOVE_DOWN);
      commandMap.Add(ConsoleKey.D6,Command.MOVE_WORD_START);
      commandMap.Add(ConsoleKey.D4,Command.MOVE_WORD_END);
      commandMap.Add(ConsoleKey.Spacebar,Command.SWAP_ORIENTATION);
      commandMap.Add(ConsoleKey.Backspace,Command.DEL_WORD);
      commandMap.Add(ConsoleKey.D,Command.DEL_CHAR);
    }

    private Command? tryGetNormalCommand(ConsoleKey key) {
      if ( !commandMap.ContainsKey(key) ) {
        return null;
      }
      Command command = commandMap[key];
      return Commands.Normal.Contains(command) ? command : null;
    }

    private Command? tryGetMetaCommand(ConsoleKey key) {
      if ( !commandMap.ContainsKey(key) ) {
        return null;
      }
      Command command = commandMap[key];
      return Commands.Meta.Contains(command) ? command : null;
    }

    void processKey(ConsoleKey key) {

      Command? meta = tryGetMetaCommand(key);

      if ( meta.HasValue ) {
        if ( meta == Command.NORMAL_MODE && mode == Mode.INSERT ) {
          mode = Mode.NORMAL;
          return;
        } else if ( meta == Command.INSERT_MODE && mode == Mode.NORMAL ) {
          mode = Mode.INSERT;
          return;
        }
      }

      Command? normal = tryGetNormalCommand(key);

      if ( mode == Mode.NORMAL ) {
        if ( normal.HasValue ) {
          triggerCommand(normal.Value);
          return;
        }
      }      

      if ( mode == Mode.INSERT ) {
        int keyNum = ((int)key);
        if ( 65 <= keyNum && keyNum <= 90 ) {
          raiseCommandEvent(this, new CommandEventArgs(Command.INSERT_CHAR,key));
        } else if ( 97 <= keyNum && keyNum <= 122 ) {
          raiseCommandEvent(this, new CommandEventArgs(Command.INSERT_CHAR,key));
        } else if ( key == ConsoleKey.Backspace ) {
          raiseCommandEvent(this, new CommandEventArgs(Command.DEL_CHAR,key));
        }
        return;
      }

    }

    protected virtual void OnRaiseCustomEvent(CommandEventArgs e)
    {
        EventHandler<CommandEventArgs>? raiseEvent = raiseCommandEvent;
        if (raiseEvent != null) { 
          raiseEvent(this, e);
        }
    }

    void processIO() {
      while ( running ) {
        if ( Console.KeyAvailable ) {
          ConsoleKeyInfo keyInfo = Console.ReadKey(false);
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
