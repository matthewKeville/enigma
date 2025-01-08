namespace UI.View.Game
{

    using System.Data;
    using Entity;
    using Enums;
    using Event;
    using Terminal.Gui;
    using Settings.Theme;
    using System.Drawing;

    public class CluesSplitView : Toplevel
    {
        private DatabaseContext _dbContext;
        private EventBus _eventBus;
        private Theme _theme;

        private TableView _acrossTableView;
        private TableView _downTableView;
        private Label _acrossLabel;
        private Label _downLabel;

        private int? _crosswordId;
        public (int AcrossOrdinal, int DownOrdinal)? _activeClues;
        public Direction _activeOrientation = Direction.Across;

        public CluesSplitView(DatabaseContext dbContext, EventBus eventBus, Theme theme)
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

            SetupViews();

        }

        private void SetupViews()
        {
            this.ColorScheme = new ColorScheme(new Attribute(_theme.CluesBackgroundBG));

            TableStyle tableStyle = new TableStyle();
            tableStyle.ShowHeaders = false;
            tableStyle.ShowHorizontalHeaderOverline = false;
            tableStyle.ShowHorizontalHeaderUnderline = false;
            tableStyle.ShowVerticalCellLines = false;
            tableStyle.ExpandLastColumn = true;

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

            _acrossLabel = new Label();
            _acrossLabel.Text = "Across";
            _acrossLabel.X = 1;
            _acrossLabel.Y = 1;
            _acrossLabel.Width = Dim.Fill();
            _acrossLabel.ColorScheme = new ColorScheme(new Attribute(_theme.CluesHeaderFG,_theme.CluesHeaderBG));

            _downLabel = new Label();
            _downLabel.Text = "Down";
            _downLabel.X = 1;
            _downLabel.Y = Pos.Percent(50) + 1;
            _downLabel.Width = Dim.Fill();
            _downLabel.ColorScheme = new ColorScheme(new Attribute(_theme.CluesHeaderFG,_theme.CluesHeaderBG));

            _downTableView = new TableView() { };
            _downTableView.X = 1;
            _downTableView.Y = Pos.Bottom(_downLabel) + 1;
            _downTableView.Width = Dim.Fill();
            _downTableView.Height = Dim.Fill() - 1;

            _acrossTableView = new TableView() { };
            _acrossTableView.X = 1;
            _acrossTableView.Y = Pos.Bottom(_acrossLabel) + 1;
            _acrossTableView.Width = Dim.Fill();
            _acrossTableView.Height = Dim.Height(_downTableView);

            _acrossTableView.FullRowSelect = true;
            _acrossTableView.MinCellWidth = 2;
            _acrossTableView.MaxCellWidth = 2;
            _acrossTableView.Style = tableStyle;
            _acrossTableView.Style.RowColorGetter = rowColorGetter;
            _acrossTableView.ColorScheme = new ColorScheme(new Attribute(_theme.CluesTableBackgroundBG));

            _downTableView.FullRowSelect = true;
            _downTableView.MinCellWidth = 2;
            _downTableView.MaxCellWidth = 2;
            _downTableView.Style = tableStyle;
            _downTableView.Style.RowColorGetter = rowColorGetter;
            _downTableView.ColorScheme = new ColorScheme(new Attribute(_theme.CluesTableBackgroundBG));

            Add(_acrossLabel);
            Add(_acrossTableView);
            Add(_downLabel);
            Add(_downTableView);

        }

        public void UpdateTableContent() {
          UpdateTableContent(Viewport.Size);
        }

        //@size : Viewport Size for rendering
        //see CluesSingleView.UpdateTableContent for ref.
        private void UpdateTableContent(Size size)
        {

            IEnumerable<Word> words = _dbContext.Words.Where(w => w.CrosswordId == _crosswordId);

            String clueTrailString = "...";
            int clueCutoff = size.Width
              - (_acrossTableView.MinCellWidth + 3); /* guess on future TableView 2nd column size */
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
            _acrossTableView.Table = new DataTableSource(adt);

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
            _downTableView.Table = new DataTableSource(ddt);

        }

        public void UpdateSelectedRows()
        {

            if (_acrossTableView.Table is null || _downTableView.Table is null)
            {
                return;
            }

            if (_activeClues.HasValue)
            {

                // Across

                int ordinalRowAcross = 0;
                bool foundAcross = false;
                while (!foundAcross && ordinalRowAcross < _acrossTableView.Table.Rows)
                {
                    if ((Int32.Parse((string)_acrossTableView.Table[ordinalRowAcross, 0])) == _activeClues.Value.AcrossOrdinal)
                    {
                        foundAcross = true;
                    }
                    else
                    {
                        ordinalRowAcross++;
                    }
                }
                _acrossTableView.SelectedRow = ordinalRowAcross;
                _acrossTableView.EnsureSelectedCellIsVisible();

                // Down

                int ordinalRowDown = 0;
                bool foundDown = false;
                while (!foundDown && ordinalRowDown < _downTableView.Table.Rows)
                {
                    if ((Int32.Parse((string)_downTableView.Table[ordinalRowDown, 0])) == _activeClues.Value.DownOrdinal)
                    {
                        foundDown = true;
                    }
                    else
                    {
                        ordinalRowDown++;
                    }
                }
                _downTableView.SelectedRow = ordinalRowDown;
                _downTableView.EnsureSelectedCellIsVisible();

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
            UpdateTableContent();
        }

        private void OnFocusClueChangeEvent(FocusClueChangeEventArgs args)
        {
            _activeClues = (args.ActiveClues.AcrossOrdinal, args.ActiveClues.DownOrdinal);
            UpdateSelectedRows();
        }

        private void OnOrientationChangeEvent(OrientationChangeEventArgs args)
        {
            _activeOrientation = (args.Orientation);
            UpdateSelectedRows();
        }

    }

}
