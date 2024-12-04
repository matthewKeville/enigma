using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.CrosswordInstaller;
using Services.CrosswordInstaller.NYT;
using System.Data;
using Event;
using Entity;
using Enums;
using static Event.StartPuzzleEventArgs;
using System.Text;
using Terminal.Gui;
using System.Drawing;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
builder.Services.AddSingleton<DatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<CrosswordService, CrosswordService>();
builder.Services.AddSingleton<NYTCrosswordInstaller, NYTCrosswordInstaller>();
builder.Services.AddSingleton<NYTCrosswordParser, NYTCrosswordParser>();
builder.Services.AddSingleton<CrosswordInstallerService, CrosswordInstallerService>();

builder.Services.AddSingleton<EventBus, EventBus>();
builder.Services.AddSingleton<RootView, RootView>();
builder.Services.AddSingleton<BrowserView, BrowserView>();
builder.Services.AddSingleton<PuzzleInstallerView, PuzzleInstallerView>();
builder.Services.AddSingleton<PuzzlePickerView, PuzzlePickerView>();
builder.Services.AddSingleton<GameView, GameView>();
builder.Services.AddSingleton<GridView, GridView>();
builder.Services.AddSingleton<CluesView, CluesView>();

IHost host = builder.Build();
host.Start();

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;
Application.Init();
Application.Run(host.Services.GetService<RootView>());
Application.Shutdown ();

public class RootView : Window
{
    private BrowserView _browserView;
    private GameView _gameView;
    private EventBus _eventBus;

    public RootView (BrowserView browserView,GameView gameView,EventBus eventBus)
    {
      _browserView = browserView;
      _gameView = gameView;
      _eventBus = eventBus;

      _eventBus.Register(this,(eventArgs) => {
        if ( eventArgs is StartPuzzleEventArgs ) {
          showGame();
        } else if ( eventArgs is EndPuzzleEventArgs ) {
          showBrowser();
        }
      });

      showBrowser();
      /**
      DrawView drawView = new DrawView();
      drawView.Width = 20;
      drawView.Height = 10;
      Add(drawView);
      */
    }

    private void showGame() {
      RemoveAll();
      Add( _gameView );
    }

    private void showBrowser() {
      RemoveAll();
      Add( _browserView );
    }
}

public class BrowserView : Window
{
    private bool _picker = true;
    private PuzzlePickerView _puzzlePickerView;
    private PuzzleInstallerView _puzzleInstallerView;

    public BrowserView(PuzzlePickerView puzzlePickerView, 
        PuzzleInstallerView puzzleInstallerView,
        EventBus eventBus)
    {
      _puzzlePickerView = puzzlePickerView;
      _puzzleInstallerView = puzzleInstallerView;

      Add(_puzzlePickerView);

      KeyDown += (sender,args) => {
        if (args.KeyCode == KeyCode.Tab) {
          _picker=!_picker;
          RemoveAll();
          Add( _picker ? _puzzlePickerView : _puzzleInstallerView );
        }
        };
    }
}

// Defines a top-level window with border and title
public class PuzzlePickerView : Window
{

    private CrosswordService _crosswordService;
    private EventBus _eventBus;

    public PuzzlePickerView(CrosswordService crosswordService,EventBus eventBus)
    {
        _crosswordService = crosswordService;
        _eventBus = eventBus;

        Title = $"Puzzle Picker :  ({Application.QuitKey} to quit)";
        var label = new Label { Text = " Select a Puzzle " };

        var dt = new DataTable();

        dt.Columns.Add("Type");
        dt.Columns.Add("Title");
        dt.Columns.Add("Complete");
        dt.Columns.Add("Elapsed");

        List<CrosswordHeader> headers = crosswordService.GetCrosswordHeaders();
        headers.ForEach( ch => {
          dt.Rows.Add(new String [] {$"{ch.Type}",$"{ch.Title}",$"{ch.Complete}",$"{ch.Elapsed}"});
        });

        var tableView = new TableView() {
          X = 0,
          Y= 0,
          Width = Dim.Fill(),
          Height = Dim.Fill()
        };

        tableView.Table = new DataTableSource(dt);
        tableView.FullRowSelect = true;
        tableView.KeyDown += (sender,args) => {

          if (args.KeyCode == KeyCode.Enter) {
            int r = tableView.SelectedRow;
            String rowString = $"{tableView.Table[r,0]} {tableView.Table[r,1]}";
            int option = MessageBox.Query(30,5,"Start Puzzle ",rowString,"ok","cancel");
            if ( option == 0 ) {
              MessageBox.Query(30,5,"System","Starting Puzzle","OK");
              int puzzleId = headers[r].PuzzleId;
              _eventBus.PostEvent(new StartPuzzleEventArgs(puzzleId));
            } 
          }

          //note the accessor checks for oob
          if (args.KeyCode == KeyCode.J) {
            tableView.SelectedRow++;
          }

          if (args.KeyCode == KeyCode.K) {
            tableView.SelectedRow--;
          }

        };

        // Add the views to the Window
        Add(tableView);
    }
}

