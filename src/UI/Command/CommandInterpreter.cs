using System.Diagnostics;

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

    public CommandInterpreter() {
      buildCommandMap();
      Thread thread = new Thread(processIO);
      thread.Start();
      Console.WriteLine("started");
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

    void processKey(ConsoleKey key) {

      if (!commandMap.ContainsKey(key)) {
        return;
      }

      Command command = commandMap[key];

      if ( Commands.Meta.Contains(command) ) {
        triggerCommand(command);
        return;
      }

      if ( Commands.Normal.Contains(command) ) {
        triggerCommand(command);
        return;
      }
      
      if ( Commands.Insert.Contains(command) ) {
        triggerCommand(command);
        return;
      }

    }

    protected virtual void OnRaiseCustomEvent(CommandEventArgs e)
    {
        // Make a temporary copy of the event to avoid possibility of
        // a race condition if the last subscriber unsubscribes
        // immediately after the null check and before the event is raised.
        EventHandler<CommandEventArgs>? raiseEvent = raiseCommandEvent;

        // Event will be null if there are no subscribers
        if (raiseEvent != null)
        {
            // Call to raise the event.
            raiseEvent(this, e);
        }
    }

    void processIO() {
      while ( true ) {
        if ( Console.KeyAvailable ) {
          ConsoleKeyInfo keyInfo = Console.ReadKey(false);
          processKey(keyInfo.Key);
        }
      }
    }


  }

}
