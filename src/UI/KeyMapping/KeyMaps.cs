using Terminal.Gui;
using UI.KeyMapping;

namespace UI.KeyMaping {

  //KeyMap is an abstraction over the pairing between UICommands &
  //and the List<Key> sequences that trigger them. The motivation here
  //is to allow the user to customize against KeyMap and use (List<Key>, UICommand)
  //under the hood, so the user isn't exposed to the implementation of parametric 
  //mappings, which generate a set of (List<Key>, UICommand) for each possible
  //input.
  public abstract class KeyMap {
    public List<(List<Key>,UICommand)> Bindings = new();
    public String Description = "";
  }

  public class FixedKeyMap : KeyMap {
    public List<Key> Sequence = new();
    
    public FixedKeyMap(List<Key> sequence,UICommandType commandType,String description = "") {
      Sequence = sequence;
      Bindings.Add((sequence,new UICommand(commandType)));
      Description = description;
    }
  }

  public class ParametricKeyMap : KeyMap {
    public List<Key> PrinicpalSequence = new ();
    public String ArgSpec = "";
    public bool Posterior;

    public ParametricKeyMap(List<Key> principalSequence,UICommandType commandType, List<(List<Key>,UICommandArgs)> variations, bool posterior = true, String description = "", String argSpec = "") {
      PrinicpalSequence = principalSequence;
      foreach ( (List<Key> terminalSequence,UICommandArgs args) in variations ) {
        //variations after principal
        if ( posterior ) {
          List<Key> sequence = new(principalSequence);
          sequence.AddRange(terminalSequence);
          UICommand command = new UICommand(commandType,args);
          Bindings.Add((sequence,command));
        //variations before principal
        } else {
          List<Key> sequence = new(terminalSequence);
          sequence.AddRange(principalSequence);
          UICommand command = new UICommand(commandType,args);
          Bindings.Add((sequence,command));
        }
      }
      Description = description;
      ArgSpec = argSpec;
      Posterior = posterior;
    }

    public static ParametricKeyMap InsertCharKeyMapOfPrincipal(List<Key> principalSequence) {

      List<(List<Key>,UICommandArgs)> variations = new();
      variations.AddRange(Enumerable.Range(0,26).Select( x => 
          (new List<Key>() { new Key((char)(x+65)) }, (UICommandArgs) new InsertCharArgs((char)(x+65)))
      ));
      variations.AddRange(Enumerable.Range(0,26).Select( x => 
          (new List<Key>() { new Key((char)(x+97)) }, (UICommandArgs) new InsertCharArgs((char)(x+65)))
      ));

      return new ParametricKeyMap(principalSequence,UICommandType.INSERT_CHAR,variations,true,"Write char","<char>");

    }

    public static ParametricKeyMap FindCharKeyMapOfPrincipal(List<Key> principalSequence,bool rev) {

      List<(List<Key>,UICommandArgs)> variations = new();
      variations.AddRange(Enumerable.Range(0,26).Select( x => 
          (new List<Key>() { new Key((char)(x+65)) }, (UICommandArgs) new FindCharArgs((char)(x+65)))
      ));
      variations.AddRange(Enumerable.Range(0,26).Select( x => 
          (new List<Key>() { new Key((char)(x+97)) }, (UICommandArgs) new FindCharArgs((char)(x+65)))
      ));

      return rev ? 
        new ParametricKeyMap(principalSequence,UICommandType.FIND_REV_CHAR,variations,true,"Find <char> in word backwards","<char>") :
        new ParametricKeyMap(principalSequence,UICommandType.FIND_CHAR,variations,true,"Find <char> in word","<char>");
    }

    public static ParametricKeyMap ReplaceCharKeyMapOfPrincipal(List<Key> principalSequence) {

      List<(List<Key>,UICommandArgs)> variations = new();
      variations.AddRange(Enumerable.Range(0,26).Select( x => 
          (new List<Key>() { new Key((char)(x+65)) }, (UICommandArgs) new ReplaceCharArgs((char)(x+65)))
      ));
      variations.AddRange(Enumerable.Range(0,26).Select( x => 
          (new List<Key>() { new Key((char)(x+97)) }, (UICommandArgs) new ReplaceCharArgs((char)(x+65)))
      ));

      return new ParametricKeyMap(principalSequence,UICommandType.REPLACE_CHAR,variations,true,"Replace cursor with <char>","<char>");

    }

    public static ParametricKeyMap MoveWordKeyMapOfPrincipal(List<Key> principalSequence) {

      List<(List<Key>,UICommandArgs)> variations = new();
      foreach ( int x  in Enumerable.Range(0,9)) {
        foreach ( int y  in Enumerable.Range(0,9)) {
          List<Key> sequence = new ();
          if ( x != 0 ) {
            sequence.Add( new Key((char)(48+x)) );
          }
          sequence.Add( new Key((char)(48+y)) );
          variations.Add(
            (sequence, (UICommandArgs) new MoveClueArgs(x != 0 ? Int32.Parse($"{x}{y}") : y)
          ));
        }
      }
      return new ParametricKeyMap(principalSequence,UICommandType.MOVE_CLUE,variations,false,"Move to orientation clue <num>(<num>)","<num>(<num>)");
    }

  }