public class PuzzleInstallerView : Window
{

    public PuzzleInstallerView ()
    {

        Title = $"Puzzle Installer :  ({Application.QuitKey} to quit)";
        var label = new Label { Text = " Install a puzzle " };

        var dt = new DataTable();

        dt.Columns.Add("Date");
        dt.Columns.Add("Status");

        dt.Rows.Add(new String [] {"11/8/2024","NOT INSTALLED"});
        dt.Rows.Add(new String [] {"11/9/2024","INSTALLED"});

        var tableView = new TableView() {
          X = 0,
          Y= 0,
          Width = Dim.Fill(),
          Height = 6
        };

        tableView.Table = new DataTableSource(dt);
        tableView.FullRowSelect = true;
        tableView.KeyDown += (sender,args) => {

          if (args.KeyCode == KeyCode.Enter) {
            int r = tableView.SelectedRow;
            String rowString = $"{tableView.Table[r,0]} {tableView.Table[r,1]}";
            int option = MessageBox.Query(30,5,"Install Puzzle ",rowString,"install","cancel");
            if ( option == 0 ) {
              //do the install
            } 
          }

          //note the accessor checks for oob
          if (args.KeyCode == KeyCode.J) {
            tableView.SelectedRow++;
          }

          if (args.KeyCode == KeyCode.K) {
            tableView.SelectedRow--;
          }

        };

        // Add the views to the Window
        Add(tableView);
    }
}

// Defines a top-level window with border and title
public class GameView : Window
{
    private CluesView _cluesView;
    private GridView _gridView;
    private EventBus _eventBus;
    public GameView (CluesView cluesView,GridView gridView,EventBus eventBus)
    {
        _cluesView = cluesView;
        _gridView = gridView;
        _eventBus = eventBus;

        Title = $"Game :  ({Application.QuitKey} to quit)";
        var label = new Label { Text = " welcome to the puzzle " };

        gridView.X = 0;
        gridView.Width = Dim.Percent(50);

        cluesView.X = Pos.Percent(50);
        cluesView.Width = Dim.Percent(50);

        Add(gridView);
        Add(cluesView);

        KeyDown += (sender,args) => {
          if (args.KeyCode == KeyCode.F8) {
              MessageBox.Query(30,5,"System","Ending Puzzle","OK");
              _eventBus.PostEvent(new EndPuzzleEventArgs());
          }
          };
    }
}

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
      _acrossTableView.Style.AlwaysShowHeaders = true;
      //seems to be broken?
      _acrossTableView.Style.ShowHorizontalBottomline = false;

      _downTableView = new TableView() {
        X = 0,
        Y= Pos.Percent(50),
        Width = Dim.Fill(),
        Height = Dim.Percent(40)
      };
      _downTableView.FullRowSelect = true;
      _downTableView.MinCellWidth = 8;
      _downTableView.Style.AlwaysShowHeaders = true;
      //seems to be broken?
      _downTableView.Style.ShowHorizontalBottomline = true;

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

public class GridView : Window
{
  private DatabaseContext _dbContext;
  private EventBus _eventBus;
  private List<Word> words;

  public GridView(DatabaseContext dbContext,EventBus eventBus) 
  {
      _dbContext = dbContext;
      _eventBus = eventBus;
      _eventBus.Register(this,(args) => { 
        if ( args is StartPuzzleEventArgs ) {
          OnStartPuzzleEvent((StartPuzzleEventArgs) args);
        }
      });
  }

  public override void OnDrawContent(Rectangle contentArea) {
    base.OnDrawContent(contentArea);
    var ctx = Driver;
    for (int y = 0; y < contentArea.Height; y++) {
      for (int x = 0; x < contentArea.Width; x++)
      {
          Move(x, y); // Move cursor to the position
          ctx.AddRune((Rune)((x + y) % 26 + 'A')); // Draw a character
      }
    }
  }

  private void Init(int crosswordId) {
    char[,] gridData =
      {
          { 'H', 'E', 'L', 'L', 'O' },
          { '_', '_', '_', '_', '_' },
          { 'W', 'O', 'R', 'L', 'D' },
          { '_', '_', '_', '_', '_' },
          { 'T', 'E', 'S', 'T', '_' }
      };
    SetNeedsDisplay();
  }

  private void OnStartPuzzleEvent(StartPuzzleEventArgs args) {
    Init(args.CrosswordId);
  }
}
