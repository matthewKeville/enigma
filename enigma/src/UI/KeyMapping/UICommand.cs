
namespace UI.KeyMapping {

public enum UICommandType {

    //////////////////////////////////////////
    //GridView Commands
    //////////////////////////////////////////

    //Normal Mode Commands
    ENTER_NORMAL_MODE,
    SWAP_ORIENTATION,

    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT,
    MOVE_RIGHT,

    MOVE_CLUE,
    MOVE_NEXT_CLUE,
    MOVE_PREV_CLUE,
    MOVE_CLUE_START,
    MOVE_CLUE_END,

    FIND_CHAR,
    FIND_REV_CHAR,

    REPLACE_CHAR,
    DELETE_CHAR,

    DELETE_WORD,
    DELETE_INNER_WORD,
    CHANGE_WORD,
    CHANGE_INNER_WORD,

    CHECK_CHAR,
    CHECK_WORD,
    CHECK_PUZZLE,

    TOGGLE_CLUES_VIEW,
    SHOW_KEYBINDS,
    EXIT_PUZZLE,

    //Insert Mode Commands

    ENTER_INSERT_MODE,
    INSERT_CHAR,
    DELETE_CHAR_INS


  }

  public interface UICommandArgs {}

  public class ReplaceCharArgs : UICommandArgs {
    public char C;
    public ReplaceCharArgs(char c) {
      this.C = c;
    }
  }

  public class InsertCharArgs : UICommandArgs {
    public char C;
    public InsertCharArgs(char c) {
      this.C = c;
    }
  }

  public class FindCharArgs : UICommandArgs {
    public char C;
    public FindCharArgs(char c) {
      this.C = c;
    }
  }

  public class MoveClueArgs : UICommandArgs {
    public int I;
    public MoveClueArgs(int i) {
      this.I = i;
    }
  }

  public class UICommand : EventArgs {
    public UICommandType Type;
    public UICommandArgs? Args;
    public UICommand(UICommandType type){
      this.Type = type;
    }
    public UICommand(UICommandType type,UICommandArgs args){
      this.Type = type;
      this.Args = args;
    }
  }

  public class UICommands {
    public static List<UICommandType> NormalCommands { get; } = new () {
      UICommandType.ENTER_NORMAL_MODE,
      UICommandType.SWAP_ORIENTATION,

      UICommandType.MOVE_UP,
      UICommandType.MOVE_DOWN,
      UICommandType.MOVE_LEFT,
      UICommandType.MOVE_RIGHT,

      UICommandType.MOVE_CLUE,
      UICommandType.MOVE_NEXT_CLUE,
      UICommandType.MOVE_PREV_CLUE,
      UICommandType.MOVE_CLUE_START,
      UICommandType.MOVE_CLUE_END,

      UICommandType.FIND_CHAR,
      UICommandType.FIND_REV_CHAR,

      UICommandType.REPLACE_CHAR,
      UICommandType.DELETE_CHAR,

      UICommandType.DELETE_WORD,
      UICommandType.DELETE_INNER_WORD,
      UICommandType.CHANGE_WORD,
      UICommandType.CHANGE_INNER_WORD,

      UICommandType.CHECK_CHAR,
      UICommandType.CHECK_WORD,
      UICommandType.CHECK_PUZZLE,
    };

    public static List<UICommandType> InsertCommands { get; } = new () {
      UICommandType.ENTER_INSERT_MODE,
      UICommandType.INSERT_CHAR,
      UICommandType.DELETE_CHAR_INS
    };
  }

}
