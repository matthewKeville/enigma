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
                if (args is UICommand) {
                  ProcessUICommand((UICommand) args);
                }
            });

            _theme = theme;

            KeyBindings.Clear();

            setupView();

        }

        public void ProcessUICommand(UICommand command)
        {

          var activeCluesStart = _gridModel.GetActiveClues();
          var orientationStart = _gridModel.Orientation;

          switch ( command.Type ) {

            //////////////////////////////////////////
            //Normal
            //////////////////////////////////////////

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
            case UICommandType.CHANGE_WORD:
              _gridModel.DeleteWord();
              break;
            case UICommandType.CHANGE_INNER_WORD:
              _gridModel.DeleteInnerWord();
              break;

            //////////////////////////////////////////
            //Insert
            //////////////////////////////////////////

            case UICommandType.INSERT_CHAR:
              InsertCharArgs insertCharArgs = (InsertCharArgs) command.Args!;
              _gridModel.InsertChar(insertCharArgs.C);
              break;

            case UICommandType.DELETE_CHAR_INS:
              _gridModel.DeleteChar(true);
              break;

            default:
              break;

          }

          var activeCluesEnd = _gridModel.GetActiveClues();

          if (activeCluesStart != activeCluesEnd ) { 
            _eventBus.PostEvent(new FocusClueChangeEventArgs((activeCluesEnd.Item1.I,activeCluesEnd.Item2.I)));
          }

          SetNeedsDisplay();

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

        private void setupView() {
          this.ColorScheme = new ColorScheme(new Attribute(_theme.GridBackgroundBG));
        }

    }

}
