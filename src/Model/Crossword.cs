using System.Text;

namespace Model {

class Crossword {

  public String name { get; set; } = "debug"; 
  public List<Word> words { get; set; } = new List<Word>();
  public int rowCount { get; set; }
  public int colCount { get; set; }

  public Crossword(int rowCount, int colCount ) {
    this.rowCount = rowCount;
    this.colCount = colCount;
  }

  public override String ToString() {

    StringBuilder sb = new StringBuilder(500);
    sb.Append(string.Format("Crossword {0} has {1} words",name,words.Count));

    String spacer = new String('*',80);
    sb.AppendLine();
    sb.AppendLine();

    foreach ( Word word in words ) {
      sb.Append(word.ToString());
      sb.AppendLine();
      sb.AppendLine();
      sb.AppendLine(spacer);
      sb.AppendLine();
    }

    return sb.ToString();
  }

}

}
