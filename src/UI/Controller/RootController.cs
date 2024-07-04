using System.Diagnostics;
using Context;
using Enums;
using Model;
using Services;
using UI.Command;
using UI.View.Spectre;

namespace UI.Controller {

public class RootController {

  private RootView rootView;
  private RootModel rootModel;
  private GameController gameController;
  private ContextAccessor contextAccessor;

  public RootController(RootView rootView,CommandDispatcher commandDispatcher,
      ContextAccessor contextAccessor,GameController gameController) {

    this.contextAccessor = contextAccessor;
    commandDispatcher.raiseCommandEvent += ProcessCommandEvent;

    this.rootModel = contextAccessor.getContext().rootModel;
    this.rootView = rootView;
    this.rootView.setContext(contextAccessor.getContext());
    this.gameController = gameController;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {

      case Command.Command.DBG_PUZZLE_SWAP:
        Trace.WriteLine("puzzle swap triggered in root");
        //tmp test
        NYDebugCrosswordGenerator gen = new NYDebugCrosswordGenerator();
        contextAccessor.setContext(new ApplicationContext(gen.sample2()));
        //this.rootModel = contextAccessor.getContext().rootModel;
        gameController.ProcessCommandEvent(this,commandEventArgs);
        break;
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
