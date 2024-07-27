using Enums;
using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Model.Game;
using UI.View.Spectre.Game.Complete;
using UI.View.Spectre.Status;

namespace UI.View.Spectre.Game {

  public class GameView : SpectreView<GameModel> {

    private CluesView cluesView;
    private StatusView statusView;
    private GridView gridView;
    private CompleteView completeView;

    public GameView(StatusView statusView,CluesView cluesView,GridView gridView,CompleteView completeView) {
      this.statusView = statusView;
      this.gridView = gridView;
      this.cluesView = cluesView;
      this.completeView = completeView;
    }
    
    protected override IRenderable render() {

      if ( model.activeWindow == Window.GAME ) {

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

      } else {
        return completeView.Render();
      }

    }

  }
}
