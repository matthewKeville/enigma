namespace UI.Command {

  public class Commands {

    public static List<Command> Meta { get; }  = new List<Command>() {
        Command.NORMAL_MODE,
        Command.INSERT_MODE
    };

    public static List<Command> Normal { get; } = new List<Command>() {
        Command.SWITCH_VIEW,
        Command.MOVE_LEFT,
        Command.MOVE_RIGHT,
        Command.MOVE_UP,
        Command.MOVE_DOWN,
        Command.MOVE_WORD_START,
        Command.MOVE_WORD_END,
        Command.MOVE_WORD,
        Command.MOVE_BACK_WORD,
        Command.CHECK_WORD,
        Command.SWAP_ORIENTATION,
        Command.SWAP_PANE,
        Command.DEL_WORD,
        Command.CONFIRM,
        Command.TOGGLE_HELP,
        Command.EXIT,
    };

    public static List<Command> Insert { get; } = new List<Command>() {
       Command.INSERT_CHAR,
       Command.DEL_CHAR
    };
  }

  public enum  Command {

    NORMAL_MODE,
    INSERT_MODE,
    SWITCH_VIEW,
    MOVE_LEFT,
    MOVE_RIGHT,
    MOVE_UP,
    MOVE_DOWN,
    MOVE_WORD_START,
    MOVE_WORD_END,
    MOVE_WORD,
    CHECK_WORD,
    MOVE_BACK_WORD,
    INSERT_CHAR,
    DEL_CHAR,
    SWAP_ORIENTATION,
    SWAP_PANE,
    DEL_WORD,
    CONFIRM,
    TOGGLE_HELP,
    EXIT

  }


}
