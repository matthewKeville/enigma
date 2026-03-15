namespace UI.View.Game
{

    using System.Data;
    using Enums;
    using Event;
    using Terminal.Gui;
    using Settings.Theme;
    using System.Drawing;
    using UI.Model;

    public class CluesSingleView : Toplevel
    {
        private DatabaseContext _dbContext;
        private EventBus _eventBus;
        private Theme _theme;
        private TableView _acrossTableView;
        private TableView _downTableView;
        private Label _tableLabel;

        private TableView _activeTableView;
        private TableView _inactiveTableView;

        private GameModel? _gameModel;

        public CluesSingleView(DatabaseContext dbContext, EventBus eventBus,Theme theme)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _eventBus.Register(this, (args) =>
            {

                if (args is PuzzleLoadedEventArgs)
                {
                    OnPuzzleLoaded((PuzzleLoadedEventArgs)args);
                }
                if (args is FocusClueChangeEventArgs)
                {
                    OnFocusClueChange();
                }
                if (args is OrientationChangeEventArgs)
                {
                    OnOrientationChange();
                }

            });
            _theme = theme;

            //Setup Table Views (inlined fix)
            this.ColorScheme = new ColorScheme(new Attribute(_theme.CluesBackgroundBG));

            TableStyle tableStyle = new TableStyle();
            tableStyle.ShowHeaders = false;
            tableStyle.ShowHorizontalHeaderOverline = false;
            tableStyle.ShowHorizontalHeaderUnderline = false;
            tableStyle.ShowVerticalCellLines = false;
            tableStyle.ExpandLastColumn = true; //ignore Max/Min Columns for last

            _acrossTableView = new TableView() {
              X = 1,
              Y = 3,
              Width = Dim.Fill(),
              //Not sure what this warn was, but idc
              Height = Dim.Fill()! - 1
            };
            _downTableView = new TableView() {
              X = _acrossTableView.X,
              Y =  _acrossTableView.Y,
              Width = _acrossTableView.Width,
              Height = _acrossTableView.Height
            };

            RowColorGetterDelegate rowColorGetter = (RowColorGetterArgs) =>
            {

                Direction direction =
                  RowColorGetterArgs.Table == _acrossTableView.Table ?
                    Direction.Across :
                    Direction.Down;

                TableView tableView = RowColorGetterArgs.Table == _acrossTableView.Table ?
                  _acrossTableView :
                  _downTableView;

                if (RowColorGetterArgs.RowIndex == tableView.SelectedRow)
                {
                    //i guess this is fine? doesn't really matter
                    if (_gameModel?.GridModel.Orientation == direction)
                    {
                        return new ColorScheme(new Attribute(_theme.FocusedClueFG, _theme.FocusedClueBG));
                    }
                    else
                    {
                        return new ColorScheme(new Attribute(_theme.CrossClueFG, _theme.CrossClueBG));
                    }
                }
                else
                {
                    return new ColorScheme(new Attribute(_theme.InactiveClueFG, _theme.InactiveClueBG));
                }
            };

            _acrossTableView.FullRowSelect = true;
            _acrossTableView.MinCellWidth = 2;
            _acrossTableView.MaxCellWidth = 2;
            _acrossTableView.Style = tableStyle;
            _acrossTableView.Style.RowColorGetter = rowColorGetter;
            _acrossTableView.ColorScheme = new ColorScheme(new Attribute(_theme.InactiveClueFG, _theme.InactiveClueBG));

            _downTableView.FullRowSelect = _acrossTableView.FullRowSelect;
            _downTableView.MinCellWidth = _acrossTableView.MinCellWidth;
            _downTableView.MaxCellWidth = _acrossTableView.MaxCellWidth;
            _downTableView.Style = _acrossTableView.Style;
            _downTableView.ColorScheme = new ColorScheme(new Attribute(_theme.InactiveClueFG, _theme.InactiveClueBG));

            _tableLabel = new Label();
            _tableLabel.X = 1;
            _tableLabel.Y = 1;
            _tableLabel.Width = Dim.Fill();
            _tableLabel.ColorScheme = new ColorScheme(new Attribute(_theme.CluesHeaderFG,_theme.CluesHeaderBG));
            _tableLabel.Text = "Across";

            _activeTableView = _acrossTableView;
            _inactiveTableView = _downTableView;

            Add(_tableLabel);
            Add(_acrossTableView);
            Add(_downTableView);
        }


        private void UpdateTableContent() {
          UpdateTableContent(Viewport.Size);
        }

        // @size : the size of the table we render the content for
        // in this case of OnViewportChanged, this would run before the
        // viewport change takes effect, so we need a way to inject
        // the future viewport size into the method. We are using the new
        // viewport size of this component to estimate the realized size of
        // the table views.
        private void UpdateTableContent(Size size)
        {

            if (_gameModel == null) {
              return;
            }

            //IEnumerable<Word> words = _dbContext.Words.Where(w => w.CrosswordId == _crosswordId);
            List<GridClueModel> clues = _gameModel.GridModel.GridClueModels;

            String clueTrailString = "...";
            int clueCutoff = size.Width
              - (_acrossTableView.MinCellWidth + 3);/* guess on future TableView 2nd column size */
            clueCutoff = Math.Max(0,clueCutoff);


            var adt = new DataTable();
            adt.Columns.Add("ordinal");
            adt.Columns.Add("clue");

            List<GridClueModel> across = clues.Where(w => w.Direction == Direction.Across).ToList();
            across.ForEach( across =>
            {
                String clueText = clueCutoff < across.Prompt.Count() 
                  ? across.Prompt.Substring(0,Math.Max(0,clueCutoff-clueTrailString.Count())) + clueTrailString
                  : across.Prompt;
                  
                adt.Rows.Add(new object[] { across.I, clueText });
            });


            var ddt = new DataTable();
            ddt.Columns.Add("ordinal");
            ddt.Columns.Add("clue");

            List<GridClueModel> down = clues.Where(w => w.Direction == Direction.Down).ToList();
            down.ForEach(down =>
            {
                String clueText = clueCutoff < down.Prompt.Count() 
                  ? down.Prompt.Substring(0,Math.Max(0,clueCutoff-clueTrailString.Count())) + clueTrailString
                  : down.Prompt;
                  
                ddt.Rows.Add(new object[] { down.I, clueText  });
            });

            _acrossTableView.Table = new DataTableSource(adt);
            _downTableView.Table = new DataTableSource(ddt);

        }

        public void UpdateSelectedRows()
        {

          //nullable bs
          if (_gameModel == null) {
            return;
          }

            if (_acrossTableView.Table is null || _downTableView.Table is null)
            {
                return;
            }

            _tableLabel.Text = _gameModel.GridModel.Orientation == Direction.Across ? "Across" : "Down";

            int ordinalRow = 0;
            bool found = false;
            //if (_activeClues.HasValue)
            (GridClueModel? across, GridClueModel? down) active = _gameModel.GridModel.GetActiveClues();
            if ( active.across != null && active.down != null)
            {

                int activeOrdinal = _gameModel.GridModel.Orientation == Direction.Across ?
                  active.across.I :
                  active.down.I;

                while (!found && ordinalRow < _activeTableView.Table.Rows)
                {
                    if ((Int32.Parse((string)_activeTableView.Table[ordinalRow, 0])) == activeOrdinal)
                    {
                        found = true;
                    }
                    else
                    {
                        ordinalRow++;
                    }
                }

                _activeTableView.SelectedRow = ordinalRow;
                _activeTableView.EnsureSelectedCellIsVisible();

            }

        }

        protected override void OnViewportChanged(DrawEventArgs e) {
          //this runs before the viewport change has re laid the sub views,
          //that is UpdateTableContent call would have the old Table Viewport
          //size, if we didn't pass it from the event here. Feels like
          //a hack, but I couldn't find a better way to achieve this
          if ( _gameModel != null ) {
            UpdateTableContent(e.NewViewport.Size);
          }
        }


        private void OnPuzzleLoaded(PuzzleLoadedEventArgs args) {
            _gameModel = args.GameModel;
            _activeTableView = _acrossTableView;
            _inactiveTableView = _downTableView;
            _acrossTableView.Visible = true;
            _downTableView.Visible = false;
            UpdateTableContent();
        }

        private void OnFocusClueChange() {
            UpdateSelectedRows();
        }

        private void OnOrientationChange()
        {
            if (_gameModel == null) {
              return;
            }

            _activeTableView = _gameModel.GridModel.Orientation == Direction.Across ? 
              _acrossTableView : _downTableView;
            _inactiveTableView = _gameModel.GridModel.Orientation == Direction.Across ? 
              _downTableView : _acrossTableView;

            _activeTableView.Visible = true;
            _inactiveTableView.Visible = false;
            UpdateSelectedRows();
        }

    }

}
