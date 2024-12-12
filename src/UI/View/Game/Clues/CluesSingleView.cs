namespace UI.View.Game
{

    using System.Data;
    using Entity;
    using Enums;
    using Event;
    using Terminal.Gui;
    using Settings.Theme;
    using System.Drawing;

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

        private int? _crosswordId;
        public (int AcrossOrdinal, int DownOrdinal)? _activeClues;
        private Direction _activeOrientation = Direction.Across;

        public CluesSingleView(DatabaseContext dbContext, EventBus eventBus,Theme theme)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _eventBus.Register(this, (args) =>
            {

                if (args is StartPuzzleEventArgs)
                {
                    OnStartPuzzleEvent((StartPuzzleEventArgs)args);
                }
                if (args is FocusClueChangeEventArgs)
                {
                    OnFocusClueChangeEvent((FocusClueChangeEventArgs)args);
                }
                if (args is OrientationChangeEventArgs)
                {
                    OnOrientationChangeEvent((OrientationChangeEventArgs)args);
                }

            });
            _theme = theme;

            SetupTableView();
        }


        private void SetupTableView()
        {
            this.ColorScheme = new ColorScheme(new Attribute(_theme.CluesBackgroundBG));

            TableStyle tableStyle = new TableStyle();
            tableStyle.ShowHeaders = false;
            tableStyle.ShowHorizontalHeaderOverline = false;
            tableStyle.ShowHorizontalHeaderUnderline = false;
            tableStyle.ShowVerticalCellLines = false;
            tableStyle.ExpandLastColumn = true; //ignore Max/Min Columns for last

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
                    if (_activeOrientation == direction)
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

            _acrossTableView = new TableView() {
              X = 0,
              Y = 2,
              Width = Dim.Fill(),
              Height = Dim.Fill()
            };
            _acrossTableView.FullRowSelect = true;
            _acrossTableView.MinCellWidth = 2;
            _acrossTableView.MaxCellWidth = 2;
            _acrossTableView.Style = tableStyle;
            _acrossTableView.Style.RowColorGetter = rowColorGetter;
            _acrossTableView.ColorScheme = new ColorScheme(new Attribute(_theme.InactiveClueFG, _theme.InactiveClueBG));

            _downTableView = new TableView() {
              X = 0,
              Y = 2,
              Width = Dim.Fill(),
              Height = Dim.Fill()
            };
            _downTableView.FullRowSelect = true;
            _downTableView.MinCellWidth = 2;
            _downTableView.MaxCellWidth = 2;
            _downTableView.Style = tableStyle;
            _downTableView.Style.RowColorGetter = rowColorGetter;
            _downTableView.ColorScheme = new ColorScheme(new Attribute(_theme.InactiveClueFG, _theme.InactiveClueBG));

            _tableLabel = new Label();
            _tableLabel.X = 0;
            _tableLabel.Y = 0;
            _tableLabel.Width = Dim.Fill();
            _tableLabel.ColorScheme = new ColorScheme(new Attribute(_theme.CluesHeaderFG,_theme.CluesHeaderBG));

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

            IEnumerable<Word> words = _dbContext.Words.Where(w => w.CrosswordId == _crosswordId);

            String clueTrailString = "...";
            int clueCutoff = size.Width
              - (_acrossTableView.MinCellWidth + 3);/* guess on future TableView 2nd column size */
            clueCutoff = Math.Max(0,clueCutoff);


            var adt = new DataTable();
            adt.Columns.Add("ordinal");
            adt.Columns.Add("clue");

            List<Word> across = words.Where(w => w.Direction == Direction.Across).ToList();
            across.ForEach(across =>
            {
                String clueText = clueCutoff < across.Clue.Count() 
                  ? across.Clue.Substring(0,Math.Max(0,clueCutoff-clueTrailString.Count())) + clueTrailString
                  : across.Clue;
                  
                adt.Rows.Add(new object[] { across.I, clueText });
            });


            var ddt = new DataTable();
            ddt.Columns.Add("ordinal");
            ddt.Columns.Add("clue");

            List<Word> down = words.Where(w => w.Direction == Direction.Down).ToList();
            down.ForEach(down =>
            {
                String clueText = clueCutoff < down.Clue.Count() 
                  ? down.Clue.Substring(0,Math.Max(0,clueCutoff-clueTrailString.Count())) + clueTrailString
                  : down.Clue;
                  
                ddt.Rows.Add(new object[] { down.I, clueText  });
            });

            _acrossTableView.Table = new DataTableSource(adt);
            _downTableView.Table = new DataTableSource(ddt);

        }

        public void UpdateSelectedRows()
        {

            if (_acrossTableView.Table is null || _downTableView.Table is null)
            {
                return;
            }

            _tableLabel.Text = _activeOrientation == Direction.Across ? "Across" : "Down";

            int ordinalRow = 0;
            bool found = false;
            if (_activeClues.HasValue)
            {

                int activeOrdinal = _activeOrientation == Direction.Across ?
                  _activeClues.Value.AcrossOrdinal :
                  _activeClues.Value.DownOrdinal;

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
          if ( _crosswordId.HasValue ) {
            UpdateTableContent(e.NewViewport.Size);
          }
        }


        private void OnStartPuzzleEvent(StartPuzzleEventArgs args)
        {
            _crosswordId = args.CrosswordId;
            _activeOrientation = Direction.Across;
            _activeTableView = _acrossTableView;
            _inactiveTableView = _downTableView;
            _acrossTableView.Visible = true;
            _downTableView.Visible = false;
            UpdateTableContent();
        }

        private void OnFocusClueChangeEvent(FocusClueChangeEventArgs args)
        {
            _activeClues = args.ActiveClues;
            UpdateSelectedRows();
        }

        private void OnOrientationChangeEvent(OrientationChangeEventArgs args)
        {
            _activeOrientation = args.Orientation;

            _activeTableView = _activeOrientation == Direction.Across ? 
              _acrossTableView : _downTableView;
            _inactiveTableView = _activeOrientation == Direction.Across ? 
              _downTableView : _acrossTableView;

            _activeTableView.Visible = true;
            _inactiveTableView.Visible = false;
            UpdateSelectedRows();
        }

    }

}
