using Context;
using Model;
using Spectre.Console;
using UI.View.Spectre.Status;

namespace UI.View.Spectre.Game {

  public class GameView {

    //private GridView gridView;
    private CluesView cluesView;
    private StatusView statusView;
    private GridView gridView;
    private ContextAccessor contextAccessor;

    /**
    public GameView(GridView gridView,CluesView cluesView,StatusView statusView) {
      this.gridView = gridView;
      this.statusView = statusView;
      this.cluesView = cluesView;
    }
    */

    public GameView(ContextAccessor contextAccessor,StatusView statusView,CluesView cluesView,GridView gridView) {
      this.contextAccessor = contextAccessor;
      this.statusView = statusView;
      this.gridView = gridView;
      this.cluesView = cluesView;
    }

    public void update(IModel model) {}

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

    /**
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
    */

  }
}
