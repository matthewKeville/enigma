namespace UI.Commands {

  public class Command {
    public CommandType Type;
    public CommandMode Mode;
    public ConsoleKey? Key;

    public Command(CommandMode mode, CommandType type) {
      this.Mode = mode;
      this.Type = type;
    }

    public Command(CommandMode mode, CommandType type, ConsoleKey key) {
      this.Mode = mode;
      this.Type = type;
      this.Key = key;
    }

  }

  public enum  CommandType {
    LOAD_PUZZLE,
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
    INSTALL,
    EXIT
  }

  public enum CommandMode {
    META,
    INSERT,
    NORMAL
  }


}
