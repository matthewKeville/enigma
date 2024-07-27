using Enums;

namespace UI.Events {

  public class GridWordChangeEventArgs : EventArgs {
    public int ordinal;
    public Direction direction;
    public GridWordChangeEventArgs(int ordinal, Direction direction) {
      Trace.WriteLine("Gird Word Change Event");
      this.ordinal = ordinal;
      this.direction = direction;
    }
  }

  public class CluesWordChangeEventArgs : EventArgs {
    public int ordinal;
    public Direction direction;
    public CluesWordChangeEventArgs(int ordinal, Direction direction) {
      Trace.WriteLine("Clues Word Change Event");
      this.ordinal = ordinal;
      this.direction = direction;
    }
  }

  public class LoadPuzzleEventArgs : EventArgs {
    public int puzzleId;
    public LoadPuzzleEventArgs(int puzzleId) {
      Trace.WriteLine("Load Puzzle Event");
      this.puzzleId = puzzleId;
    }
  }

  public class ExitPuzzleEventArgs : EventArgs {
    public int puzzleId;
    public ExitPuzzleEventArgs(int puzzleId) {
      this.puzzleId = puzzleId;
      Trace.WriteLine("Exit Puzzle Event");
    }
  }

  public class PuzzleCompleteArgs : EventArgs {
    public int puzzleId;
    public PuzzleCompleteArgs(int puzzleId) {
      this.puzzleId = puzzleId;
      Trace.WriteLine("Puzzle Complete Event");
    }
  }

  public class PuzzleInstalledEvent : EventArgs {
    public PuzzleInstalledEvent() {
      Trace.WriteLine("Puzzle Installed Event");
    }
  }

}
