// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.DependencyInjection;
// using Services;
// using Services.CrosswordInstaller;
// using Services.CrosswordInstaller.NYT;
// HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
// builder.Services.AddSingleton<DatabaseContext, DatabaseContext>();
// builder.Services.AddSingleton<CrosswordService, CrosswordService>();
// builder.Services.AddSingleton<NYTCrosswordInstaller, NYTCrosswordInstaller>();
// builder.Services.AddSingleton<NYTCrosswordParser, NYTCrosswordParser>();
// builder.Services.AddSingleton<CrosswordInstallerService, CrosswordInstallerService>();
// IHost host = builder.Build();
// host.Run();


using System.Data;
using Terminal.Gui;

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;
Application.Run<RootView> ().Dispose ();
Application.Shutdown ();

// Defines a top-level window with border and title
public class RootView : Window
{
    BrowserView bv;
    GameView gv;

    public RootView ()
    {
      bv = new BrowserView( () => {
        showGame();
      });
      gv = new GameView();

      KeyDown += (sender,args) => {
        if (args.KeyCode == KeyCode.Tab) {
          
        }
      };

      showBrowser();
    }

    private void showGame() {
      RemoveAll();
      Add( gv );
    }
    private void showBrowser() {
      RemoveAll();
      Add( bv );
    }
}

public class BrowserView : Window
{
    bool picker = true;
    PuzzlePickerView pv;
    PuzzleInstallerView iv;

    public BrowserView (Action onPuzzleSelected)
    {
      pv = new PuzzlePickerView(onPuzzleSelected);
      iv = new PuzzleInstallerView();
      Add(pv);
      KeyDown += (sender,args) => {
        if (args.KeyCode == KeyCode.Tab) {
          picker=!picker;
          RemoveAll();
          Add( picker ? pv : iv );
        }
        };
    }
}

// Defines a top-level window with border and title
public class PuzzlePickerView : Window
{

    private Action onPuzzleSelected;

    public PuzzlePickerView (Action onPuzzleSelected)
    {
        this.onPuzzleSelected = onPuzzleSelected;

        Title = $"Puzzle Picker :  ({Application.QuitKey} to quit)";
        var label = new Label { Text = " Select a Puzzle " };

        var dt = new DataTable();

        dt.Columns.Add("Source");
        dt.Columns.Add("Date");
        dt.Columns.Add("Status");
        dt.Columns.Add("Elapsed");

        dt.Rows.Add(new String [] {"NYT","12/2/2024","In Progress","23:43"});
        dt.Rows.Add(new String [] {"NYT","12/3/2024","New","00:00"});
        dt.Rows.Add(new String [] {"NYT","12/4/2024","In Progress","00:43"});
        dt.Rows.Add(new String [] {"NYT","12/5/2024","In Progress","43:22"});
        dt.Rows.Add(new String [] {"NYT","12/6/2024","Complete","23:43"});
        dt.Rows.Add(new String [] {"NYT","12/7/2024","Complete","13:12"});
        dt.Rows.Add(new String [] {"NYT","12/8/2024","Complete","17:57"});

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
            int option = MessageBox.Query(30,5,"Start Puzzle ",rowString,"ok","cancel");
            if ( option == 0 ) {
              MessageBox.Query(30,5,"System","Starting Puzzle","OK");
              onPuzzleSelected();
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
    public GameView ()
    {
        Title = $"Game :  ({Application.QuitKey} to quit)";
        var label = new Label { Text = " welcome to the puzzle " };
        Add(label);
    }
}
