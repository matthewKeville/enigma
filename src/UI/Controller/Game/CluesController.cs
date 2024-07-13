using Entity;
using Enums;
using Services;
using UI.Commands;
using UI.Event;
using UI.Events;
using UI.Model.Game;
using UI.View.Spectre.Game;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller.Game {

public class CluesController : Controller<CluesModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private CluesView cluesView;
  private KeySeqInterpreter keySeqInterpreter;

  public CluesController(EventDispatcher eventDispatcher,CrosswordService crosswordService,CluesView cluesView) {

    this.model = new CluesModel();

    this.cluesView = cluesView;
    this.cluesView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    eventDispatcher.RaiseEvent += ProcessEvent;
    this.crosswordService = crosswordService;

    buildKeySeqInterpreter();
  }

  public void PerformAndNotifyCluesWordChange(Action action) {
    var (prevOrdinal,prevDirection) = model.ActiveClue;
    action();
    var (ordinal,direction) = model.ActiveClue;
    if ( prevOrdinal != ordinal || prevDirection != direction ) {
      eventDispatcher.DispatchEvent(new CluesWordChangeEventArgs(ordinal,direction));
    }
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(GridWordChangeEventArgs)) {
      GridWordChangeEventArgs args = ((GridWordChangeEventArgs) eventArgs);
      this.model.ActiveClue = (args.ordinal,args.direction);
    }

    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {

      LoadPuzzleEventArgs args = ((LoadPuzzleEventArgs) eventArgs);
      Crossword crossword = crosswordService.GetCrossword(args.puzzleId);
      this.model = new CluesModel()
      {
          Across = crossword.Words.Where( w => w.Direction == Direction.Across )
            .OrderBy( w => w.I )
            .Select( w => new ClueModel(w.I,w.Clue))
            .ToList(),
          Down = crossword.Words
            .Where( w => w.Direction == Direction.Down )
            .OrderBy( w => w.I )
            .Select( w => new ClueModel(w.I,w.Clue))
            .ToList(),
          ActiveClue = (0,Direction.Across)
      };
      ClueModel firstClue = this.model.Across.OrderBy( c => c.ordinal).First();
      this.model.ActiveClue = (firstClue.ordinal,Direction.Across);
      this.cluesView.SetModel(this.model);
    }

  }

  public void ProcessKeyInput(ConsoleKey key) {

    KeySeqResponse response = keySeqInterpreter.ProcessKey(key);
    
    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } else if ( response.Propagate ) {
      //no children
    }

  }

  public void ProcessCommand(Command command ) {
    switch ( command.Type ) { 

      case CommandType.MOVE_LEFT:
        PerformAndNotifyCluesWordChange( () => { model.ChangeOrientation(true);} );
        break;

      case CommandType.MOVE_RIGHT:
        PerformAndNotifyCluesWordChange( () => { model.ChangeOrientation(false);} );
        break;


      case CommandType.MOVE_UP:
        PerformAndNotifyCluesWordChange( () => { model.PrevClue();} );
        break;

      case CommandType.MOVE_DOWN:
        PerformAndNotifyCluesWordChange( () => { model.NextClue();} );
        break;

      default:
        break;
    }
  }

  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();

    //normal (movement)
    commandMap[new List<ConsoleKey>(){ConsoleKey.L}] = new Command(CommandMode.NORMAL,CommandType.MOVE_RIGHT);
    commandMap[new List<ConsoleKey>(){ConsoleKey.H}] = new Command(CommandMode.NORMAL,CommandType.MOVE_LEFT);
    commandMap[new List<ConsoleKey>(){ConsoleKey.J}] = new Command(CommandMode.NORMAL,CommandType.MOVE_DOWN);
    commandMap[new List<ConsoleKey>(){ConsoleKey.K}] = new Command(CommandMode.NORMAL,CommandType.MOVE_UP);

    keySeqInterpreter = new KeySeqInterpreter(commandMap);
  }


}

}
