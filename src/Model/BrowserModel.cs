namespace Model {


  public class PuzzleHeader {
    public String name = "debug";
    public DateTime date = DateTime.UtcNow;
    public bool started;
    public bool complete;
    public int puzzleId; //todo be guid

  }

  public class BrowserModel {
    public List<PuzzleHeader> headers;
    public int selection = 0;
  }

}
