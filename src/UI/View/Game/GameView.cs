namespace UI.View.Game
{
    using Entity;
    using Event;
    using Terminal.Gui;
    using UI.KeyMaping;
    using UI.KeyMapping;
    using UI.Model;
    using UI.View.Game.Clues;
    using Microsoft.EntityFrameworkCore;

    public class GameView : Toplevel
    {

        private EventBus _eventBus;
        private KeySequenceInterpreter _insertKeySequenceInterpreter;
        private KeySequenceInterpreter _normalKeySequenceInterpreter;
        private KeyMaps _keyMaps;
        private DatabaseContext _dbContext;

        private bool _isInsertMode = false;
        private GameModel _gameModel;

        private CluesView _cluesView;
        private GridView _gridView;
        private StatusView _statusView;
        private KeyBindsView _keyBindsView;

        public GameView(CluesView cluesView, GridView gridView, StatusView statusView,EventBus eventBus, KeyBindsView keyBindsView, KeyMaps keyMaps, DatabaseContext dbContext)
        {

            _gridView = gridView;
            _cluesView = cluesView;
            _statusView = statusView;
            _keyBindsView = keyBindsView;
            _eventBus = eventBus;
            _keyMaps = keyMaps;

            _gridView.X = 0;
            _gridView.Visible = true;

            _statusView.X = 0;
            _statusView.Y = Pos.Bottom(_gridView) + 2;

            // _keyBindsView.Visible = false;
            // _keyBindsView.Width = Dim.Fill();
            // _keyBindsView.Height = Dim.Fill();
     
            _cluesView.X = Pos.Right(_gridView);
            _cluesView.Width = Dim.Fill();
            _cluesView.Visible = true;

            Add(_gridView);
            Add(_statusView);
            Add(_cluesView);

            //Add(_keyBindsView);

            _normalKeySequenceInterpreter = new KeySequenceInterpreter(_keyMaps.NormalKeyMaps);
            _insertKeySequenceInterpreter = new KeySequenceInterpreter(_keyMaps.InsertKeyMaps);

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

          Crossword crossword = _dbContext.Crosswords
            .Include( x => x.GridChars )
            .Include( x => x.Clues )
            .First( x => x.Id == args.CrosswordId );
          crossword.StartDate ??= DateTime.UtcNow;

          //build game model
          int crosswordId = args.CrosswordId;
          GridModel gridModel = new GridModel(
                crossword.GridChars,
                crossword.Clues,
                crossword.Rows,
                crossword.Columns
          );

          _gameModel = new GameModel(crosswordId,gridModel,crossword.Elapsed,
              crossword.CharacterCheckCount,
              crossword.WordCheckCount,
              crossword.PuzzleCheckCount,
              crossword.Title
              );
          _eventBus.PostEvent(new PuzzleLoadedEventArgs(_gameModel));
        }

        public void SavePuzzle(bool complete) {

          _gameModel.GridModel.GridCharModels.ForEach( gcm => {
            GridChar gc = _dbContext.GridChars.First( 
                gc => gc.CrosswordId == _gameModel.CrosswordId &&
                gc.X == gcm.X &&
                gc.Y == gcm.Y);
            gc.UserChar = gcm.UserChar;
            gc.KnownChars = gcm.KnownChars;
          });

          Crossword crossword = _dbContext.Crosswords.First( c => c.Id == _gameModel.CrosswordId );
          crossword.Elapsed += DateTime.UtcNow - _gameModel.SessionStartTime;

          crossword.CharacterCheckCount = _gameModel.CharacterCheckCount;
          crossword.WordCheckCount = _gameModel.WordCheckCount;
          crossword.PuzzleCheckCount = _gameModel.PuzzleCheckCount;

          if ( complete ) {
            crossword.FinishDate = DateTime.UtcNow;
          }

          _dbContext.SaveChanges();
        }

        public void PuzzleFinished() {
          SavePuzzle(true);
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
            ///
            case UICommandType.SHOW_KEYBINDS:
              //_eventBus.PostEvent(new ClueViewChangeEventArgs());
              Application.Run(_keyBindsView);
              Trace.WriteLine("showing keybidns");
              break;
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
              _gameModel.GridModel.DeleteChar(false);
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

            case UICommandType.CHECK_CHAR:
              Trace.WriteLine("check char");
              _gameModel.GridModel.CheckChar();
              _gameModel.CharacterCheckCount++;
              _eventBus.PostEvent(new PuzzleLoadedEventArgs(_gameModel));
              break;
            case UICommandType.CHECK_WORD:
              Trace.WriteLine("check word");
              _gameModel.GridModel.CheckWord();
              _gameModel.WordCheckCount++;
              _eventBus.PostEvent(new PuzzleLoadedEventArgs(_gameModel));
              break;
            case UICommandType.CHECK_PUZZLE:
              Trace.WriteLine("check puzzle");
              _gameModel.GridModel.CheckPuzzle();
              _gameModel.PuzzleCheckCount++;
              _eventBus.PostEvent(new PuzzleLoadedEventArgs(_gameModel));
              break;

            case UICommandType.ENTER_INSERT_MODE:
              _isInsertMode = true;
              break;


            case UICommandType.EXIT_PUZZLE:
              var confirm = MessageBox.Query(30, 5, "System", "Ending Puzzle", "CONFIRM", "ABORT");
              if ( confirm == 0 ) {
                SavePuzzle(false);
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
