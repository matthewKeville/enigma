using Terminal.Gui;
using UI.KeyMapping;

namespace UI.KeyMaping {

  public class KeyMaps {

        public static List<(List<Key>,UICommand)> BuildNormalKeyMaps() {
          List<(List<Key>,UICommand)> normalKeyMaps = new () {

            (new List<Key>() { Key.I },new UICommand(UICommandType.ENTER_INSERT_MODE)),

            (new List<Key>() { Key.Space },new UICommand(UICommandType.SWAP_ORIENTATION)),


            (new List<Key>() { Key.K },new UICommand(UICommandType.MOVE_UP)),
            (new List<Key>() { Key.J },new UICommand(UICommandType.MOVE_DOWN)),
            (new List<Key>() { Key.H },new UICommand(UICommandType.MOVE_LEFT)),
            (new List<Key>() { Key.L },new UICommand(UICommandType.MOVE_RIGHT)),

            (new List<Key>() { Key.W },new UICommand(UICommandType.MOVE_NEXT_CLUE)),
            (new List<Key>() { Key.B },new UICommand(UICommandType.MOVE_PREV_CLUE)),

            // C# Console.Driver limitation, perhaps down the line, the application can
            // request the use of curses driver, and depending on the underlying implementation
            // we can enable the preferred mapping
            // (new List<Key>() { Key.D4.WithShift },new UICommand(UICommandType.MOVE_CLUE_END)),
            // (new List<Key>() { Key.D6.WithShift },new UICommand(UICommandType.MOVE_CLUE_START)),
            // these now conflict <D><D>gg
            // (new List<Key>() { Key.D4},new UICommand(UICommandType.MOVE_CLUE_END)),
            // (new List<Key>() { Key.D6},new UICommand(UICommandType.MOVE_CLUE_START)),
          

            (new List<Key>() { Key.X },new UICommand(UICommandType.DELETE_CHAR)),
            (new List<Key>() { Key.D, Key.W },new UICommand(UICommandType.DELETE_WORD)),
            (new List<Key>() { Key.D, Key.I, Key.W },new UICommand(UICommandType.DELETE_INNER_WORD)),
            (new List<Key>() { Key.C, Key.W},new UICommand(UICommandType.CHANGE_WORD)),
            (new List<Key>() { Key.C, Key.I, Key.W },new UICommand(UICommandType.CHANGE_INNER_WORD)),

            (new List<Key>() { Key.Tab },new UICommand(UICommandType.TOGGLE_CLUES_VIEW)),
            (new List<Key>() { Key.Z, Key.Z },new UICommand(UICommandType.EXIT_PUZZLE)),
            
            (new List<Key>() { Key.Z, Key.C },new UICommand(UICommandType.CHECK_CHAR)),
            (new List<Key>() { Key.Z, Key.W },new UICommand(UICommandType.CHECK_WORD)),
            (new List<Key>() { Key.Z, Key.G },new UICommand(UICommandType.CHECK_PUZZLE)),

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

        public static List<(List<Key>,UICommand)> BuildInsertKeyMaps() {

          //Terminal.Gui.Key

          List<(List<Key>,UICommand)> insertKeyMaps = new () {
            (new List<Key>() { Key.Esc },new UICommand(UICommandType.ENTER_NORMAL_MODE)),
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
            (new List<Key>() { Key.Backspace } , new UICommand(UICommandType.DELETE_CHAR_INS))
          );

          return insertKeyMaps;
        }

    }
}
