namespace UI.View.ViewModel {

  public enum Pane {
    GRID,
    CLUES
  }

  public class GameViewModel {
    public Pane activePane = Pane.GRID;
  }

}
