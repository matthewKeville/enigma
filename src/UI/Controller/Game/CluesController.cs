using Context;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Game;

namespace UI.Controller.Game {

public class CluesController : Controller<CluesModel> {

  private ContextAccessor contextAccessor;
  private EventDispatcher eventDispatcher;

  public CluesController(ContextAccessor ctx, EventDispatcher eventDispatcher) {
    this.contextAccessor = ctx;
    this.eventDispatcher = eventDispatcher;
    Register(ctx);
    eventDispatcher.RaiseEvent += ProcessEvent;
  }

  public void PerformAndNotifyCluesWordChange(Action action) {
    var (prevOrdinal,prevDirection) = model.ActiveClue;
    action();
    var (ordinal,direction) = model.ActiveClue;
    if ( prevOrdinal != ordinal || prevDirection != direction ) {
      eventDispatcher.DispatchEvent(new CluesWordChangeEventArgs(ordinal,direction));
    }
  }

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

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(GridWordChangeEventArgs)) {

      Trace.WriteLine(" Recieved Grid Word Change ");
      GridWordChangeEventArgs args = ((GridWordChangeEventArgs) eventArgs);
      this.model.ActiveClue = (args.ordinal,args.direction);

    }

  }

}

}
