namespace UI.View.Game
{
    using Entity;
    using Event;
    using Terminal.Gui;
    using UI.KeyMaping;
    using UI.KeyMapping;
    using UI.Model;
    using UI.View.Game.Clues;

    public class GameView : Toplevel
    {

        private CluesView _cluesView;
        private GridView _gridView;
        private EventBus _eventBus;
        private KeySequenceInterpreter _insertKeySequenceInterpreter;
        private KeySequenceInterpreter _normalKeySequenceInterpreter;
        private DatabaseContext _dbContext;

        private bool _isInsertMode = false;
        private GameModel _gameModel;

        public GameView(CluesView cluesView, GridView gridView, EventBus eventBus, DatabaseContext dbContext)
        {

            _gridView = gridView;
            _cluesView = cluesView;
            _eventBus = eventBus;

            _gridView.X = 0;
            _gridView.Visible = true;

            _cluesView.X = Pos.Right(_gridView);
            _cluesView.Width = Dim.Fill();
            _cluesView.Visible = true;

            Add(gridView);
            Add(cluesView);

            _normalKeySequenceInterpreter = new KeySequenceInterpreter(KeyMaps.BuildNormalKeyMaps());
            _insertKeySequenceInterpreter = new KeySequenceInterpreter(KeyMaps.BuildInsertKeyMaps());

            _eventBus.Register(this, (args) =>
            {
                if (args is StartPuzzleEventArgs)
                {
                  OnStartPuzzleEvent((StartPuzzleEventArgs) args);
                }
            });

            _dbContext = dbContext;

        }

        public override bool OnKeyDown(Key key)
        {

            (bool partialMatch, UICommand? command) = _isInsertMode ?
              _insertKeySequenceInterpreter.ProcessKey(key) :
              _normalKeySequenceInterpreter.ProcessKey(key);

            if (command != null) {
              ProcessUICommand(command);
            }

            return true;

        }

        public void OnStartPuzzleEvent(StartPuzzleEventArgs args) {
          //build game model
          int crosswordId = args.CrosswordId;
          GridModel gridModel = new GridModel(
                _dbContext.GridChars.Where(gc => gc.CrosswordId == crosswordId).ToList(),
                _dbContext.Words.Where(w => w.CrosswordId == crosswordId).ToList(),
                _dbContext.Crosswords.First( c => c.Id == crosswordId ).Rows,
                _dbContext.Crosswords.First( c => c.Id == crosswordId ).Columns
          );
          _gameModel = new GameModel(crosswordId,gridModel);
          _eventBus.PostEvent(new PuzzleLoadedEventArgs(_gameModel));
        }

        public void SavePuzzle() {
          _gameModel.GridModel.GridCharModels.ForEach( gcm => {
            GridChar gc = _dbContext.GridChars.First( 
                gc => gc.CrosswordId == _gameModel.CrosswordId &&
                gc.X == gcm.X &&
                gc.Y == gcm.Y);
            gc.C = gcm.C;
            _dbContext.SaveChanges();
          });
        }

        public void PuzzleFinished() {
          SavePuzzle();
          MessageBox.Query(30, 5, "System", "Puzzle Complete", "Exit");
          Application.RequestStop();
        }


        public void ProcessUICommand(UICommand command)
        {

          var activeOrientationStart = _gameModel.GridModel.Orientation;
          var activeCluesStart = _gameModel.GridModel.GetActiveClues();
          var orientationStart = _gameModel.GridModel.Orientation;

          switch ( command.Type ) {

            //////////////////////////////////////////
            //Normal
            //////////////////////////////////////////

            case UICommandType.TOGGLE_CLUES_VIEW:
              _eventBus.PostEvent(new ClueViewChangeEventArgs());
              break;
            case UICommandType.SWAP_ORIENTATION:
              _gameModel.GridModel.SwapOrientation();
              break;
            case UICommandType.MOVE_UP:
              _gameModel.GridModel.MoveUp();
              break;
            case UICommandType.MOVE_DOWN:
              _gameModel.GridModel.MoveDown();
              break;
            case UICommandType.MOVE_LEFT:
              _gameModel.GridModel.MoveLeft();
              break;
            case UICommandType.MOVE_RIGHT:
              _gameModel.GridModel.MoveRight();
              break;
            case UICommandType.MOVE_NEXT_CLUE:
              _gameModel.GridModel.MoveNextClue();
              break;
            case UICommandType.MOVE_PREV_CLUE:
              _gameModel.GridModel.MovePrevClue();
              break;
            case UICommandType.MOVE_CLUE_END:
              _gameModel.GridModel.MoveClueEnd();
              break;
            case UICommandType.MOVE_CLUE_START:
              _gameModel.GridModel.MoveClueStart();
              break;
            case UICommandType.MOVE_CLUE:
              MoveClueArgs moveClueArgs = (MoveClueArgs) command.Args!;
              _gameModel.GridModel.MoveClue(moveClueArgs.I);
              break;
            case UICommandType.FIND_CHAR:
              FindCharArgs findCharArgs = (FindCharArgs) command.Args!;
              _gameModel.GridModel.FindChar(findCharArgs.C);
              break;
            case UICommandType.FIND_REV_CHAR:
              FindCharArgs findRevCharArgs = (FindCharArgs) command.Args!;
              _gameModel.GridModel.FindReverseChar(findRevCharArgs.C);
              break;
            case UICommandType.REPLACE_CHAR:
              ReplaceCharArgs replaceCharArgs = (ReplaceCharArgs) command.Args!;
              _gameModel.GridModel.ReplaceChar(replaceCharArgs.C);
              break;
            case UICommandType.DELETE_CHAR:
              _gameModel.GridModel.DeleteChar();
              break;
            case UICommandType.DELETE_WORD:
              _gameModel.GridModel.DeleteWord();
              break;
            case UICommandType.DELETE_INNER_WORD:
              _gameModel.GridModel.DeleteInnerWord();
              break;
            case UICommandType.CHANGE_WORD:
              _gameModel.GridModel.DeleteWord();
              _isInsertMode = true;
              break;
            case UICommandType.CHANGE_INNER_WORD:
              _gameModel.GridModel.DeleteInnerWord();
              _isInsertMode = true;
              break;
            case UICommandType.ENTER_INSERT_MODE:
              _isInsertMode = true;
              break;

            case UICommandType.EXIT_PUZZLE:
              var confirm = MessageBox.Query(30, 5, "System", "Ending Puzzle", "CONFIRM", "ABORT");
              if ( confirm == 0 ) {
                SavePuzzle();
                Application.RequestStop();
              }
              return;

            //////////////////////////////////////////
            //Insert
            //////////////////////////////////////////

            case UICommandType.INSERT_CHAR:
              InsertCharArgs insertCharArgs = (InsertCharArgs) command.Args!;
              _gameModel.GridModel.InsertChar(insertCharArgs.C);
              break;
            case UICommandType.DELETE_CHAR_INS:
              _gameModel.GridModel.DeleteChar(true);
              break;
            case UICommandType.ENTER_NORMAL_MODE:
              _isInsertMode = false;
              break;

            default:
              break;

          }

          var activeCluesEnd = _gameModel.GridModel.GetActiveClues();

          if (activeCluesStart != activeCluesEnd ) { 
            _eventBus.PostEvent(new FocusClueChangeEventArgs());
          }

          if ( activeOrientationStart != _gameModel.GridModel.Orientation ) {
            _eventBus.PostEvent(new OrientationChangeEventArgs());
          }

          if ( _gameModel.GridModel.IsComplete() ) {
            PuzzleFinished();
          }

          SetNeedsDisplay();

        }

    }

}
