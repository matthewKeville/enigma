using Enums;

namespace UI.Model.Game {

  public class GameModel : IModel {

    public int crosswordId = 0;
    public Pane activePane = Pane.GRID;

    public void SwapPane() {
      if (activePane == Pane.CLUES) {
        activePane = Pane.GRID;
      } else {
        activePane = Pane.CLUES;
      }
    }
  }

}
