using Terminal.Gui;

namespace UI.KeyMapping {
public enum UICommandType {

    //Normal Mode Commands
    ENTER_NORMAL_MODE,

    SWAP_ORIENTATION,

    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT,
    MOVE_RIGHT,
    MOVE_NEXT_CLUE,
    MOVE_PREV_CLUE,

    REPLACE_CHAR,
    DELETE_CHAR,

    DELETE_WORD,
    DELETE_INNER_WORD,
    CHANGE_WORD,
    CHANGE_INNER_WORD,

    //Insert Mode Commands

    ENTER_INSERT_MODE,

    INSERT_CHAR,

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

    public UICommand? ProcessKey(Key key) {

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

        //Debug.WriteLine("no partial matches");

        _keyBuffer.Clear();
        return null;
      }

      //exact match?
      List<(List<Key>,UICommand)> exactMatches = partialMatches
        .FindAll( km => { return km.Item1.Count() == _keyBuffer.Count(); });

      if (exactMatches.Any()) {

        //Debug.WriteLine("hit keysequence matches " + exactMatches.Count());
        //exactMatches.ForEach( m => dumpSequence(m.Item1));

        _keyBuffer.Clear();
        return exactMatches[0].Item2;
      }

      return null;

    }
  }
}
