using Context;
using UI.Command;
using UI.Model.Game;
using UI.View.Spectre.Game;

namespace UI.Controller.Game {

public class CluesController {

  private CluesView cluesView;
  private CluesModel clues;
  private ContextAccessor contextAccessor;

  public CluesController(ContextAccessor contextAccessor, CluesView cluesView) {
    this.contextAccessor = contextAccessor;
    this.clues = contextAccessor.GetContext().cluesModel;
    this.cluesView = cluesView;
    this.cluesView.SetContext(contextAccessor.GetContext().cluesModel);
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.UPDATE_CONTEXT:
        Trace.WriteLine("puzzle swap triggered in game clues controller");
        this.clues = contextAccessor.GetContext().cluesModel;
        cluesView.SetContext(contextAccessor.GetContext().cluesModel);
        break;
    }
  }

}

}
