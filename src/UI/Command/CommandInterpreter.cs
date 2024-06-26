using System.Diagnostics;

namespace UI.Command {

  public class CommandInterpreter {

    enum Mode {
      NORMAL,
      INSERT
    }

    private Mode mode = Mode.NORMAL;
    public event EventHandler<UI.Command.CommandEventArgs> raiseCommandEvent;

    public CommandInterpreter() {
      Thread thread = new Thread(processIO);
      thread.Start();
      Console.WriteLine("started");
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

          ConsoleKeyInfo keyInfo = Console.ReadKey(true);

          switch ( keyInfo.Key ) {
            case ConsoleKey.Escape:
              mode = Mode.NORMAL;
              Trace.WriteLine("Normal mode activated");
              continue;
            case ConsoleKey.I:
              mode = Mode.INSERT;
              Trace.WriteLine("Insert mode activated");
              continue;
          }

          if ( mode == Mode.NORMAL ) {

            switch ( keyInfo.Key ) {
              case ConsoleKey.Tab:
                raiseCommandEvent(this, new CommandEventArgs(Command.SWITCH_VIEW));
                break;
              case ConsoleKey.H:
                raiseCommandEvent(this, new CommandEventArgs(Command.MOVE_LEFT));
                break;
              case ConsoleKey.L:
                raiseCommandEvent(this, new CommandEventArgs(Command.MOVE_RIGHT));
                break;
              case ConsoleKey.K:
                raiseCommandEvent(this, new CommandEventArgs(Command.MOVE_UP));
                break;
              case ConsoleKey.J:
                raiseCommandEvent(this, new CommandEventArgs(Command.MOVE_DOWN));
                break;
              case ConsoleKey.Spacebar:
                raiseCommandEvent(this, new CommandEventArgs(Command.SWAP_ORIENTATION));
                break;
            }

          } else if ( mode == Mode.INSERT ) {

            char c = ((char)keyInfo.Key);

            if ( c > 31 && c < 127 ) {
              raiseCommandEvent(this, new CommandEventArgs(Command.INSERT_CHAR,keyInfo.Key));
            }

            if ( keyInfo.Key == ConsoleKey.Backspace ) {
              raiseCommandEvent(this, new CommandEventArgs(Command.DEL_CHAR));
            }



          }

        }

      }
    }

  }

}
