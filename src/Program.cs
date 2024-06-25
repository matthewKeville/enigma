using Foos;
using Model;
using UI;
using Spectre.Console;

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

CrosswordTable ctable = new CrosswordTable(crossword);
AnsiConsole.Write(ctable.Render());
