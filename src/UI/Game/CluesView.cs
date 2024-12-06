namespace UI.Game {

using System.Data;
    using System.Drawing;
    using Entity;
using Enums;
using Event;
using Terminal.Gui;

public class CluesView : Window
{
  private DatabaseContext _dbContext;
  private CluesModel _cluesModel;
  private TableView _acrossTableView;
  private TableView _downTableView;
  private EventBus _eventBus;

  public CluesView(DatabaseContext dbContext,EventBus eventBus) 
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


      _acrossTableView = new TableView() {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Percent(40)
      };
      _acrossTableView.FullRowSelect = true;
      _acrossTableView.MinCellWidth = 8;
      _acrossTableView.Style.ShowHeaders = false;
      _acrossTableView.Style.ShowHorizontalHeaderOverline = false;
      _acrossTableView.Style.ShowHorizontalHeaderUnderline = false;
      _acrossTableView.Style.ShowVerticalCellLines = false;
      RowColorGetterDelegate acrossRCG = (RowColorGetterArgs) => {
        if (RowColorGetterArgs.RowIndex == _acrossTableView.SelectedRow ) {
          if ( _cluesModel.ActiveOrientation == Direction.Across ) {
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
      _acrossTableView.Style.RowColorGetter = acrossRCG;

      _downTableView = new TableView() {
        X = 0,
        Y= Pos.Percent(50),
        Width = Dim.Fill(),
        Height = Dim.Percent(40)
      };
      _downTableView.FullRowSelect = true;
      _downTableView.MinCellWidth = 8;
      _downTableView.Style.ShowHeaders = false;
      _downTableView.Style.ShowHorizontalHeaderOverline = false;
      _downTableView.Style.ShowHorizontalHeaderUnderline = false;
      _downTableView.Style.ShowVerticalCellLines = false;
      RowColorGetterDelegate downRCG = (RowColorGetterArgs) => {
        if (RowColorGetterArgs.RowIndex == _downTableView.SelectedRow ) {
          if ( _cluesModel.ActiveOrientation == Direction.Down ) {
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
      _downTableView.Style.RowColorGetter = downRCG;

      Add(_acrossTableView);
      Add(_downTableView);
  }

  public override void OnDrawContent(Rectangle contentArea) {
    _acrossTableView.SelectedRow = _cluesModel.ActiveClues.AcrossOrdinal;
    _downTableView.SelectedRow = _cluesModel.ActiveClues.DownOrdinal;
    _acrossTableView.EnsureSelectedCellIsVisible();
    _downTableView.EnsureSelectedCellIsVisible();
    Trace.WriteLine("redrew clues view");
    base.OnDrawContent(contentArea);
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

    _cluesModel = new CluesModel();

  }

  private void OnStartPuzzleEvent(StartPuzzleEventArgs args) {
    Init(args.CrosswordId);
  }

  private void OnFocusClueChangeEvent(FocusClueChangeEventArgs args) {
    Trace.WriteLine("got clue change");
    _cluesModel.UpdateActiveClues(args.ActiveClues.AcrossOrdinal,args.ActiveClues.DownOrdinal);
    //SetNeedsDisplay();
  }

  private void OnOrientationChangeEvent(OrientationChangeEventArgs args) {
    Trace.WriteLine("got orientation change");
    _cluesModel.UpdateOrientation(args.Orientation);
    //SetNeedsDisplay();
  }
}

}
