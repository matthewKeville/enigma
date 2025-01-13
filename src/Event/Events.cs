using UI.Model;

namespace Event {

  public class StartPuzzleEventArgs : EventArgs {
    public int CrosswordId;

    public StartPuzzleEventArgs(int crosswordId) {
       CrosswordId = crosswordId;
    }
  }

  public class PuzzleLoadedEventArgs : EventArgs {
    public GameModel GameModel;

    public PuzzleLoadedEventArgs(GameModel gameModel) {
       GameModel = gameModel;
    }
  }

  public class FocusClueChangeEventArgs : EventArgs {
    public FocusClueChangeEventArgs() {
    }
  }

  public class OrientationChangeEventArgs : EventArgs {
    public OrientationChangeEventArgs() {
    }
  }

  public class ClueViewChangeEventArgs : EventArgs {
    public ClueViewChangeEventArgs() {
    }
  }

  public class CheckChangeEventArgs : EventArgs {
    public CheckChangeEventArgs() {
    }
  }

}
