using System.Diagnostics;
using System.Drawing;
using Enums;

namespace UI.Model.Game {

  public class GridModel {

    public char[,] charMatrix;
    public Point entry = new Point(0,0);
    public Direction orientation = Direction.Across;

    public int ColumnCount = 10;
    public int RowCount = 10;
    public List<WordModel> words;

    public GridModel(int columnCount,int rowCount,List<WordModel> words) {
      this.ColumnCount = columnCount;
      this.RowCount = rowCount;
      this.words = words;
      createCharMatrix();
    }

  // i = col , j = row
  private void createCharMatrix() {

    this.charMatrix = new char[ColumnCount,RowCount];

    //set all to block 
    for ( int i = 0; i < ColumnCount; i++ ) {
      for ( int j = 0; j < RowCount; j++ ) {
        this.charMatrix[i,j] = '\0';
      }
    }

    //set words as spaces
    foreach ( WordModel word in words ) {
      for ( int x = 0; x < word.answer.Count(); x++ ) {
        if ( word.direction == Direction.Across ) {
          charMatrix[word.x+x,word.y] = ' ';
        } else {
          charMatrix[word.x,word.y+x] = ' ';
        }
      }
    }


    
  }

  //return the word the cursor is in
  public List<WordModel> InWords(int x,int y) {
    List<WordModel> inWords = words.FindAll( w => {
    //WordModel? word = words.FirstOrDefault( w => {
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
          Enumerable.Range(wxs,wxf-wxs+1).Contains(x) && 
          Enumerable.Range(wys,wyf-wys+1).Contains(y);
    });
    return inWords;
  }

  public bool IsInWord(int x,int y) {
    return InWords(x,y).Any();
  }

  public WordModel? ActiveWord() {
    return InWords(entry.X,entry.Y).FirstOrDefault( w => w.direction == orientation,null);
  }

  public bool InActiveWord(int x, int y) {
    WordModel? expectWordModel = InWords(entry.X,entry.Y).FirstOrDefault( w => w.direction == orientation, null);
    if ( expectWordModel == null ) {
      Trace.WriteLine("ERROR : no active word");
      return false;
    }
    WordModel word = (WordModel) expectWordModel;
    if (word.direction == Direction.Across) {
      return (word.x <= x && x <= word.x + word.answer.Count()) && ( y == word.y );
    } else {
      return ( word.x == x ) && ( word.y <= y && y <= word.y + word.answer.Count());
    }
  }

  }

}
