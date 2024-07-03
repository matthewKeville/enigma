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
  private ContextAccessor contextAccessor;

  public RootController(RootView rootView,CommandInterpreter commandInterpreter,
      ContextAccessor contextAccessor,GameController gameController) {

    this.contextAccessor = contextAccessor;
    commandInterpreter.raiseCommandEvent += ProcessCommandEvent;

    this.rootModel = contextAccessor.getContext().rootModel;
    this.rootView = rootView;
    this.rootView.setContext(contextAccessor.getContext());
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
