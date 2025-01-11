using Enums;

namespace Event {

  public class StartPuzzleEventArgs : EventArgs {
    public int CrosswordId;

    public StartPuzzleEventArgs(int crosswordId) {
      CrosswordId = crosswordId;
    }
  }

  public class FocusClueChangeEventArgs : EventArgs {
    public (int AcrossOrdinal,int DownOrdinal) ActiveClues;
    public FocusClueChangeEventArgs((int acrossOrdinal,int downOrdinal) activeClues) {
      ActiveClues = activeClues;
    }
  }

}
