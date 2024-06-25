using Model;
using UI;
using Spectre.Console;
using System.Diagnostics;

Trace.Listeners.Add(new TextWriterTraceListener("/logs/enigma.log"));
Trace.AutoFlush = true;

/**
  cats#f
  on#two
  bday#g
*/
Crossword crossword = new Crossword(3,6);


// ROW 1
                            //x,y,i
crossword.words.Add(new Word(0,0,1,Direction.Across,"cats","?"));
crossword.words.Add(new Word(0,0,1,Direction.Down,"cob","?"));
crossword.words.Add(new Word(1,0,2,Direction.Down,"and","?"));
crossword.words.Add(new Word(3,0,3,Direction.Down,"sty","?"));
crossword.words.Add(new Word(5,0,4,Direction.Down,"fog","?"));

// ROW 2

crossword.words.Add(new Word(0,1,5,Direction.Across,"on","?"));
crossword.words.Add(new Word(3,1,6,Direction.Across,"two","?"));

// ROW 3

crossword.words.Add(new Word(0,2,7,Direction.Across,"bday","?"));

UI.Grid grid = new UI.Grid(crossword);
UI.Clues clues = new UI.Clues(crossword);
//int i = 0;

void render() {

  while ( true ) {

    AnsiConsole.Clear();

    Layout root = new Layout();
    root.SplitColumns(new Layout("Grid"),new Layout("Clues"));
    root["Grid"].Update(grid.Render());
    root["Clues"].Update(clues.Render());

    //render
    Layout padRoot = new Layout();
    padRoot.Update(new Padder(root));

    AnsiConsole.Write(padRoot);
   
    //end render

    AnsiConsole.Write(root);
    System.Threading.Thread.Sleep(100);

  }

}

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

Thread renderThread = new Thread(new ThreadStart(render));
renderThread.Start();

Thread ioThread = new Thread(new ThreadStart(processIO));
ioThread.Start();
