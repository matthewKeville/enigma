using System.Diagnostics;
using Context;
using UI.Command;
using UI.Model.Game;

namespace UI.Controller.Game {

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
      case Command.Command.UPDATE_CONTEXT:
        Trace.WriteLine("puzzle swap triggered in game controller");
        this.game = contextAccessor.getContext().gameModel;
        gridController.ProcessCommandEvent(this,commandEventArgs);
        cluesController.ProcessCommandEvent(this,commandEventArgs);
        break;
      default:
        gridController.ProcessCommandEvent(this,commandEventArgs);
        break;
    }
  }

}

}