  public class KeyMaps {

        public List<KeyMap> NormalKeyMaps = new ();
        public List<KeyMap> InsertKeyMaps = new ();

        public KeyMaps() {
          NormalKeyMaps = buildNormalKeyMaps();
          InsertKeyMaps = buildInsertKeyMaps();
        }

        private List<KeyMap> buildNormalKeyMaps() {

          return new List<KeyMap>() {
            new FixedKeyMap( new List<Key>() { Key.I },UICommandType.ENTER_INSERT_MODE,"Enter insert mode"),
            new FixedKeyMap( new List<Key>() { Key.Space }, UICommandType.SWAP_ORIENTATION,"Swap orientation"),
            new FixedKeyMap( new List<Key>() { Key.Tab }, UICommandType.TOGGLE_CLUES_VIEW,"Toggle Split/Single view"),
            new FixedKeyMap( new List<Key>() { Key.F1 }, UICommandType.SHOW_KEYBINDS,"Show this page"),
            new FixedKeyMap( new List<Key>() { Key.K }, UICommandType.MOVE_UP,"Move up"),
            new FixedKeyMap( new List<Key>() { Key.J }, UICommandType.MOVE_DOWN,"Move down"),
            new FixedKeyMap( new List<Key>() { Key.H }, UICommandType.MOVE_LEFT,"Move left"),
            new FixedKeyMap( new List<Key>() { Key.L }, UICommandType.MOVE_RIGHT,"Move right"),
            // // C# Console.Driver limitation, perhaps down the line, the application can
            // // request the use of curses driver, and depending on the underlying implementation
            // // we can enable the preferred mapping
            // { Key.D4.WithShift },new UICommand(UICommandType.MOVE_CLUE_END)),
            // { Key.D6.WithShift },new UICommand(UICommandType.MOVE_CLUE_START)),
            new FixedKeyMap( new List<Key>() { Key.G, Key.D0 }, UICommandType.MOVE_CLUE_START,"Move to start of clue"),
            new FixedKeyMap( new List<Key>() { Key.G, Key.D4 }, UICommandType.MOVE_CLUE_END,"Move to end of clue"),
            new FixedKeyMap( new List<Key>() { Key.W }, UICommandType.MOVE_NEXT_CLUE,"Move to next clue"),
            new FixedKeyMap( new List<Key>() { Key.B }, UICommandType.MOVE_PREV_CLUE,"Move to prev clue"),
            new FixedKeyMap( new List<Key>() { Key.X }, UICommandType.DELETE_CHAR,"Delete character under cursor"),
            new FixedKeyMap( new List<Key>() { Key.D, Key.W }, UICommandType.DELETE_WORD,"Delete word"),
            new FixedKeyMap( new List<Key>() { Key.D, Key.I, Key.W }, UICommandType.DELETE_INNER_WORD,"Delete inner word"),
            new FixedKeyMap( new List<Key>() { Key.C, Key.W }, UICommandType.CHANGE_WORD,"Change word"),
            new FixedKeyMap( new List<Key>() { Key.C, Key.I, Key.W }, UICommandType.CHANGE_INNER_WORD,"Change inner word"),
            new FixedKeyMap( new List<Key>() { Key.Z, Key.C }, UICommandType.CHECK_CHAR,"Check char"),
            new FixedKeyMap( new List<Key>() { Key.Z, Key.W }, UICommandType.CHECK_WORD,"Check word"),
            new FixedKeyMap( new List<Key>() { Key.Z, Key.P }, UICommandType.CHECK_PUZZLE,"Check puzzle"),
            new FixedKeyMap( new List<Key>() { Key.Z, Key.Z }, UICommandType.EXIT_PUZZLE,"Save & Exit"),
            ParametricKeyMap.FindCharKeyMapOfPrincipal( new List<Key>(){ Key.F },false ),
            ParametricKeyMap.FindCharKeyMapOfPrincipal( new List<Key>(){ Key.F.WithShift },true ),
            ParametricKeyMap.ReplaceCharKeyMapOfPrincipal( new List<Key>(){ Key.R }),
            ParametricKeyMap.MoveWordKeyMapOfPrincipal( new List<Key>(){ Key.G, Key.G })
          };

        }

        private List<KeyMap> buildInsertKeyMaps() {
          return new List<KeyMap>() {
            new FixedKeyMap( new List<Key>() { Key.Esc },UICommandType.ENTER_NORMAL_MODE,"Enter normal mode"),
            new FixedKeyMap( new List<Key>() { Key.Backspace },UICommandType.DELETE_CHAR_INS,"Delete char"),
           //Grrr I want to do <C-]> but it's not supported by Console.ReadKey ... Key
            ParametricKeyMap.InsertCharKeyMapOfPrincipal( new List<Key>(){} )
          };
        }

    }
}
