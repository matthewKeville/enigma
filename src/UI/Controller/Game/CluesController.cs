using Context;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Game;

namespace UI.Controller.Game {

public class CluesController {

  private CluesModel cluesModel;
  private ContextAccessor contextAccessor;

  public CluesController(ContextAccessor contextAccessor, EventDispatcher eventDispatcher) {
    this.contextAccessor = contextAccessor;
    this.cluesModel = (CluesModel) contextAccessor.GetModel<CluesModel>();
    eventDispatcher.RaiseEvent += ProcessEvent;
    contextAccessor.RaiseContextChangeEvent += (Object? sender,EventArgs args) => { 
      this.cluesModel = (CluesModel) contextAccessor.GetModel<CluesModel>();
    };
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
      this.cluesModel.ActiveClue = (args.ordinal,args.direction);
      Trace.WriteLine($"active clue (i,d) {this.cluesModel.ActiveClue.ToString()}");
    }
  }

}

}
