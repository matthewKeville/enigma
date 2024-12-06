using Enums;

namespace UI.Game {

  public class CluesModel {

    public (int AcrossOrdinal,int DownOrdinal) ActiveClues;
    public Direction ActiveOrientation = Direction.Across;
    //bool splitView

    public CluesModel() {
      ActiveClues = (1,1);
    }

    public void UpdateActiveClues(int acrossOrdinal,int downOrdinal) {
      ActiveClues = (acrossOrdinal,downOrdinal);
    }

    public void UpdateOrientation(Direction orientation) {
      ActiveOrientation = orientation;
    }
  }
}
