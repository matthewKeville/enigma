namespace UI.Game
{

    using System.Drawing;
    using System.Text;
    using Enums;
    using Event;
    using Terminal.Gui;
    using UI.Model.Game;
    using static Event.StartPuzzleEventArgs;

    public class GridView : Window
    {
        private DatabaseContext _dbContext;
        private EventBus _eventBus;
        private GridModel _gridModel;
        private bool _isInsertMode = false;
        private KeySequenceInterpreter _normalKeySequenceInterpreter;
        private KeySequenceInterpreter _insertKeySequenceInterpreter;

        public GridView(DatabaseContext dbContext, EventBus eventBus)
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

            KeyBindings.Clear();

            List<(List<Key>,UICommand)> normalKeyMaps = new () {
              (new List<Key>() { Key.I },new UICommand(UICommandType.ENTER_INSERT_MODE)),

              (new List<Key>() { Key.Space },new UICommand(UICommandType.SWAP_ORIENTATION)),

              (new List<Key>() { Key.K },new UICommand(UICommandType.MOVE_UP)),
              (new List<Key>() { Key.J },new UICommand(UICommandType.MOVE_DOWN)),
              (new List<Key>() { Key.H },new UICommand(UICommandType.MOVE_LEFT)),
              (new List<Key>() { Key.L },new UICommand(UICommandType.MOVE_RIGHT)),

              (new List<Key>() { Key.X },new UICommand(UICommandType.DELTE_CHAR)),

            };

            //tolerate ambigous case for r? input
            foreach ( int x  in Enumerable.Range(0,26)) {
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

            _normalKeySequenceInterpreter = new KeySequenceInterpreter(normalKeyMaps);

            List<(List<Key>,UICommand)> insertKeyMaps = new () {
              (new List<Key>() { Key.Esc },new UICommand(UICommandType.ENTER_NORMAL_MODE)),
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

            _insertKeySequenceInterpreter = new KeySequenceInterpreter(insertKeyMaps);
        }

        public override bool OnKeyDown(Key key)
        {

          UICommand? command = _isInsertMode ? 
            _insertKeySequenceInterpreter.ProcessKey(key) :
            _normalKeySequenceInterpreter.ProcessKey(key) ;

          if ( command is null ) {
            Trace.WriteLine("no command");
            return true;
          } 

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

            case UICommandType.REPLACE_CHAR:
              ReplaceCharArgs replaceCharArgs = (ReplaceCharArgs) command.Args!;
              _gridModel.ReplaceChar(replaceCharArgs.C);
              break;
            case UICommandType.DELTE_CHAR:
              _gridModel.DeleteChar();
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


            default:
              Trace.WriteLine("Unhandled command type : " + command.Type.ToString());
              break;
          }

          SetNeedsDisplay();
          return true;

          /**
            //context
            switch (key.KeyCode)
            {
                //prevent further key processing, with early return
                case KeyCode.I:
                    _isInsertMode = true;
                    return true;
                case KeyCode.Esc:
                    _isInsertMode = false;
                    return true;
                default:
                    break;
            }

            if (!_isInsertMode)
            {

                switch (key.KeyCode)
                {

                    //movement
                    case KeyCode.J:
                        _gridModel.MoveDown();
                        break;
                    case KeyCode.K:
                        _gridModel.MoveUp();
                        break;
                    case KeyCode.H:
                        _gridModel.MoveLeft();
                        break;
                    case KeyCode.L:
                        _gridModel.MoveRight();
                        break;

                    //editing
                    case KeyCode.X:
                        _gridModel.DeleteChar();
                        break;

                    //orientation
                    case KeyCode.Space:
                        _gridModel.SwapOrientation();
                        break;

                    //quit
                    case KeyCode.Q:
                        _eventBus.PostEvent(new EndPuzzleEventArgs());
                        break;

                    default:
                        return false;

                }

            }
            else
            {
                char insertChar = ((char)key.KeyCode);
                _gridModel.InsertChar(insertChar);
            }
            **/


        }

        public override void OnDrawContent(Rectangle contentArea)
        {

            base.OnDrawContent(contentArea);
            var ctx = Driver;

            List<GridCharModel> active = _gridModel.ActiveWordChars();

            foreach (GridCharModel gcm in _gridModel.GridCharModels)
            {

       
                Rune rune;
                Attribute attr;

                //block
                if (gcm.IsBlock) {

                  rune = new Rune('#');
                  attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.White,Terminal.Gui.Color.Blue);

                //emtpy
                } else if ( gcm.C == ' ' ) {

                  if ( _gridModel.Selection.Equals(gcm) ) {
                    rune = new Rune('*');
                    attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.Yellow,Terminal.Gui.Color.Blue);
                  }

                  else if ( _gridModel.ActiveWordChars().Contains(gcm) ) {
                    rune = _gridModel.Orientation == Direction.Across ? new Rune('-') : new Rune('|');
                    attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightMagenta,Terminal.Gui.Color.Blue);
                  } 

                  else {
                    rune = new Rune('·');
                    attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.White,Terminal.Gui.Color.Blue);
                  }

                //filled
                } else {

                  rune = new Rune(gcm.C);

                  if ( _gridModel.Selection.Equals(gcm) ) {
                    attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.Yellow,Terminal.Gui.Color.Blue);
                  }

                  else if ( _gridModel.ActiveWordChars().Contains(gcm) ) {
                    attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightMagenta,Terminal.Gui.Color.Blue);
                  } 

                  else {
                    attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.White,Terminal.Gui.Color.Blue);
                  }

                }

                ////////
                //Rune
                ////////

                Move(gcm.X, gcm.Y);
                Driver.SetAttribute(attr);
                ctx.AddRune(rune);

            }

        }

        private void Init(int crosswordId)
        {
            _gridModel = new GridModel(_dbContext.GridChars.Where(gc => gc.CrosswordId == crosswordId).ToList());
            SetNeedsDisplay();
        }

        private void OnStartPuzzleEvent(StartPuzzleEventArgs args)
        {
            Init(args.CrosswordId);
        }
    }

}
