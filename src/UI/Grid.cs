using System.Drawing;
using System.Diagnostics;
using Model;
using Spectre.Console;

namespace UI {

enum Move {
  RIGHT,
  UP,
  LEFT,
  DOWN
}

//Data model x,y is col,row
//Entry is col, row
//CharMatrix is row col
// ^ this is fucked

class Grid {

  private const char block = '⊞';
  public Point entry { get; set; } // x,y = {row,col}
  private char[,] charMatrix { get; set; }
  public Crossword crossword { get; set; }
  //UI
  private Table table = new Table();
  private Layout layout = new Layout("Grid");
  private Panel testPanel = new Panel(string.Format("x,y : {0},{1}",0,0));

  public Grid(Crossword crossword) {

    this.crossword = crossword;
    this.entry = new Point(0,0);

    this.charMatrix = new char[crossword.rowCount,crossword.colCount];
    for ( int i = 0; i < crossword.rowCount; i++ ) {
      for ( int j = 0; j < crossword.colCount; j++ ) {
        this.charMatrix[i,j] = block;
      }
    }

    init();
  }

  private void init() {
    for ( int j = 0; j < crossword.colCount; j++ ) {
      table.AddColumn(""+j);
    }
    for ( int i = 0; i < crossword.rowCount; i++ ) {
      string[] rowChars = new string[crossword.colCount];
      for ( int x = 0; x < crossword.colCount; x++ ) {
        rowChars[x] =  charMatrix[i,x].ToString();
      }
      table.AddRow(rowChars);
    }
    table.ShowRowSeparators();
    table.HideHeaders();

    layout.SplitRows(new Layout("Top"),new Layout("Bottom"));

    layout["Top"].Update(table);
    layout["Top"].Size(8);


  }

  //convert words in character matrix
  private void UpdateCharMatrix() {
    
    foreach ( Word word in crossword.words ) {
        for ( int i = 0; i < word.answer.Count(); i++ ) {
          if ( word.direction == Direction.Across ) {
            charMatrix[word.y,word.x+i] = word.answer[i];
          } else {
            charMatrix[word.y+i,word.x] = word.answer[i];
          }
        }
    }
  }

  //return the word the cursor is in
  private Word? CurrentWord(int x,int y) {
    Word? word = crossword.words.FirstOrDefault( w => {
        int wxs = w.x;
        int wxf = w.direction == Direction.Across ? 
          w.x + w.answer.Count() -1:
          w.x;
        int wys = w.y;
        int wyf = w.direction == Direction.Down ? 
          w.y + w.answer.Count() -1:
          w.y;
        Trace.WriteLine(
            string.Format("{0} {1} {2} : {3} {4} {5}",wxs,x,wxf,wys,y,wyf));
        return 
          // didn't realize Range(start,count) : caused a headache
          Enumerable.Range(wxs,wxf-wxs+1).Contains(x) && 
          Enumerable.Range(wys,wyf-wys+1).Contains(y);
    },null);
    Trace.WriteLine(string.Format(" {0},{1} : in word? {2} : word : {3} ",
          x,y,word is not null,word?.answer ?? ""));
    return word;
  }

  private bool IsInWord(int x,int y) {
    return CurrentWord(x,y) is not null;
  }

  //render character matrix to Table
  public Layout Render() {

    UpdateCharMatrix();

    for ( int j = 0; j < crossword.colCount; j++ ) {
      for ( int i = 0; i < crossword.rowCount; i++ ) {
        String charDisplay = ""+charMatrix[i,j];
        if ( entry.X == j && entry.Y == i ) {
          charDisplay = "[blue]"+charDisplay+"[/]";
        }
        table.UpdateCell(i,j,charDisplay);
      }
    }

    Panel testPanel = new Panel(string.Format("x,y : {0},{1}",entry.X,entry.Y));
    testPanel.Padding = new Padding(0,0,0,0);
    layout["Bottom"].Update(testPanel);
    layout["Bottom"].Size(3);

    return layout;

  }

  public void MoveEntry(Move move) {

    int offx = 0;
    int offy = 0;

    //OOB check
    switch (move) {
      case Move.RIGHT:
        if ( entry.X != crossword.colCount-1) {
          offx = 1;
        }
        break;
      case Move.UP:
        if ( entry.Y != 0 ) {
          offy = -1;
        }
        break;
      case Move.LEFT:
        if ( entry.X != 0 ) {
          offx = -1;
        }
        break;
      case Move.DOWN:
        if ( entry.Y != crossword.rowCount-1 ) {
          offy = 1;
        }
        break;
    }

    //Valid word position?
    Point nextEntry = Point.Add(entry,new System.Drawing.Size(offx,offy));
    if ( IsInWord(nextEntry.X,nextEntry.Y) ) {
      entry = nextEntry;
    }

  }

}

}
