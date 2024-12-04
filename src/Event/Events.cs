namespace Event {

  public class StartPuzzleEventArgs : EventArgs {
    public int CrosswordId;

    public StartPuzzleEventArgs(int crosswordId) {
      CrosswordId = crosswordId;
    }

    public class EndPuzzleEventArgs : EventArgs {

    }
  }
}
