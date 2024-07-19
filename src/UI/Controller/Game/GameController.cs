using Enums;
using Services;
using UI.Commands;
using UI.Controller.Game.Status;
using UI.Event;
using UI.Events;
using UI.Model.Game;
using UI.View.Spectre.Game;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller.Game {

public class GameController : Controller<GameModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private GameView gameView;
  private GridController gridController;
  private CluesController cluesController;
  private StatusController statusController;
  private KeySeqInterpreter keySeqInterpreter;

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

    buildKeySeqInterpreter();
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {
      Trace.WriteLine("gc cid " + ((LoadPuzzleEventArgs) eventArgs).puzzleId);
      this.model.crosswordId = ((LoadPuzzleEventArgs)eventArgs).puzzleId;
    }

  }

  public void ProcessKeyInput(ConsoleKey key) {

    KeySeqResponse response = keySeqInterpreter.ProcessKey(key);
    
    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } else if ( response.Propagate ) {
      PropagateKeys(response.Sequence);
    }

  }

  public void ProcessCommand(Command command) {

    switch ( command.Type ) {

      case CommandType.EXIT:
        Trace.WriteLine($" exiting crossword id : {model.crosswordId}");
        eventDispatcher.DispatchEvent(new ExitPuzzleEventArgs(model.crosswordId));
        return;

      case CommandType.SWAP_PANE:
        model.SwapPane();
        Trace.WriteLine("Swapping Pane");
        break;

    }

  }

  private void PropagateKeys(List<ConsoleKey> keys) {
    foreach ( ConsoleKey key in keys) {
      switch ( model.activePane) {
        case Pane.GRID:
          gridController.ProcessKeyInput(key);
          break;
        case Pane.CLUES:
          cluesController.ProcessKeyInput(key);
          break;
        }
    }
  }

  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> normalCommandMap = new Dictionary<List<ConsoleKey>,Command>();
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.Tab}] = new Command(CommandMode.NORMAL,CommandType.SWAP_PANE);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.Q}] = new Command(CommandMode.NORMAL,CommandType.EXIT);
    keySeqInterpreter = new KeySeqInterpreter(normalCommandMap);
  }

}

}
