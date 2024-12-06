namespace Event {

  public class StartPuzzleEventArgs : EventArgs {
    public int CrosswordId;

    public StartPuzzleEventArgs(int crosswordId) {
      CrosswordId = crosswordId;
    }
  }

  public class EndPuzzleEventArgs : EventArgs {

  }

  public class FocusClueChangeEventArgs : EventArgs {
    public int Ordinal;
    public FocusClueChangeEventArgs(int ordinal) {
      Ordinal = ordinal;
    }
  }
}
