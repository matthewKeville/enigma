namespace UI.View.Game
{

    using System.Data;
    using System.Drawing;
    using Entity;
    using Enums;
    using Event;
    using Terminal.Gui;
    using UI.Theme;

    public class CluesSplitView : Window
    {
        private DatabaseContext _dbContext;
        private EventBus _eventBus;
        private Theme _theme;
        private TableView _acrossTableView;
        private TableView _downTableView;
        private Label _acrossLabel;
        private Label _downLabel;

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

        public override void OnDrawContent(Rectangle contentArea)
        {

            if (_acrossTableView.Table is null || _downTableView.Table is null)
            {
                base.OnDrawContent(contentArea);
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

            base.OnDrawContent(contentArea);
        }

        private void SetupViews()
        {
            this.ColorScheme = new ColorScheme(new Attribute(_theme.CluesBackgroundBG));

            TableStyle tableStyle = new TableStyle();
            tableStyle.ShowHeaders = false;
            tableStyle.ShowHorizontalHeaderOverline = false;
            tableStyle.ShowHorizontalHeaderUnderline = false;
            tableStyle.ShowVerticalCellLines = false;

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
            _acrossLabel.X = 0;
            _acrossLabel.Y = 0;
            _acrossLabel.Width = Dim.Fill();
            _acrossLabel.ColorScheme = new ColorScheme(new Attribute(_theme.CluesHeaderFG,_theme.CluesHeaderBG));

            Add(_acrossLabel);

            _acrossTableView = new TableView() { };
            _acrossTableView.X = 0;
            _acrossTableView.Y = Pos.Bottom(_acrossLabel) + 1;
            _acrossTableView.Width = Dim.Fill();
            _acrossTableView.Height = Dim.Percent(45);

            _acrossTableView.FullRowSelect = true;
            _acrossTableView.MinCellWidth = 8;
            _acrossTableView.Style = tableStyle;
            _acrossTableView.Style.RowColorGetter = rowColorGetter;
            //need to set this, for when there is more space alotted than rows
            _acrossTableView.ColorScheme = new ColorScheme(new Attribute(_theme.InactiveClueFG, _theme.InactiveClueBG));

            Add(_acrossTableView);

            _downLabel = new Label();
            _downLabel.Text = "Down";
            _downLabel.X = 0;
            _downLabel.Y = Pos.Percent(50);
            _downLabel.Width = Dim.Percent(45);
            _downLabel.ColorScheme = new ColorScheme(new Attribute(_theme.CluesHeaderFG,_theme.CluesHeaderBG));

            Add(_downLabel);

            _downTableView = new TableView() { };
            _downTableView.X = 0;
            _downTableView.Y = Pos.Bottom(_downLabel) + 1;
            _downTableView.Width = Dim.Fill();
            _downTableView.Height = Dim.Fill();

            _downTableView.FullRowSelect = true;
            _downTableView.MinCellWidth = 8;
            _downTableView.Style = tableStyle;
            _downTableView.Style.RowColorGetter = rowColorGetter;
            //need to set this, for when there is more space alotted than rows
            _downTableView.ColorScheme = new ColorScheme(new Attribute(_theme.InactiveClueFG, _theme.InactiveClueBG));
            Add(_downTableView);

        }

        private void Init(int crosswordId)
        {

            IEnumerable<Word> words = _dbContext.Words.Where(w => w.CrosswordId == crosswordId);

            var adt = new DataTable();
            adt.Columns.Add("ordinal");
            adt.Columns.Add("clue");
            List<Word> across = words.Where(w => w.Direction == Direction.Across).ToList();
            across.ForEach(across =>
            {
                adt.Rows.Add(new object[] { across.I, across.Clue });
            });
            _acrossTableView.Table = new DataTableSource(adt);

            var ddt = new DataTable();
            ddt.Columns.Add("ordinal");
            ddt.Columns.Add("clue");
            List<Word> down = words.Where(w => w.Direction == Direction.Down).ToList();
            down.ForEach(down =>
            {
                ddt.Rows.Add(new object[] { down.I, down.Clue });
            });
            _downTableView.Table = new DataTableSource(ddt);

        }

        private void OnStartPuzzleEvent(StartPuzzleEventArgs args)
        {
            Init(args.CrosswordId);
        }

        private void OnFocusClueChangeEvent(FocusClueChangeEventArgs args)
        {
            _activeClues = (args.ActiveClues.AcrossOrdinal, args.ActiveClues.DownOrdinal);
        }

        private void OnOrientationChangeEvent(OrientationChangeEventArgs args)
        {
            _activeOrientation = (args.Orientation);
        }

    }

}
