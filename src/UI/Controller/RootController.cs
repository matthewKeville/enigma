using Context;
using Enums;
using Model;
using UI.Command;
using UI.View.Spectre;

namespace UI.Controller {

public class RootController {

  private RootView rootView;
  private RootModel rootModel;
  private GameController gameController;
  private ContextAccessor ctx;

  public RootController(RootView rootView,CommandInterpreter commandInterpreter,
      ContextAccessor accessor,GameController gameController) {

    commandInterpreter.raiseCommandEvent += ProcessCommandEvent;
    this.rootModel = accessor.getContext().rootModel;
    this.rootView = rootView;
    this.gameController = gameController;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {

      case Command.Command.SWITCH_VIEW:
        rootModel.SwitchWindow();
        break;
      default:
        if (rootModel.activeWindow == Window.GAME) {
          gameController.ProcessCommandEvent(this,commandEventArgs);
        }
        break;

    }
  }


}

}
