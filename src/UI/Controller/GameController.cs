using UI.Command;
using UI.View.ViewModel;

namespace UI.Controller {

public class GameController {

  private GameViewModel gameViewModel;
  private GridController gridController;

  public GameController(GameViewModel gameViewModel,
      GridController gridController) {
    this.gameViewModel = gameViewModel;
    this.gridController = gridController;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      //fallthrough
      case Command.Command.MOVE_LEFT:
      case Command.Command.MOVE_RIGHT:
      case Command.Command.MOVE_UP:
      case Command.Command.MOVE_DOWN:
      case Command.Command.INSERT_CHAR:
      case Command.Command.DEL_CHAR:
      case Command.Command.SWAP_ORIENTATION:
        gridController.ProcessCommandEvent(this,commandEventArgs);
        break;
    }
  }

}

}
