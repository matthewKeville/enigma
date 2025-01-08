namespace UI.View.Game
{

    using System.Drawing;
    using System.Text;
    using Entity;
    using Enums;
    using Event;
    using Settings.Theme;
    using Terminal.Gui;
    using UI.KeyMapping;

    public class GridView : Toplevel
    {
        private DatabaseContext _dbContext;
        private EventBus _eventBus;
        private GridModel? _gridModel;
        private Crossword? _crossword;
        private bool _isInsertMode = false;
        private KeySequenceInterpreter _normalKeySequenceInterpreter;
        private KeySequenceInterpreter _insertKeySequenceInterpreter;
        private Theme _theme;

        private int gridOffX = 3;
        private int gridOffY = 3;

        public GridView(DatabaseContext dbContext, EventBus eventBus,Theme  theme)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _eventBus.Register(this, (args) =>
            {
                if (args is StartPuzzleEventArgs)
                {
                    OnStartPuzzleEvent((StartPuzzleEventArgs)args);
                }
            });

            _eventBus.Register(this, (eventArgs) =>
            {
                if (eventArgs is StartPuzzleEventArgs)
                {
                    SetFocus();
                }
            }
            );
            _theme = theme;

            KeyBindings.Clear();

            _normalKeySequenceInterpreter = new KeySequenceInterpreter(BuildNormalKeyMaps());
            _insertKeySequenceInterpreter = new KeySequenceInterpreter(BuildInsertKeyMaps());

            setupView();

        }

        public override bool OnKeyDown(Key key)
        {

          (bool partialMatch,UICommand? command) = _isInsertMode ? 
            _insertKeySequenceInterpreter.ProcessKey(key) :
            _normalKeySequenceInterpreter.ProcessKey(key) ;

          if ( !partialMatch ) {
            return false;
          }
          if ( partialMatch && command is null) {
            return true;
          } 

          var activeCluesStart = _gridModel.GetActiveClues();
          var orientationStart = _gridModel.Orientation;

          switch ( command.Type ) {

            //////////////////////////////////////////
            //Normal
            //////////////////////////////////////////
            
            case UICommandType.ENTER_INSERT_MODE:
              _isInsertMode = true;
              break;

            case UICommandType.SWAP_ORIENTATION:
              _gridModel.SwapOrientation();
              break;

            case UICommandType.MOVE_UP:
              _gridModel.MoveUp();
              break;
            case UICommandType.MOVE_DOWN:
              _gridModel.MoveDown();
              break;
            case UICommandType.MOVE_LEFT:
              _gridModel.MoveLeft();
              break;
            case UICommandType.MOVE_RIGHT:
              _gridModel.MoveRight();
              break;
            case UICommandType.MOVE_NEXT_CLUE:
              _gridModel.MoveNextClue();
              break;
            case UICommandType.MOVE_PREV_CLUE:
              _gridModel.MovePrevClue();
              break;
            case UICommandType.MOVE_CLUE_END:
              _gridModel.MoveClueEnd();
              break;
            case UICommandType.MOVE_CLUE_START:
              _gridModel.MoveClueStart();
              break;
            case UICommandType.MOVE_CLUE:
              MoveClueArgs moveClueArgs = (MoveClueArgs) command.Args!;
              _gridModel.MoveClue(moveClueArgs.I);
              break;
            case UICommandType.FIND_CHAR:
              FindCharArgs findCharArgs = (FindCharArgs) command.Args!;
              _gridModel.FindChar(findCharArgs.C);
              break;
            case UICommandType.FIND_REV_CHAR:
              FindCharArgs findRevCharArgs = (FindCharArgs) command.Args!;
              _gridModel.FindReverseChar(findRevCharArgs.C);
              break;

            case UICommandType.REPLACE_CHAR:
              ReplaceCharArgs replaceCharArgs = (ReplaceCharArgs) command.Args!;
              _gridModel.ReplaceChar(replaceCharArgs.C);
              break;
            case UICommandType.DELETE_CHAR:
              _gridModel.DeleteChar();
              break;
            case UICommandType.DELETE_WORD:
              _gridModel.DeleteWord();
              break;
            case UICommandType.DELETE_INNER_WORD:
              _gridModel.DeleteInnerWord();
              break;
            //Not loving this, perhaps the mode change should be captured in
            //the model...
            case UICommandType.CHANGE_WORD:
              _gridModel.DeleteWord();
              _isInsertMode = true;
              break;
            case UICommandType.CHANGE_INNER_WORD:
              _gridModel.DeleteInnerWord();
              _isInsertMode = true;
              break;

            //////////////////////////////////////////
            //Insert
            //////////////////////////////////////////
            
            case UICommandType.ENTER_NORMAL_MODE:
              _isInsertMode = false;
              break;


            case UICommandType.INSERT_CHAR:
              InsertCharArgs insertCharArgs = (InsertCharArgs) command.Args!;
              _gridModel.InsertChar(insertCharArgs.C);
              break;

            case UICommandType.DELETE_CHAR_INS:
              _gridModel.DeleteChar(true);
              break;


            default:
              Trace.WriteLine("Unhandled command type : " + command.Type.ToString());
              break;
          }

          var activeCluesEnd = _gridModel.GetActiveClues();

          if (activeCluesStart != activeCluesEnd ) { 
            Trace.WriteLine("Active clues have changed");
            Trace.WriteLine($"Across Clue {activeCluesEnd.Item1.X} {activeCluesEnd.Item1.Y} {activeCluesEnd.Item1.I}");
            Trace.WriteLine($"Down Clue {activeCluesEnd.Item2.X} {activeCluesEnd.Item2.Y} {activeCluesEnd.Item2.I}");
              _eventBus.PostEvent(new FocusClueChangeEventArgs((activeCluesEnd.Item1.I,activeCluesEnd.Item2.I)));
          }

          var orientationEnd = _gridModel.Orientation;
          if (orientationEnd != orientationStart ) { 
            Trace.WriteLine("Orientation has changed");
              _eventBus.PostEvent(new OrientationChangeEventArgs(orientationEnd));
          }

          SetNeedsDisplay();
          return true;

        }

        public override void OnDrawContent(Rectangle contentArea)
        {

            base.OnDrawContent(contentArea);
            Driver.FillRect(contentArea,' ');

            foreach (GridCharModel gcm in _gridModel.GridCharModels)
            {
       
                Rune rune;
                Attribute attr;

                //block
                if (gcm.IsBlock) {

                  rune = new Rune(_theme.BlockChar);
                  attr = new Terminal.Gui.Attribute(_theme.BlockFG,_theme.BlockBG);

                //emtpy
                } else if ( gcm.C == ' ' ) {

                  if ( _gridModel.Selection.Equals(gcm) ) {
                    rune = new Rune(_theme.CursorEmptyHighlightChar);
                    attr = new Terminal.Gui.Attribute(_theme.CursorEmptyHighlightFG,_theme.CursorEmptyHighlightBG);
                  }

                  else if ( _gridModel.ActiveWordChars().Contains(gcm) ) {
                    rune = _gridModel.Orientation == Direction.Across 
                      ? new Rune(_theme.ActiveEmptyHighlightAcrossChar)
                      : new Rune(_theme.ActiveEmptyHighlightDownChar);
                    attr = new Terminal.Gui.Attribute(_theme.ActiveEmptyHighlightFG,_theme.ActiveEmptyHighlightBG);
                  } 

                  else if ( _gridModel.CrossWordChars().Contains(gcm) ) {
                    rune = _gridModel.Orientation == Direction.Across 
                      ? new Rune(_theme.CrossEmptyHighlightDownChar)
                      : new Rune(_theme.CrossEmptyHighlightAcrossChar);
                    attr = new Terminal.Gui.Attribute(_theme.CrossEmptyHighlightFG,_theme.CrossEmptyHighlightBG);
                  } 

                  else {
                    rune = new Rune(_theme.CellEmptyChar);
                    attr = new Terminal.Gui.Attribute(_theme.CellFG,_theme.CellBG);
                  }

                //filled
                } else {

                  rune = new Rune(gcm.C);

                  if ( _gridModel.Selection.Equals(gcm) ) {
                    attr = new Terminal.Gui.Attribute(_theme.CursorHighlightFG,_theme.CursorHighlightBG);
                  }

                  else if ( _gridModel.ActiveWordChars().Contains(gcm) ) {
                    attr = new Terminal.Gui.Attribute(_theme.ActiveHighlightFG,_theme.ActiveHighlightBG);
                  } 

                  else if ( _gridModel.CrossWordChars().Contains(gcm) ) {
                    attr = new Terminal.Gui.Attribute(_theme.CrossHighlightFG,_theme.CrossHighlightBG);
                  } 

                  else {
                    attr = new Terminal.Gui.Attribute(_theme.CellFG,_theme.CellBG);
                  }

                }

                ////////
                //Rune
                ////////

                Move(gcm.X + gridOffX, gcm.Y + gridOffY);
                Driver.SetAttribute(attr);
                Driver.AddRune(rune);

            }

        }

        private void Init(int crosswordId)
        {
            _gridModel = new GridModel(
                _dbContext.GridChars.Where(gc => gc.CrosswordId == crosswordId).ToList(),
                _dbContext.Words.Where(w => w.CrosswordId == crosswordId).ToList()
            );
            _crossword = _dbContext.Crosswords.Where( c => c.Id == crosswordId ).FirstOrDefault();
            Width = _crossword.Columns + (gridOffX * 2);
            Height = _crossword.Rows + (gridOffY * 2);
            SetNeedsDisplay();
        }

        private void OnStartPuzzleEvent(StartPuzzleEventArgs args)
        {
            Init(args.CrosswordId);
        }

        private List<(List<Key>,UICommand)> BuildNormalKeyMaps() {
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

        private void setupView() {
          this.ColorScheme = new ColorScheme(new Attribute(_theme.GridBackgroundBG));
        }

    }

}
