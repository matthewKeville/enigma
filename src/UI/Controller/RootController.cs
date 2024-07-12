using Enums;
using UI.Command;
using UI.Controller.Browser;
using UI.Controller.Game;
using UI.Controller.Help;
using UI.Event;
using UI.Events;
using UI.Model;
using UI.View.Spectre;

namespace UI.Controller {

public class RootController : Controller<RootModel> {

  private RootView rootView;
  private GameController gameController;
  private BrowserController  browserController;
  private HelpController  helpController;

  public RootController(
      CommandDispatcher commandDispatcher,
      EventDispatcher eventDispatcher,
      RootView rootView,
      GameController gameController,
      BrowserController browserController,
      HelpController helpController) 
  {

    this.model = new RootModel();

    this.rootView = rootView;
    this.rootView.SetModel(this.model);

    this.browserController = browserController;
    this.gameController = gameController;
    this.helpController = helpController;

    eventDispatcher.RaiseEvent += ProcessEvent;
    commandDispatcher.RaiseCommandEvent += ProcessCommandEvent;

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

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {
      Trace.WriteLine("root controller recieved load puzzle");
      model.activeWindow = Window.GAME;
    }

    if (eventArgs.GetType() == typeof(ExitPuzzleEventArgs)) {
      Trace.WriteLine("root controller exit puzzle");
      model.activeWindow = Window.BROWSER;
    }
  }

}

}
