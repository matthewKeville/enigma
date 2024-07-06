using Context;
using Enums;
using UI.Command;
using UI.Controller.Browser;
using UI.Controller.Game;
using UI.Model;

namespace UI.Controller {

public class RootController : Controller<RootModel> {

  private ContextAccessor contextAccessor;
  private GameController gameController;
  private BrowserController  browserController;

  public RootController(CommandDispatcher commandDispatcher,
      ContextAccessor ctx,GameController gameController,
      BrowserController browserController) {
    this.contextAccessor = ctx;
    Register(ctx);
    commandDispatcher.RaiseCommandEvent += ProcessCommandEvent;
    this.browserController = browserController;
    this.gameController = gameController;
  }

  private void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    if (commandEventArgs.command == Command.Command.TOGGLE_HELP) {
      model.ToggleHelp();
    }

    switch ( model.activeWindow ) {

      case Window.GAME:
        gameController.ProcessCommandEvent(this,commandEventArgs);
        break;
      case Window.BROWSER:
        browserController.ProcessCommandEvent(this,commandEventArgs);
        break;
      default:
        break;

    }
  }


}

}
