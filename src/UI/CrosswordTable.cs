using Model;
using Spectre.Console;

namespace UI {

class CrosswordTable {

  private const char block = '⊞';
  public Crossword crossword { get; set; }
  private char[,] charMatrix { get; set; }

  public CrosswordTable(Crossword crossword) {
    this.crossword = crossword;
    this.charMatrix = new char[crossword.rowCount,crossword.colCount];
    for ( int i = 0; i < crossword.rowCount; i++ ) {
      for ( int j = 0; j < crossword.colCount; j++ ) {
        this.charMatrix[i,j] = block;
      }
    }
  }

  private void PrintCharMatrix(Char[,] charMatrix,int rows, int cols) {
    String charMatrixString = "";
    for ( int i = 0; i < rows; i++ ) {
      String rowString = "";
      for ( int j = 0; j < cols; j++ ) {
        rowString+=charMatrix[i,j]; 
      }
      charMatrixString+="\n";
      charMatrixString+=rowString;
    }
    Console.WriteLine(charMatrixString);
  }

  private void RenderCharMatrix() {

    //Console.WriteLine("rendering  {0} words ",crossword.words.Count());
    
    foreach ( Word word in crossword.words ) {
        for ( int i = 0; i < word.answer.Count(); i++ ) {
          if ( word.direction == Direction.Across ) {

            //Console.WriteLine(string.Format("across writing to {0},{1} char {2} {3}",word.y,word.x+i,i,word.answer[i]));
            charMatrix[word.y,word.x+i] = word.answer[i];

          } else {
            //Console.WriteLine(string.Format("down writing to {0},{1} char {2} {3}",word.y+i,word.x,i,word.answer[i]));
            charMatrix[word.y+i,word.x] = word.answer[i];
          }
        }
        // Console.WriteLine("word complete");
    }
  }

  public Table Render() {

    RenderCharMatrix();

    Table table = new Table();

    for ( int j = 0; j < crossword.colCount; j++ ) {
      table.AddColumn(""+j);
    }

    for ( int i = 0; i < crossword.rowCount; i++ ) {
      string[] rowChars = new string[crossword.colCount];
      for ( int x = 0; x < crossword.colCount; x++ ) {
        rowChars[x] =  charMatrix[i,x].ToString();
      }
      table.AddRow(rowChars);
      //table.AddRow("a","a","a","☐","a");
    }

    table.ShowRowSeparators();
    table.HideHeaders();
    return table;

  }


}

}
