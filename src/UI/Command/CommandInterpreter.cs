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

          ConsoleKeyInfo keyInfo = Console.ReadKey(false);

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

              case ConsoleKey.D6:
                raiseCommandEvent(this, new CommandEventArgs(Command.MOVE_WORD_START));
                break;

              case ConsoleKey.D4:
                raiseCommandEvent(this, new CommandEventArgs(Command.MOVE_WORD_END));
                break;

              case ConsoleKey.Spacebar:
                raiseCommandEvent(this, new CommandEventArgs(Command.SWAP_ORIENTATION));
                break;
              case ConsoleKey.D:
                raiseCommandEvent(this, new CommandEventArgs(Command.DEL_WORD));
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
