using Entity;
using Enums;
using Services;
using UI.Event;
using UI.Events;
using UI.Model.Game;
using UI.View.Spectre.Game;

namespace UI.Controller.Game {

public class CluesController : Controller<CluesModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private CluesView cluesView;

  public CluesController(EventDispatcher eventDispatcher,CrosswordService crosswordService,CluesView cluesView) {

    this.model = new CluesModel();

    this.cluesView = cluesView;
    this.cluesView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    eventDispatcher.RaiseEvent += ProcessEvent;
    this.crosswordService = crosswordService;
  }

  public void PerformAndNotifyCluesWordChange(Action action) {
    var (prevOrdinal,prevDirection) = model.ActiveClue;
    action();
    var (ordinal,direction) = model.ActiveClue;
    if ( prevOrdinal != ordinal || prevDirection != direction ) {
      eventDispatcher.DispatchEvent(new CluesWordChangeEventArgs(ordinal,direction));
    }
  }

  /**
  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {

      case Command.Command.MOVE_LEFT:
        PerformAndNotifyCluesWordChange( () => { model.ChangeOrientation(true);} );
        break;

      case Command.Command.MOVE_RIGHT:
        PerformAndNotifyCluesWordChange( () => { model.ChangeOrientation(false);} );
        break;


      case Command.Command.MOVE_UP:
        PerformAndNotifyCluesWordChange( () => { model.PrevClue();} );
        break;

      case Command.Command.MOVE_DOWN:
        PerformAndNotifyCluesWordChange( () => { model.NextClue();} );
        break;

      default:
        break;
    }
  }
  */

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
      this.cluesView.SetModel(this.model);
    }

  }

}

}
