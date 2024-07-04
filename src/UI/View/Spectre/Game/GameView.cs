using Context;
using Spectre.Console;
using UI.View.Spectre.Status;

namespace UI.View.Spectre.Game {

  public class GameView : ISpectreView<Layout> {

    private CluesView cluesView;
    private StatusView statusView;
    private GridView gridView;

    public GameView(StatusView statusView,CluesView cluesView,GridView gridView) {
      this.statusView = statusView;
      this.gridView = gridView;
      this.cluesView = cluesView;
    }

    public void SetContext(ApplicationContext ctx) {}

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
