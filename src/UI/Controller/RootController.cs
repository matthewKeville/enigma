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
      default:
        if (rootViewModel.activeWindow == Window.GAME) {
          gameController.ProcessCommandEvent(this,commandEventArgs);
        }
        break;
    }
  }


}

}
