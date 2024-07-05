using System.Drawing;
using Enums;

namespace UI.Model.Game {

  public class GridModel : IModel {

    public Direction Orientation;
    public Point Entry;
    public char[,] CharMatrix;
    public int ColumnCount = 10;
    public int RowCount = 10;

    //grid model needs to know about word boundaries,
    //but the whole word is not necessary a grid word
    //with the dimensions of the word, orientation and ordinal
    //should suffice.
    public List<WordModel> Words;

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


    public void InsertKey(ConsoleKey key) {
      CharMatrix[Entry.X,Entry.Y] = (char) key;
      MoveEntry(Orientation == Direction.Across ? Move.RIGHT : Move.DOWN);
    }

    public void DeleteKey() {
      CharMatrix[Entry.X,Entry.Y] = ' ';
      MoveEntry(Orientation == Direction.Across ? Move.LEFT : Move.UP);
    }

    public void DeleteWord() {

      WordModel word = ActiveWord();

      for ( int n = 0; n < word.answer.Count(); n++ ) {
        if ( word.direction == Direction.Across ) {
          CharMatrix[word.x + word.answer.Count()-1 - n , word.y] = ' ';
        } else {
          CharMatrix[word.x,word.y + word.answer.Count()-1 - n ] = ' ';
        }
      }

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


}
}
