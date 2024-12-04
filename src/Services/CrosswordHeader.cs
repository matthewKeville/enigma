using Enums;

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
