using Context;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Game;

namespace UI.Controller.Game {

public class CluesController : Controller<CluesModel> {

  private ContextAccessor contextAccessor;

  public CluesController(ContextAccessor ctx, EventDispatcher eventDispatcher) {
    this.contextAccessor = ctx;
    Register(ctx);
    eventDispatcher.RaiseEvent += ProcessEvent;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      default:
        break;
    }
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {
    if (eventArgs.GetType() == typeof(GridWordChangeEventArgs)) {
      Trace.WriteLine(" Recieved grid word change ");
      GridWordChangeEventArgs args = ((GridWordChangeEventArgs) eventArgs);
      this.model.ActiveClue = (args.ordinal,args.direction);
      Trace.WriteLine($"active clue (i,d) {this.model.ActiveClue.ToString()}");
    }
  }

}

}
