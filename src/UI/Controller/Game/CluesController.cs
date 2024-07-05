using Context;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Game;
using UI.View.Spectre.Game;

namespace UI.Controller.Game {

public class CluesController {

  private CluesView cluesView;
  private CluesModel cluesModel;
  private ContextAccessor contextAccessor;

  public CluesController(ContextAccessor contextAccessor, CluesView cluesView,EventDispatcher eventDispatcher) {
    this.contextAccessor = contextAccessor;
    this.cluesModel = contextAccessor.GetContext().cluesModel;
    this.cluesView = cluesView;
    this.cluesView.SetContext(contextAccessor.GetContext().cluesModel);
    eventDispatcher.RaiseEvent += ProcessEvent;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.UPDATE_CONTEXT:
        Trace.WriteLine("puzzle swap triggered in game clues controller");
        this.cluesModel = contextAccessor.GetContext().cluesModel;
        cluesView.SetContext(contextAccessor.GetContext().cluesModel);
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
