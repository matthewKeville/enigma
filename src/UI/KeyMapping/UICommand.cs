
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

    //Insert Mode Commands

    ENTER_INSERT_MODE,
    INSERT_CHAR,
    DELETE_CHAR_INS,

    //

    TOGGLE_CLUES_VIEW,
    EXIT_PUZZLE

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

}
