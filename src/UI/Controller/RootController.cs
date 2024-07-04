using Context;
using Entity;
using Enums;
using Services;
using UI.Command;
using UI.Controller.Browser;
using UI.Controller.Game;
using UI.Model;
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
    commandDispatcher.RaiseCommandEvent += ProcessCommandEvent;

    this.rootModel = contextAccessor.GetContext().rootModel;
    this.rootView = rootView;
    this.rootView.SetContext(contextAccessor.GetContext());
    this.helpView = helpView;
    this.helpView.SetContext(contextAccessor.GetContext());
    this.browserController = browserController;
    this.gameController = gameController;
    this.crosswordService = crosswordService;
  }

  private void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {

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

              int crosswordId = contextAccessor.GetContext().browserModel.getActiveHeader.puzzleId;
              Crossword crossword = crosswordService.GetCrossword(crosswordId);
              this.contextAccessor.UpdateContext(crossword);
              this.rootView.SetContext(contextAccessor.GetContext());
              
              this.contextAccessor.GetContext().rootModel.activeWindow = Window.GAME;
              this.rootModel = contextAccessor.GetContext().rootModel;
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
