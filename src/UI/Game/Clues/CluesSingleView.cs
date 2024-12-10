namespace UI.Game {

using System.Data;
using System.Drawing;
using Entity;
using Enums;
using Event;
using Terminal.Gui;

public class CluesSingleView : Window
{
  private DatabaseContext _dbContext;
  private EventBus _eventBus;
  public (int AcrossOrdinal,int DownOrdinal)? _activeClues;
  private Direction _activeOrientation = Direction.Across;
  private TableView _acrossTableView;
  private TableView _downTableView;
  private Label _tableLabel;

  public CluesSingleView(DatabaseContext dbContext,EventBus eventBus) 
  {
      _dbContext = dbContext;
      _eventBus = eventBus;
      _eventBus.Register(this,(args) => { 

        if ( args is StartPuzzleEventArgs ) {
          OnStartPuzzleEvent((StartPuzzleEventArgs) args);
        }
        if ( args is FocusClueChangeEventArgs ) {
          OnFocusClueChangeEvent((FocusClueChangeEventArgs) args);
        }
        if ( args is OrientationChangeEventArgs ) {
          OnOrientationChangeEvent((OrientationChangeEventArgs) args);
        }

      });

      SetupTableView();
  }

  public override void OnDrawContent(Rectangle contentArea) {

    if ( _acrossTableView.Table is null || _downTableView.Table is null ) {
      base.OnDrawContent(contentArea);
      return;
    }

    TableView activeTableView = _activeOrientation == Direction.Across ? _acrossTableView : _downTableView;
    TableView inactiveTableView = _activeOrientation == Direction.Across ?  _downTableView : _acrossTableView;

    Remove(inactiveTableView);
    Add(_tableLabel);
    Add(activeTableView);

    _tableLabel.Text = _activeOrientation == Direction.Across ? "Across" : "Down";
    _tableLabel.X = 0;
    _tableLabel.Y = 0;
    _tableLabel.Width = Dim.Fill();

    activeTableView.X = 0;
    activeTableView.Y = 2;
    activeTableView.Width = Dim.Fill();
    activeTableView.Height = Dim.Percent(98);

    int ordinalRow = 0;
    bool found = false;
    if ( _activeClues.HasValue ) {

      int activeOrdinal = _activeOrientation == Direction.Across ?
        _activeClues.Value.AcrossOrdinal:
        _activeClues.Value.DownOrdinal;

      while ( !found && ordinalRow < activeTableView.Table.Rows ) {
        if ( (Int32.Parse((string)activeTableView.Table[ordinalRow,0])) == activeOrdinal) {
          found = true;
        } else {
          ordinalRow++;
        }
      }
      activeTableView.SelectedRow = ordinalRow;
      activeTableView.EnsureSelectedCellIsVisible();

    } 

    base.OnDrawContent(contentArea);
  }

  private void SetupTableView() {

      TableStyle tableStyle = new TableStyle();
        tableStyle.ShowHeaders = false;
        tableStyle.ShowHorizontalHeaderOverline = false;
        tableStyle.ShowHorizontalHeaderUnderline = false;
        tableStyle.ShowVerticalCellLines = false;              

      RowColorGetterDelegate rowColorGetter = (RowColorGetterArgs) => {

        Direction direction = 
          RowColorGetterArgs.Table == _acrossTableView.Table ?
            Direction.Across :
            Direction.Down;

        TableView tableView = RowColorGetterArgs.Table == _acrossTableView.Table ?
          _acrossTableView :
          _downTableView;

        if (RowColorGetterArgs.RowIndex == tableView.SelectedRow ) {
          if ( _activeOrientation == direction ) {
            return new ColorScheme {
              Normal = new Attribute(ColorName.Yellow,ColorName.Blue),
              Focus = new Attribute(ColorName.Yellow,ColorName.Blue),
              HotNormal = new Attribute(ColorName.Yellow,ColorName.Blue),
              HotFocus = new Attribute(ColorName.Yellow,ColorName.Blue)
            };
          } else {
            return new ColorScheme {
              Normal = new Attribute(ColorName.Black,ColorName.Blue),
              Focus = new Attribute(ColorName.Black,ColorName.Blue),
              HotNormal = new Attribute(ColorName.Black,ColorName.Blue),
              HotFocus = new Attribute(ColorName.Black,ColorName.Blue)
            };
          }
        } else {
          return new ColorScheme {
            Normal = new Attribute(ColorName.Red,ColorName.Blue),
            Focus = new Attribute(ColorName.Red,ColorName.Blue),
            HotNormal = new Attribute(ColorName.Red,ColorName.Blue),
            HotFocus = new Attribute(ColorName.Red,ColorName.Blue)
          };
        }
      };

    _acrossTableView = new TableView() {};
    _acrossTableView.FullRowSelect = true;
    _acrossTableView.MinCellWidth = 8;
    _acrossTableView.Style = tableStyle;
    _acrossTableView.Style.RowColorGetter = rowColorGetter;

    _downTableView = new TableView() {};
    _downTableView.FullRowSelect = true;
    _downTableView.MinCellWidth = 8;
    _downTableView.Style = tableStyle;
    _downTableView.Style.RowColorGetter = rowColorGetter;

    _tableLabel = new Label();

  }

  private void Init(int crosswordId) {

    IEnumerable<Word> words = _dbContext.Words.Where( w => w.CrosswordId == crosswordId );

    var adt = new DataTable();
    adt.Columns.Add("ordinal");
    adt.Columns.Add("clue");
    List<Word> across = words.Where( w => w.Direction == Direction.Across ).ToList();
    across.ForEach( across => {
      adt.Rows.Add(new object [] {across.I, across.Clue});
    });
    _acrossTableView.Table = new DataTableSource(adt);

    var ddt = new DataTable();
    ddt.Columns.Add("ordinal");
    ddt.Columns.Add("clue");
    List<Word> down = words.Where( w => w.Direction == Direction.Down ).ToList();
    down.ForEach( down => {
      ddt.Rows.Add(new object[] {down.I, down.Clue});
    });
    _downTableView.Table = new DataTableSource(ddt);

    _activeOrientation = Direction.Across;

  }

  private void OnStartPuzzleEvent(StartPuzzleEventArgs args) {
    Init(args.CrosswordId);
  }

  private void OnFocusClueChangeEvent(FocusClueChangeEventArgs args) {
    _activeClues = args.ActiveClues;
  }

  private void OnOrientationChangeEvent(OrientationChangeEventArgs args) {
    _activeOrientation = args.Orientation;
  }

}

}
