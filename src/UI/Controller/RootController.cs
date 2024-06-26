using UI.Command;
using View.ViewModel;

namespace UI.Controller {

public class RootController {

  private RootViewModel rootViewModel;
  private GameController gameController;
  public RootController(RootViewModel rootViewModel,
      CommandInterpreter commandInterpreter,
      GameController gameController
    ) {
    this.rootViewModel = rootViewModel;
    this.gameController = gameController;
    commandInterpreter.raiseCommandEvent += ProcessCommandEvent;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.SWITCH_VIEW:
        rootViewModel.SwitchWindow();
        break;
      //Fall through
      case Command.Command.MOVE_LEFT:
      case Command.Command.MOVE_RIGHT:
      case Command.Command.MOVE_UP:
      case Command.Command.MOVE_DOWN:

      //Fall through
      case Command.Command.INSERT_CHAR:
      case Command.Command.DEL_CHAR:
      case Command.Command.SWAP_ORIENTATION:
        if (rootViewModel.activeWindow == Window.GAME) {
          gameController.ProcessCommandEvent(this,commandEventArgs);
        }
        break;
    }
  }


}

}
