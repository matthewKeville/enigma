using Context;
using Model;
using UI.Command;

namespace UI.Controller {

public class GameController {

  private GameModel game;
  private GridController gridController;
  private CluesController cluesController;
 
  private ContextAccessor contextAccessor;

  public GameController(ContextAccessor contextAccessor,GridController gridController,CluesController cluesController) {
    this.contextAccessor = contextAccessor;
    this.game = contextAccessor.getContext().gameModel;
    this.gridController = gridController;
    this.cluesController = cluesController;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      default:
        gridController.ProcessCommandEvent(this,commandEventArgs);
        break;
    }
  }

}

}
