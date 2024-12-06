using Entity;
using Enums;

namespace UI.Model.Game {

  public class GridWordModel {
    public int x;
    public int y;
    public int i;
    public Direction direction;
    public String answer ="";
    public String prompt ="";

    public GridWordModel(int x, int y, int i, Direction direction, String answer, String prompt) {
      this.x = x;
      this.y = y;
      this.i = i;
      this.direction = direction;
      this.answer = answer;
      this.prompt = prompt;
    }
  }

  public class GridCharModel {
    public int X;
    public int Y;
    public char C;
    public int Status = 0;
    public bool IsBlock;
    public GridCharModel? Up;
    public GridCharModel? Down;
    public GridCharModel? Left;
    public GridCharModel? Right;

    public GridCharModel(int x, int y, char c, bool isBlock) {
      this.X = x;
      this.Y = y;
      this.C = c;
      this.IsBlock = isBlock;
    }

  }

  public class GridModel {

    public List<GridCharModel> GridCharModels;
    public GridCharModel Selection;

    public int crosswordId;
    public int ColumnCount = 0;
    public int RowCount = 0;
    public Direction Orientation = Direction.Across;

    public int WordCheckCount;
    public List<GridWordModel> Words = new List<GridWordModel>();

    public GridModel(List<GridChar> gridChars) {

      GridCharModels = new List<GridCharModel>();
      foreach ( GridChar gc in  gridChars ) {
        var gcm = new GridCharModel(gc.X,gc.Y,gc.C,gc.C == '\0');
        GridCharModels.Add(gcm);
      }

      foreach ( GridCharModel gcm in GridCharModels ) {
        gcm.Up = GridCharModels.FirstOrDefault( m => m.X == gcm.X && m.Y == gcm.Y-1,null);
        gcm.Down = GridCharModels.FirstOrDefault( m => m.X == gcm.X && m.Y == gcm.Y+1,null);
        gcm.Left = GridCharModels.FirstOrDefault( m => m.X == gcm.X-1 && m.Y == gcm.Y,null);
        gcm.Right = GridCharModels.FirstOrDefault( m => m.X == gcm.X+1 && m.Y == gcm.Y,null);
      }

      Selection = GridCharModels.First();

    }

    public bool MoveUp() {
      Trace.WriteLine("Hit move up");
      if ( !Selection.Up?.IsBlock ?? false ) {
        Selection = Selection.Up;
        return true;
      }
      return false;
    }

    public bool MoveDown() {
      Trace.WriteLine("Hit move down");
      if ( !Selection.Down?.IsBlock ?? false ) {
        Selection = Selection.Down;
        return true;
      }
      return false;
    }

    public bool MoveRight() {
      if ( !Selection.Right?.IsBlock ?? false ) {
        Selection = Selection.Right;
        return true;
      }
      return false;
    }

    public bool MoveLeft() {
      if ( !Selection.Left?.IsBlock ?? false ) {
        Selection = Selection.Left;
        return true;
      }
      return false;
    }

    public void SwapOrientation(){
      if ( Orientation == Direction.Across ) {
        Orientation = Direction.Down;
      } else {
        Orientation = Direction.Across;
      }
    }

    public void InsertChar(char c){
      Selection.C = c;
      //todo advance key ...
    }

    public void DeleteChar(){
      Selection.C = ' ';
    }

    /**
    public void MoveToOrdinal(int ordinal,Direction direction) {
    public void MoveEntry(Move move) {
    public WordModel ActiveWord() {
    public bool InActiveWord(int x, int y) {
    private List<WordModel> InWords(int x,int y) {
    private bool InWordRange(WordModel word,int x, int y) {
    public void SwapOrientation() {
    public void InsertKey(ConsoleKey key,bool advance = true) {
    public void DeleteKey(bool advance = true) {
    public void DeleteWord() {
    public void FindChar(ConsoleKey key, bool forward = true) {
    private void FindCharForward(ConsoleKey key) {
    private void FindCharBackward(ConsoleKey key) {
    public void MoveToWordStart() {
    public void MoveToWordEnd() {
    public void MoveWord() {
    public void MoveBackWord() {
    public void MoveAnswer() {
    public void MoveBackAnswer() {
    public void CheckWord() {
    public bool IsComplete() {
    */

