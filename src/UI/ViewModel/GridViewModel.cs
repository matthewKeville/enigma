using System.Drawing;
using System.Diagnostics;
using Model;
using Services;
using System.ComponentModel;

namespace UI.View.ViewModel {

public enum Move {
  RIGHT,
  UP,
  LEFT,
  DOWN
}

//Data model x,y is col,row
//Entry is col, row
//CharMatrix is row col
// ^ this is fucked

public class GridViewModel {

  private ICrosswordProvider crosswordProvider;
  private Crossword crossword;

  public int ColumnCount { 
    get {
      return crossword.colCount;
    }
  }
  public int RowCount { 
    get {
      return crossword.rowCount;
    }
  }

  // x,y = {row,col}
  public Point entry { get; private set; } 
  public char[,] charMatrix { get; set; }

  public GridViewModel(ICrosswordProvider crosswordProvider) {
    this.crosswordProvider = crosswordProvider;
    this.crosswordProvider.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => {
      lock(this) {
        this.crossword = this.crosswordProvider.crossword;
        createCharMatrix();
      }
    };

    this.crossword = crosswordProvider.crossword;
    this.entry = new Point(0,0);
    createCharMatrix();
  }

  private void createCharMatrix() {

    //Create skeleton
    this.charMatrix = new char[crossword.rowCount,crossword.colCount];
    for ( int i = 0; i < crossword.rowCount; i++ ) {
      for ( int j = 0; j < crossword.colCount; j++ ) {
        this.charMatrix[i,j] = '\0';
      }
    }

    //convert words in character matrix

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

  private void MoveEntry(Move move) {

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
