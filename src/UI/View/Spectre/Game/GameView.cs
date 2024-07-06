using Context;
using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Model.Game;
using UI.View.Spectre.Status;

namespace UI.View.Spectre.Game {

  public class GameView : SpectreView<GameModel> {

    private CluesView cluesView;
    private StatusView statusView;
    private GridView gridView;

    public GameView(ContextAccessor ctx,StatusView statusView,CluesView cluesView,GridView gridView) {
      Register(ctx);
      this.statusView = statusView;
      this.gridView = gridView;
      this.cluesView = cluesView;
    }
    
    protected override IRenderable render() {

      Table table = new Table();
      table.Centered();
      table.AddColumn(new TableColumn("Grid"));
      table.AddColumn(new TableColumn("Clues"));
      table.HideHeaders();
      table.NoBorder();

      table.AddRow(gridView.Render(),cluesView.Render());

      Layout layout = new Layout("Root")
        .SplitRows(
            new Layout("top"),
            new Layout("bottom")
        );
      layout["top"].Update(statusView.Render());
      layout["bottom"].Update(table);
      layout["top"].Size = 3;
      return layout;

      //return table;
    }

  }
}
