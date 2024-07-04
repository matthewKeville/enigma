using Context;
using UI.Command;
using UI.Model.Game;
using UI.View.Spectre.Game;

namespace UI.Controller.Game {

public class GameController {

  private GameModel gameModel;
  private GameView gameView;
  private GridController gridController;
  private CluesController cluesController;
  private ContextAccessor contextAccessor;

  public GameController(ContextAccessor contextAccessor,GridController gridController,CluesController cluesController,GameView gameView) {
    this.contextAccessor = contextAccessor;
    this.gameModel = contextAccessor.GetContext().gameModel;
    this.gridController = gridController;
    this.cluesController = cluesController;
    this.gameView = gameView;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.UPDATE_CONTEXT:
        Trace.WriteLine("puzzle swap triggered in game controller");
        this.gameModel = contextAccessor.GetContext().gameModel;
        this.gameView.SetContext(this.gameModel);
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
