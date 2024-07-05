using Context;
using Enums;
using UI.Command;
using UI.Model.Game;

namespace UI.Controller.Game {

public class GameController : Controller<GameModel> {

  private GridController gridController;
  private CluesController cluesController;
  private ContextAccessor contextAccessor;

  public GameController(ContextAccessor ctx,GridController gridController,CluesController cluesController) {
    this.contextAccessor = ctx;
    Register(ctx);
    this.gridController = gridController;
    this.cluesController = cluesController;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {
      case Command.Command.SWAP_PANE:
        model.SwapPane();
        Trace.WriteLine("pane swap");
        break;
      default:
        break;
    }

    switch ( model.activePane) {

      case Pane.GRID:
        gridController.ProcessCommandEvent(this,commandEventArgs);
        break;
      case Pane.CLUES:
        cluesController.ProcessCommandEvent(this,commandEventArgs);
        break;
      default:
        break;

    }

  }

}

}
