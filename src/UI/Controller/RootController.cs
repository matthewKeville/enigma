using System.Diagnostics;
using Context;
using Entity;
using Enums;
using Model;
using Services;
using UI.Command;
using UI.View.Spectre;
using UI.View.Spectre.Help;

namespace UI.Controller {

public class RootController {

  private RootView rootView;
  private RootModel rootModel;
  private GameController gameController;
  private HelpView  helpView;
  private BrowserController  browserController;
  private ContextAccessor contextAccessor;
  private CrosswordService crosswordService;

  public RootController(RootView rootView,CommandDispatcher commandDispatcher,
      ContextAccessor contextAccessor,GameController gameController,HelpView helpView,
      BrowserController browserController, CrosswordService crosswordService ) {

    this.contextAccessor = contextAccessor;
    commandDispatcher.raiseCommandEvent += ProcessCommandEvent;

    this.rootModel = contextAccessor.getContext().rootModel;
    this.rootView = rootView;
    this.rootView.setContext(contextAccessor.getContext());
    this.helpView = helpView;
    this.helpView.setContext(contextAccessor.getContext());
    this.browserController = browserController;
    this.gameController = gameController;
    this.crosswordService = crosswordService;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {

      // case Command.Command.UPDATE_CONTEXT:
      //   Trace.WriteLine("puzzle swap triggered in root");
      //   this.rootModel = contextAccessor.getContext().rootModel;
      //   gameController.ProcessCommandEvent(this,commandEventArgs);
      //   break;
      // case Command.Command.SWITCH_VIEW:
      //   rootModel.SwitchWindow();
      //   break;
      default:
        switch ( rootModel.activeWindow ) {
          case Window.GAME:
            gameController.ProcessCommandEvent(this,commandEventArgs);
            break;
          case Window.BROWSER:
            //load selected puzzle
            if ( commandEventArgs.command == Command.Command.CONFIRM ) {

              int crosswordId = contextAccessor.getContext().browserModel.getActiveHeader.puzzleId;
              Crossword crossword = crosswordService.getCrossword(crosswordId);
              this.contextAccessor.updateContext(crossword);
              this.rootView.setContext(contextAccessor.getContext());
              
              this.contextAccessor.getContext().rootModel.activeWindow = Window.GAME;
              this.rootModel = contextAccessor.getContext().rootModel;
              gameController.ProcessCommandEvent(this,new CommandEventArgs(Command.Command.UPDATE_CONTEXT));
            }
            browserController.ProcessCommandEvent(this,commandEventArgs);
            break;
          default:
            break;
        }
        break;
    }
  }


}

}
