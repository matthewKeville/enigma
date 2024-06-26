using Spectre.Console;

namespace UI.View.Spectre {

  public class MainView {

    private GridView gridView;
    private CluesView cluesView;
    private StatusView statusView;

    public MainView(GridView gridView,CluesView cluesView,StatusView statusView) {
      this.gridView = gridView;
      this.statusView = statusView;
      this.cluesView = cluesView;
    }

    public Layout Render() {
      Layout root = new Layout();
      root.SplitRows(new Layout("Status"),new Layout("main"));
      root["Status"].Size = 4;
      root["Status"].Update(statusView.Render());
      root["main"].SplitColumns(new Layout("Grid"),new Layout("Clues"));
      root["main"]["Grid"].Update(gridView.Render());
      root["main"]["Clues"].Update(cluesView.Render());
      return root;
    }

    /**
    public void Start() {

      // Thread ioThread = new Thread(new ThreadStart(processIO));
      // ioThread.Start();

      Layout root = new Layout();

      root.SplitRows(new Layout("Status"),new Layout("main"));
      root["Status"].Size = 4;

      Layout main = new Layout();
      root["main"].SplitColumns(new Layout("Grid"),new Layout("Clues"));

      AnsiConsole.Live(root)
        .Start(ctx =>
        {
          while (true) {
            root["Status"].Update(statusView.Render());
            root["main"]["Grid"].Update(gridView.Render());
            Trace.WriteLine("udating clues");
            root["main"]["Clues"].Update(cluesView.Render());
            ctx.Refresh();
          }
        });

      Trace.WriteLine("oh fuck");
    }
    */

        /**
    void processIO() {

      while ( true ) {
        if ( Console.KeyAvailable ) {
          ConsoleKeyInfo keyInfo = Console.ReadKey(true);
          switch ( keyInfo.Key ) {
            case ConsoleKey.L:
              gridView.MoveEntry(Move.RIGHT);
              break;
            case ConsoleKey.K:
              gridView.MoveEntry(Move.UP);
              break;
            case ConsoleKey.H:
              gridView.MoveEntry(Move.LEFT);
              break;
            case ConsoleKey.J:
              gridView.MoveEntry(Move.DOWN);
              break;
          }
        }

      }
    }
    */
  }
}
