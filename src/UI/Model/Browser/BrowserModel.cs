namespace UI.Model.Browser {

  public class CrosswordHeader {
    public String name = "debug";
    public DateTime date = DateTime.UtcNow;
    public bool started;
    public bool complete;
    public int puzzleId; //todo be guid

  }

  public class BrowserModel : IModel {

    public CrosswordHeader getActiveHeader { 
      get {
        return headers[selection];
      }
    }

    public List<CrosswordHeader> headers;
    public int selection = 0;
  }

}
