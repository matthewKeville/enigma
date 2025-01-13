using Terminal.Gui;
using UI.KeyMapping;

namespace UI.KeyMaping {

  public class KeyMaps {

        public List<(List<Key>,UICommand)> NormalKeyMaps;
        public List<(List<Key>,UICommand)> InsertKeyMaps;

        public KeyMaps() {
          NormalKeyMaps = buildNormalKeyMaps();
          InsertKeyMaps = BuildInsertKeyMaps();
        }

        private List<(List<Key>,UICommand)> buildNormalKeyMaps() {
          List<(List<Key>,UICommand)> normalKeyMaps = new () {

            (new List<Key>() { Key.I },new UICommand(UICommandType.ENTER_INSERT_MODE,"Enter Insert Mode")),

            (new List<Key>() { Key.Space },new UICommand(UICommandType.SWAP_ORIENTATION,"Switch Clue Orientation")),


            (new List<Key>() { Key.K },new UICommand(UICommandType.MOVE_UP,"Move Cursor Up")),
            (new List<Key>() { Key.J },new UICommand(UICommandType.MOVE_DOWN,"Move Cursor down")),
            (new List<Key>() { Key.H },new UICommand(UICommandType.MOVE_LEFT,"Move Cursor Left")),
            (new List<Key>() { Key.L },new UICommand(UICommandType.MOVE_RIGHT,"Move Cursor Right")),

            (new List<Key>() { Key.W },new UICommand(UICommandType.MOVE_NEXT_CLUE,"Move to next clue w/ respect to orientation")),
            (new List<Key>() { Key.B },new UICommand(UICommandType.MOVE_PREV_CLUE,"Move to prev clue w/ respect to orientation")),

            // C# Console.Driver limitation, perhaps down the line, the application can
            // request the use of curses driver, and depending on the underlying implementation
            // we can enable the preferred mapping
            // (new List<Key>() { Key.D4.WithShift },new UICommand(UICommandType.MOVE_CLUE_END)),
            // (new List<Key>() { Key.D6.WithShift },new UICommand(UICommandType.MOVE_CLUE_START)),
            // these now conflict <D><D>gg
            // (new List<Key>() { Key.D4},new UICommand(UICommandType.MOVE_CLUE_END)),
            // (new List<Key>() { Key.D6},new UICommand(UICommandType.MOVE_CLUE_START)),
          

            (new List<Key>() { Key.X },new UICommand(UICommandType.DELETE_CHAR,"Delete the character over the cursor")),
            (new List<Key>() { Key.D, Key.W },new UICommand(UICommandType.DELETE_WORD,"Delete a word")),
            (new List<Key>() { Key.D, Key.I, Key.W },new UICommand(UICommandType.DELETE_INNER_WORD,"Delete inner word")),
            (new List<Key>() { Key.C, Key.W},new UICommand(UICommandType.CHANGE_WORD,"Change word")),
            (new List<Key>() { Key.C, Key.I, Key.W },new UICommand(UICommandType.CHANGE_INNER_WORD,"Change inner word")),

            (new List<Key>() { Key.Tab },new UICommand(UICommandType.TOGGLE_CLUES_VIEW,"Toggle Clues View (Split/Single)")),
            (new List<Key>() { Key.Z, Key.Z },new UICommand(UICommandType.EXIT_PUZZLE,"Exit & Save Puzzle")),
            
            (new List<Key>() { Key.Z, Key.C },new UICommand(UICommandType.CHECK_CHAR,"Check the cell under the cursor")),
            (new List<Key>() { Key.Z, Key.W },new UICommand(UICommandType.CHECK_WORD,"Check the active word")),
            (new List<Key>() { Key.Z, Key.P },new UICommand(UICommandType.CHECK_PUZZLE,"Check every word")),

            (new List<Key>() { Key.F1 },new UICommand(UICommandType.SHOW_KEYBINDS,"Show this page")),

          };

          //r<key>
          foreach ( int x  in Enumerable.Range(0,26)) {
            //tolerate ambigous case for r? input
            normalKeyMaps.Add( 
              (new List<Key>() { Key.R, new Key((char)(x+65)) } ,
                new UICommand(
                  UICommandType.REPLACE_CHAR,
                  new ReplaceCharArgs((char)(x+65))
                )
              )
            );
            normalKeyMaps.Add( 
              (new List<Key>() { Key.R, new Key((char)(x+97)) } ,
                new UICommand(
                  UICommandType.REPLACE_CHAR,
                  new ReplaceCharArgs((char)(x+65))
                )
              )
            );
          }

          //f<key>
          foreach ( int x  in Enumerable.Range(0,26)) {
            //tolerate ambigous case for f? input
            normalKeyMaps.Add( 
              (new List<Key>() { Key.F, new Key((char)(x+65)) } ,
                new UICommand(
                  UICommandType.FIND_CHAR,
                  new FindCharArgs((char)(x+65))
                )
              )
            );
            normalKeyMaps.Add( 
              (new List<Key>() { Key.F, new Key((char)(x+97)) } ,
                new UICommand(
                  UICommandType.FIND_CHAR,
                  new FindCharArgs((char)(x+65))
                )
              )
            );
          }

          //F<key>
          foreach ( int x  in Enumerable.Range(0,26)) {
            //tolerate ambigous case for f? input
            normalKeyMaps.Add( 
              (new List<Key>() { Key.F.WithShift, new Key((char)(x+65)) } ,
                new UICommand(
                  UICommandType.FIND_REV_CHAR,
                  new FindCharArgs((char)(x+65))
                )
              )
            );
            normalKeyMaps.Add( 
              (new List<Key>() { Key.F.WithShift, new Key((char)(x+97)) } ,
                new UICommand(
                  UICommandType.FIND_REV_CHAR,
                  new FindCharArgs((char)(x+65))
                )
              )
            );
          }

          // gg<num> or gg<num1><num2>
          foreach ( int x  in Enumerable.Range(0,9)) {
            foreach ( int y  in Enumerable.Range(0,9)) {
                
              List<Key> seq = new ();
              if ( x != 0 ) {
                seq.Add( new Key((char)(48+x)) );
              }
              seq.Add( new Key((char)(48+y)) );
              seq.Add( Key.G );
              seq.Add( Key.G );

              normalKeyMaps.Add( 
                (seq,
                  new UICommand(
                    UICommandType.MOVE_CLUE,
                    new MoveClueArgs(Int32.Parse($"{x}{y}"))
                  )
                )
              );
            }
          }

          return normalKeyMaps;

        }

        private List<(List<Key>,UICommand)> BuildInsertKeyMaps() {

          //Terminal.Gui.Key

          List<(List<Key>,UICommand)> insertKeyMaps = new () {
            (new List<Key>() { Key.Esc },new UICommand(UICommandType.ENTER_NORMAL_MODE,"Enter Normal Mode")),
            //Grrr I want to do <C-]> but it's not supported by Console.ReadKey ... Key
          };

          //tolerate ambigous case for insert input
          foreach ( int x  in Enumerable.Range(0,26)) {
            insertKeyMaps.Add( 
              (new List<Key>() { new Key((char)(x+65)) } ,
                new UICommand(
                  UICommandType.INSERT_CHAR,
                  new InsertCharArgs((char)(x+65))
                )
              )
            );
            insertKeyMaps.Add( 
              (new List<Key>() { new Key((char)(x+97)) } ,
                new UICommand(
                  UICommandType.INSERT_CHAR,
                  new InsertCharArgs((char)(x+65))
                )
              )
            );
          }

          insertKeyMaps.Add( 
            (new List<Key>() { Key.Backspace } , new UICommand(UICommandType.DELETE_CHAR_INS,"Delete character"))
          );

          return insertKeyMaps;
        }

    }
}
