using Enums;
using Services;
using UI.Command;
using UI.Controller.Game.Status;
using UI.Event;
using UI.Events;
using UI.Model.Game;
using UI.View.Spectre.Game;

namespace UI.Controller.Game {

public class GameController : Controller<GameModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private GameView gameView;
  private GridController gridController;
  private CluesController cluesController;
  private StatusController statusController;

  public GameController(EventDispatcher eventDispatcher,CrosswordService crosswordService,GameView gameView,GridController gridController,CluesController cluesController,StatusController statusController) {
    this.model = new GameModel();

    this.gameView = gameView;
    this.gameView.SetModel(this.model);

    this.gridController = gridController;
    this.cluesController = cluesController;
    this.statusController = statusController;

    this.crosswordService = crosswordService;
    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {
      Trace.WriteLine("gc cid " + ((LoadPuzzleEventArgs) eventArgs).puzzleId);
      this.model.crosswordId = ((LoadPuzzleEventArgs)eventArgs).puzzleId;
    }

  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {

      case Command.Command.EXIT:
        Trace.WriteLine($" exiting crossword id : {model.crosswordId}");
        eventDispatcher.DispatchEvent(new ExitPuzzleEventArgs(model.crosswordId));
        return;

      case Command.Command.SWAP_PANE:
        model.SwapPane();
        Trace.WriteLine("Swapping Pane");
        break;
      default:
        break;

    }

    switch ( model.activePane) {

      case Pane.GRID:
        gridController.ProcessCommandEvent(this,commandEventArgs);
        break;
      case Pane.CLUES:
        cluesController.ProcessCommandEvent(this,commandEventArgs);
        break;
      default:
        break;

    }

  }

}

}
