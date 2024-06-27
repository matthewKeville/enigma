using UI.Command;
using UI.View.ViewModel;

namespace UI.Controller {

public class GameController {

  private GameViewModel gameViewModel;
  private GridController gridController;

  public GameController(GameViewModel gameViewModel,
      GridController gridController) {
    this.gameViewModel = gameViewModel;
    this.gridController = gridController;
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
