namespace UI.Game {


using System.Data;
using Entity;
using Enums;
using Event;
using Terminal.Gui;

public class CluesView : Window
{
  private DatabaseContext _dbContext;
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
      //seems to be broken?
      //_acrossTableView.Style.ShowHorizontalBottomline = true;

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
      //seems to be broken?
      //_downTableView.Style.ShowHorizontalBottomline = true;

      Add(_acrossTableView);
      Add(_downTableView);
  }

  private void Init(int crosswordId) {

    IEnumerable<Word> words = _dbContext.Words.Where( w => w.CrosswordId == crosswordId );

    var adt = new DataTable();
    adt.Columns.Add("across");
    adt.Columns.Add("."); //can't leave this blank otherwise displays ColumnN
    List<Word> across = words.Where( w => w.Direction == Direction.Across ).ToList();
    across.ForEach( across => {
      adt.Rows.Add(new String [] {across.I.ToString(), across.Clue});
    });
    _acrossTableView.Table = new DataTableSource(adt);

    var ddt = new DataTable();
    ddt.Columns.Add("down");
    ddt.Columns.Add(".");
    List<Word> down = words.Where( w => w.Direction == Direction.Down ).ToList();
    down.ForEach( down => {
      ddt.Rows.Add(new String [] {down.I.ToString(), down.Clue});
    });
    _downTableView.Table = new DataTableSource(ddt);

  }

  private void OnStartPuzzleEvent(StartPuzzleEventArgs args) {
    Init(args.CrosswordId);
  }
}

}
