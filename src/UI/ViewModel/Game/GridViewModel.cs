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
  private Game game;

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
  public Direction orientation = Direction.Across;

  public GridViewModel(ICrosswordProvider crosswordProvider) {
    this.crosswordProvider = crosswordProvider;
    this.crosswordProvider.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => {
      lock(this) {
        Trace.WriteLine("crossword changed in gridviewmodel");
        this.crossword = this.crosswordProvider.crossword;
        createCharMatrix();
      }
    };
    this.crossword = crosswordProvider.crossword;
    this.entry = new Point(0,0);
    createCharMatrix();
    createNewGame();
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

  private void createNewGame() {
    List<AnswerBlank> answerBlanks = new List<AnswerBlank>();
    foreach ( Word word in crossword.words ) {
      AnswerBlank blank = new AnswerBlank();
      blank.direction = word.direction;
      blank.size = word.answer.Count();
      blank.ordinal = word.i;
      answerBlanks.Add(blank);
    }
    game = new Game(answerBlanks);
  }

  //return the word the cursor is in
  private List<Word> CurrentWords(int x,int y) {
    List<Word> words = crossword.words.FindAll( w => {
    //Word? word = crossword.words.FirstOrDefault( w => {
        int wxs = w.x;
        int wxf = w.direction == Direction.Across ? 
          w.x + w.answer.Count() -1:
          w.x;
        int wys = w.y;
        int wyf = w.direction == Direction.Down ? 
          w.y + w.answer.Count() -1:
          w.y;
        // Trace.WriteLine(
        //     string.Format("{0} {1} {2} : {3} {4} {5}",wxs,x,wxf,wys,y,wyf));
        return 
          // didn't realize Range(start,count) : caused a headache
          Enumerable.Range(wxs,wxf-wxs+1).Contains(x) && 
          Enumerable.Range(wys,wyf-wys+1).Contains(y);
    });
    return words;
  }

  private bool IsInWord(int x,int y) {
    return CurrentWords(x,y).Any();
  }

  public bool InActiveWord(int x, int y) {
    Word? expectWord = CurrentWords(entry.X,entry.Y).FirstOrDefault( w => w.direction == orientation, null);
    if ( expectWord == null ) {
      Trace.WriteLine("ERROR : no active word");
      Environment.Exit(2);
    }
    Word word = (Word) expectWord;
    if (word.direction == Direction.Across) {
      return (word.x <= x && x <= word.x + word.answer.Count()) && ( y == word.y );
    } else {
      return ( word.x == x ) && ( word.y <= y && y <= word.y + word.answer.Count());
    }
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

  public void SwapOrientation() {
    orientation = ( orientation == Direction.Across ) ? Direction.Down : Direction.Across;
  }

  public void InsertKey(ConsoleKey key) {
    Trace.WriteLine("GridView recieved insert key invocation");
    charMatrix[entry.Y,entry.X] = (char) key;
    //todo keep current writing orientation
    MoveEntry(orientation == Direction.Across ? Move.RIGHT : Move.DOWN);
  }

  public void DeleteKey() {
    Trace.WriteLine("GridView recieved insert key invocation");
    charMatrix[entry.Y,entry.X] = ' ';
    MoveEntry(orientation == Direction.Across ? Move.LEFT : Move.UP);
  }

}

}
