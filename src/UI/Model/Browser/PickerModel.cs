using Enums;

namespace UI.Model.Browser {

  public class CrosswordHeader {
    public String Title;
    public CrosswordType Type;
    public DateTime Published;
    public DateTime? StartDate;
    public DateTime? FinishDate;
    public TimeSpan Elapsed;
    public int PuzzleId; //todo be guid
    public bool Complete { get {
      return !(FinishDate is null);}
    }
    public bool Started { get {
      return !(StartDate is null);}
    }

  }

  public class PickerModel : IModel {

    public CrosswordHeader getActiveHeader { 
      get {
        return headers[selection];
      }
    }

    public List<CrosswordHeader> headers;
    public int selection = 0;
    public void MoveUp() {
      selection = Math.Max(0,selection-1);
    }
    public void MoveDown() {
      selection = Math.Min(selection+1,headers.Count()-1);
    }
  }

}
