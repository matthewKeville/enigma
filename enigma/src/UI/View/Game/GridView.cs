namespace UI.View.Game
{

    using System.Drawing;
    using System.Text;
    using Entity;
    using Enums;
    using Event;
    using Settings.Theme;
    using Terminal.Gui;
    using UI.Model;

    public class GridView : Toplevel
    {
        private DatabaseContext _dbContext;
        private EventBus _eventBus;
        private GameModel? _gameModel;
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
                if (args is PuzzleLoadedEventArgs)
                {
                  OnPuzzleLoaded((PuzzleLoadedEventArgs)args);
                }
            });

            _theme = theme;

            KeyBindings.Clear();

            setupView();

        }


        public override void OnDrawContent(Rectangle contentArea)
        {

            base.OnDrawContent(contentArea);
            Driver.FillRect(contentArea,' ');

            if ( _gameModel == null ) {
              return;
            }

            foreach (GridCharModel gcm in _gameModel.GridModel.GridCharModels)
            {

                //DBG DELETE
                if (gcm.X == 0 && gcm.Y == 0 ) {
                  Trace.WriteLine("DBG CORNER");
                  Trace.WriteLine("DBG CORNER");
                  Trace.WriteLine("DBG CORNER");
                  Trace.WriteLine("DBG CORNER");
                  Trace.WriteLine("Answer char is " + gcm.AnswerChar);
                }
                //DBG DELETE
       
                Rune rune;
                Attribute attr;

                //block
                if (gcm.IsBlock) {

                  rune = new Rune(_theme.BlockChar);
                  attr = new Terminal.Gui.Attribute(_theme.BlockFG,_theme.BlockBG);

                //emtpy
                } else if ( gcm.UserChar == ' ' ) {

                  if ( _gameModel.GridModel.Selection.Equals(gcm) ) {
                    rune = new Rune(_theme.CursorEmptyHighlightChar);
                    attr = new Terminal.Gui.Attribute(_theme.CursorEmptyHighlightFG,_theme.CursorEmptyHighlightBG);
                  }

                  else if ( _gameModel.GridModel.ActiveWordChars().Contains(gcm) ) {
                    rune = _gameModel.GridModel.Orientation == Direction.Across 
                      ? new Rune(_theme.ActiveEmptyHighlightAcrossChar)
                      : new Rune(_theme.ActiveEmptyHighlightDownChar);
                    attr = new Terminal.Gui.Attribute(_theme.ActiveEmptyHighlightFG,_theme.ActiveEmptyHighlightBG);
                  } 

                  else if ( _gameModel.GridModel.CrossWordChars().Contains(gcm) ) {
                    rune = _gameModel.GridModel.Orientation == Direction.Across 
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

                  rune = new Rune(gcm.UserChar ?? ' ');

                  if ( _gameModel.GridModel.Selection.Equals(gcm) ) {
                    switch ( gcm.Status() ) {
                      case GridCharStatus.INCORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.CursorInorrectHighlightFG,_theme.CursorInorrectHighlightBG);
                        break;
                      case GridCharStatus.CORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.CursorCorrectHighlightFG,_theme.CursorCorrectHighlightBG);
                        break;
                      default:
                        attr = new Terminal.Gui.Attribute(_theme.CursorHighlightFG,_theme.CursorHighlightBG);
                        break;
                    }
                  }

                  else if ( _gameModel.GridModel.ActiveWordChars().Contains(gcm) ) {
                    switch ( gcm.Status() ) {
                      case GridCharStatus.INCORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.ActiveInorrectHighlightFG,_theme.ActiveInorrectHighlightBG);
                        break;
                      case GridCharStatus.CORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.ActiveCorrectHighlightFG,_theme.ActiveCorrectHighlightBG);
                        break;
                      default:
                        attr = new Terminal.Gui.Attribute(_theme.ActiveHighlightFG,_theme.ActiveHighlightBG);
                        break;
                    }
                  } 

                  else if ( _gameModel.GridModel.CrossWordChars().Contains(gcm) ) {
                    switch ( gcm.Status() ) {
                      case GridCharStatus.INCORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.CrossIncorrectHighlightFG,_theme.CrossIncorrectHighlightBG);
                        break;
                      case GridCharStatus.CORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.CrossCorrectHighlightFG,_theme.CrossCorrectHighlightBG);
                        break;
                      default:
                        attr = new Terminal.Gui.Attribute(_theme.CrossHighlightFG,_theme.CrossHighlightBG);
                        break;
                    }
                  } 

                  else {
                    switch ( gcm.Status() ) {
                      case GridCharStatus.INCORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.CellIncorrectHighlightFG,_theme.CellIncorrectHighlightBG);
                        break;
                      case GridCharStatus.CORRECT: 
                        attr = new Terminal.Gui.Attribute(_theme.CellCorrectHighlightFG,_theme.CellCorrectHighlightBG);
                        break;
                      default:
                        attr = new Terminal.Gui.Attribute(_theme.CellFG,_theme.CellBG);
                        break;
                    }
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

        private void Init(GameModel gameModel)
        {
            //FIXME 
            // Width = _crossword.Columns + (gridOffX * 2);
            // Height = _crossword.Rows + (gridOffY * 2);
            Width =  _gameModel.GridModel.ColumnCount + (gridOffX * 2);
            Height = _gameModel.GridModel.RowCount + (gridOffY * 2);
            SetNeedsDisplay();
        }

        private void OnPuzzleLoaded(PuzzleLoadedEventArgs args)
        {
            _gameModel = args.GameModel;
            Init(args.GameModel);
        }

        private void setupView() {
          this.ColorScheme = new ColorScheme(new Attribute(_theme.GridBackgroundBG));
        }

    }

}
