using Spectre.Console;
using UI.View.Spectre.Game;

namespace UI.View.Spectre.Game {

  public class GameView {

    private GridView gridView;
    private CluesView cluesView;
    private StatusView statusView;

    public GameView(GridView gridView,CluesView cluesView,StatusView statusView) {
      this.gridView = gridView;
      this.statusView = statusView;
      this.cluesView = cluesView;
    }

    public Layout Render() {
      Layout root = new Layout();
      root.SplitRows(new Layout("Status"),new Layout("main"));
      root["Status"].Size = 4;
      root["Status"].Update(statusView.Render());
      root["main"].SplitColumns(new Layout("Grid"),new Layout("Clues"));
      root["main"]["Grid"].Update(gridView.Render());
      root["main"]["Clues"].Update(cluesView.Render());
      return root;
    }

  }
}