    /**
    public void MoveToOrdinal(int ordinal,Direction direction) {
      WordModel moveWord = Words.Find( w => w.i == ordinal && w.direction == direction );
      Entry = new Point(moveWord.x,moveWord.y);
    }

    public void MoveEntry(Move move) {

      int offx = 0;
      int offy = 0;

      //Out of Bounds?
      switch (move) {
        case Move.RIGHT:
          if ( Entry.X != ColumnCount-1) {
            offx = 1;
          }
          break;
        case Move.UP:
          if ( Entry.Y != 0 ) {
            offy = -1;
          }
          break;
        case Move.LEFT:
          if ( Entry.X != 0 ) {
            offx = -1;
          }
          break;
        case Move.DOWN:
          if ( Entry.Y != RowCount-1 ) {
            offy = 1;
          }
          break;
      }

      //Valid word position
      //Is this redundant?
      Point nextEntry = Point.Add(Entry,new System.Drawing.Size(offx,offy));
      if ( InWords(nextEntry.X,nextEntry.Y).Count() != 0 ) {
        Entry = nextEntry;
      }
    }

    //What is the active word?
    public WordModel ActiveWord() {
      return InWords(Entry.X,Entry.Y).First( w => w.direction == Orientation );
    }

    //determine if this coordinate is inside the active word
    public bool InActiveWord(int x, int y) {
      return InWordRange(ActiveWord(),x,y);
    }


    //return the words that contain this coordinate
    private List<WordModel> InWords(int x,int y) {
      List<WordModel> inWords = Words.FindAll( w => 
        { return InWordRange(w,x,y); }
      );
      return inWords;
    }

    //determine if the coordinate is inside the word
    private bool InWordRange(WordModel word,int x, int y) {
        int wxs = word.x;
        int wxf = word.direction == Direction.Across ? 
          word.x + word.answer.Count() -1:
          word.x;
        int wys = word.y;
        int wyf = word.direction == Direction.Down ? 
          word.y + word.answer.Count() -1:
          word.y;
        return 
          Enumerable.Range(wxs,wxf-wxs+1).Contains(x) && 
          Enumerable.Range(wys,wyf-wys+1).Contains(y);
    }

    public void SwapOrientation() {
      Orientation = ( Orientation == Direction.Across ) ? Direction.Down : Direction.Across;
    }


    public void InsertKey(ConsoleKey key,bool advance = true) {
      CharMatrix[Entry.X,Entry.Y] = (char) key;
      StatusMatrix[Entry.X,Entry.Y] = 0;
      if ( advance ) {
        MoveEntry(Orientation == Direction.Across ? Move.RIGHT : Move.DOWN);
      }
    }

    public void DeleteKey(bool advance = true) {
      CharMatrix[Entry.X,Entry.Y] = ' ';
      StatusMatrix[Entry.X,Entry.Y] = 0;
      if ( advance ) {
        MoveEntry(Orientation == Direction.Across ? Move.LEFT : Move.UP);
      }
    }

    public void DeleteWord() {

      WordModel word = ActiveWord();

      for ( int n = 0; n < word.answer.Count(); n++ ) {
        if ( word.direction == Direction.Across ) {
          CharMatrix[word.x + word.answer.Count()-1 - n , word.y] = ' ';
          StatusMatrix[word.x + word.answer.Count()-1-n, word.y ] = 0;
        } else {
          CharMatrix[word.x,word.y + word.answer.Count()-1 - n ] = ' ';
          StatusMatrix[word.x,word.y + word.answer.Count()-1 - n ] = 0;
        }
      }

    }

    //try to move to the character within the current word
    public void FindChar(ConsoleKey key, bool forward = true) {
      if ( forward ) {
        FindCharForward(key);
      }
      FindCharBackward(key);
    }

    //try to move to the character within the current word
    //disregard the current character if it's a match
    private void FindCharForward(ConsoleKey key) {

      WordModel word = ActiveWord();

      int ix = Entry.X;
      int iy = Entry.Y;

      if ( Orientation == Direction.Across ) {
        ix++;
        while ( ix < word.x + word.answer.Count() ) {
          if ( CharMatrix[ix,iy] == (char) key ) {
            Trace.WriteLine($"found char {(char) key}");
            Entry = new Point(ix,iy);
            return;
          }
          ix++;
        }
      } else {
        iy++;
        while ( iy < word.y + word.answer.Count() ) {
          if ( CharMatrix[ix,iy] == (char) key ) {
            Trace.WriteLine($"found char {(char) key}");
            Entry = new Point(ix,iy);
            return;
          }
          iy++;
        }
      }
      Trace.WriteLine($"char not found {(char) key}");
    }

    private void FindCharBackward(ConsoleKey key) {
      //TODO
    }

    public void MoveToWordStart() {
      WordModel word = ActiveWord();
      Entry = new Point(word.x,word.y);
    }

    public void MoveToWordEnd() {
      WordModel word = ActiveWord();
      Entry = word.direction == Direction.Across ? 
        new Point(word.x + word.answer.Count()-1,word.y) :
        new Point(word.x,word.y + word.answer.Count()-1);
    }

    public void MoveWord() {
      WordModel word = ActiveWord();
      WordModel? nextWord = Words
        .FindAll(w => w.direction == Orientation)
        .OrderBy( w => w.i )
        .FirstOrDefault( w => w.i > word.i);
      if ( nextWord is not null ) {
        Entry = new Point(nextWord.x,nextWord.y);
      }
    }

    public void MoveBackWord() {
      WordModel word = ActiveWord();
      WordModel? nextWord = Words
        .FindAll(w => w.direction == Orientation)
        .OrderBy( w => w.i )
        .Reverse()
        .FirstOrDefault( w => w.i < word.i);
      if ( nextWord is not null ) {
        Entry = new Point(nextWord.x,nextWord.y);
      }
    }

    //like MoveWord, but only move to words that
    //the user has added characters to
    public void MoveAnswer() {

      WordModel word = ActiveWord();

      WordModel? nextWord = Words
        .FindAll(w => w.direction == Orientation)
        .OrderBy( w => w.i )
        .Where( w => { 

          //does the answer space have any characters?
           
          int ix = w.x;
          int iy = w.y;

          if ( Orientation == Direction.Across ) {
            while ( ix < w.x + w.answer.Count() ) {
              if ( CharMatrix[ix,iy] != ' ' ) {
                return true;
              }
              ix++;
            }
          } else {
            while ( iy < w.y + w.answer.Count() ) {
              if ( CharMatrix[ix,iy] != ' ' ) {
                return true;
              }
              iy++;
            }
          }
          
          return false;

        } )
        .FirstOrDefault( w => { return w.i > word.i; });

      if ( nextWord is not null ) {
        Entry = new Point(nextWord.x,nextWord.y);
      } 
    }

    //like MoveWord, but only move to words that
    //the user has added characters to
    public void MoveBackAnswer() {

      WordModel word = ActiveWord();

      WordModel? nextWord = Words
        .FindAll(w => w.direction == Orientation)
        .OrderBy( w => w.i )
        .Reverse()
        .Where( w => { 

          //does the answer space have any characters?
           
          int ix = w.x;
          int iy = w.y;

          if ( Orientation == Direction.Across ) {
            while ( ix < w.x + w.answer.Count() ) {
              if ( CharMatrix[ix,iy] != ' ' ) {
                return true;
              }
              ix++;
            }
          } else {
            while ( iy < w.y + w.answer.Count() ) {
              if ( CharMatrix[ix,iy] != ' ' ) {
                return true;
              }
              iy++;
            }
          }
          
          return false;

        } )
        .FirstOrDefault( w => { return w.i < word.i; });

      if ( nextWord is not null ) {
        Entry = new Point(nextWord.x,nextWord.y);
      } else {
      }

    }

    public void CheckWord() {

      WordCheckCount++;
      int z = 0;
      WordModel active = ActiveWord();

      while ( z < active.answer.Count() ) {

        int ix;
        int iy;

        if ( Orientation == Direction.Across ) {
          ix = active.x + z;
          iy = active.y;
        } else {
          ix = active.x;
          iy = active.y+z;
        }

        if ( active.answer[z] != CharMatrix[ix,iy] ) {
          StatusMatrix[ix,iy] = 1;
        } else {
          StatusMatrix[ix,iy] = 2;
        }

        z++;
      }
    }

    public bool IsComplete() {

      foreach ( WordModel word in Words ) {

        int z = 0;
        int ix = word.x;
        int iy = word.y;

        while ( z < word.answer.Count() ) {

          if ( word.answer[z] != CharMatrix[ix,iy] ) {
            Trace.WriteLine($"puzzle incomplete, atelast {word.direction.ToString()} {word.i},{word.x},{word.y} is wrong");
            Trace.WriteLine($" expected {word.answer[z]} , but found {CharMatrix[ix,iy]}");
            Trace.WriteLine($" on answer {word.answer}");
            return false;
          }

          if ( word.direction == Direction.Across ) {
            ix++;
          } else {
            iy++;
          }
          z++;

        }
      }

      return true;

    }
    */

}
}
