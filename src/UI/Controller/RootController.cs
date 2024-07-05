using Context;
using Entity;
using Enums;
using Services;
using UI.Command;
using UI.Controller.Browser;
using UI.Controller.Game;
using UI.Model;
using UI.Model.Browser;

namespace UI.Controller {

public class RootController {

  private ContextAccessor contextAccessor;
  private RootModel rootModel;
  private GameController gameController;
  private BrowserController  browserController;
  private CrosswordService crosswordService;

  public RootController(CommandDispatcher commandDispatcher,
      ContextAccessor contextAccessor,GameController gameController,
      BrowserController browserController, CrosswordService crosswordService ) {

    this.contextAccessor = contextAccessor;
    commandDispatcher.RaiseCommandEvent += ProcessCommandEvent;

    this.rootModel = (RootModel) contextAccessor.GetModel<RootModel>();
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

              //perhaps this logic should be delegated to a ContextService?
              //As the controllers act on models, acting on the surrounding context seems to be
              //an anti pattrn. Arguably the RootController is a special controller though.
              
              int crosswordId = ((BrowserModel)contextAccessor.GetModel<BrowserModel>()).getActiveHeader.puzzleId;
              Crossword crossword = crosswordService.GetCrossword(crosswordId);

              //this call to updateContext could be changed into a call to the ContextService
              //for LoadCrossword, which then builds a new context, that set's the active window
              //to Game.
 
              this.contextAccessor.UpdateContext(crossword);
              this.rootModel = (RootModel) contextAccessor.GetModel<RootModel>();
              this.rootModel.activeWindow = Window.GAME;

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
