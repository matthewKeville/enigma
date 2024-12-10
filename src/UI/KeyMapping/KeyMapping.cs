using Terminal.Gui;

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

    //Insert Mode Commands

    ENTER_INSERT_MODE,

    INSERT_CHAR,

    //////////////////////////////////////////
    //CluesView Commands
    //////////////////////////////////////////

    TOGGLE_CLUES_SPLIT_VIEW

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

  public class UICommand {
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

  public class KeySequenceInterpreter {
    private List<Key> _keyBuffer = new ();
    public List<(List<Key>,UICommand)> keyMaps;

    private void dumpSequence( List<Key> sequence ) {
      String msg = "";
      sequence.ForEach( key => {
        msg += key.ToString() + " , ";
      });
      Debug.WriteLine(msg);
    }

    public KeySequenceInterpreter(List<(List<Key>,UICommand)> keyMaps) {
      this.keyMaps = keyMaps;
      this.keyMaps.ForEach( km => dumpSequence(km.Item1));
    }

    public (bool partialMatch, UICommand? command) ProcessKey(Key key) {

      _keyBuffer.Add(key);

      List<(List<Key>,UICommand)> partialMatches = keyMaps
        .FindAll( km => { return km.Item1.Count() >= _keyBuffer.Count(); } )
        .FindAll( km => {
          for ( int i = 0; i < _keyBuffer.Count(); i++ ) {
            if ( !km.Item1[i].Equals(_keyBuffer[i])) {
              return false;
            }
          }
          return true;
        });

      if (partialMatches.Count() == 0) {

        Debug.WriteLine("no command matches");
        dumpSequence(_keyBuffer);
        _keyBuffer.Clear();
        return (false,null);
      }

      //exact match?
      List<(List<Key>,UICommand)> exactMatches = partialMatches
        .FindAll( km => { return km.Item1.Count() == _keyBuffer.Count(); });

      if (exactMatches.Any()) {

        //Debug.WriteLine("hit keysequence matches " + exactMatches.Count());
        //exactMatches.ForEach( m => dumpSequence(m.Item1));

        _keyBuffer.Clear();
        return (true,exactMatches[0].Item2);
      }

      return (true,null);

    }
  }
}
