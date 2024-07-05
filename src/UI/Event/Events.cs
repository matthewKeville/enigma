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

}
