using Model;
using UI;
using Spectre.Console;
using System.Diagnostics;
using PuzzleProvider;

Trace.Listeners.Add(new TextWriterTraceListener("/logs/enigma.log"));
Trace.AutoFlush = true;

Crossword crossword = DebugPuzzleProvider.GetPuzzle();

UI.Grid grid = new UI.Grid(crossword);
UI.Clues clues = new UI.Clues(crossword);
UI.Status status = new UI.Status(crossword);

void processIO() {

  while ( true ) {

    if ( Console.KeyAvailable ) {
      ConsoleKeyInfo keyInfo = Console.ReadKey(true);
      switch ( keyInfo.Key ) {
        case ConsoleKey.L:
          grid.MoveEntry(Move.RIGHT);
          break;
        case ConsoleKey.K:
          grid.MoveEntry(Move.UP);
          break;
        case ConsoleKey.H:
          grid.MoveEntry(Move.LEFT);
          break;
        case ConsoleKey.J:
          grid.MoveEntry(Move.DOWN);
          break;
      }
    }
  }
}

Thread ioThread = new Thread(new ThreadStart(processIO));
ioThread.Start();


Layout root = new Layout();

root.SplitRows(new Layout("Status"),new Layout("main"));
root["Status"].Size = 4;

Layout main = new Layout();
root["main"].SplitColumns(new Layout("Grid"),new Layout("Clues"));

AnsiConsole.Live(root)
  .Start(ctx =>
  {
    while (true) {
      root["Status"].Update(status.Render());
      root["main"]["Grid"].Update(grid.Render());
      root["main"]["Clues"].Update(clues.Render());
      ctx.Refresh();
    }
  });
