using System.Diagnostics;
using Context;
using Model;
using UI.Command;
using UI.View.Spectre.Game;

namespace UI.Controller {

public class CluesController {

  private CluesView cluesView;
  private CluesModel clues;
  private ContextAccessor contextAccessor;

  public CluesController(ContextAccessor contextAccessor, CluesView cluesView) {
    this.contextAccessor = contextAccessor;
    this.clues = contextAccessor.getContext().cluesModel;
    this.cluesView = cluesView;
    this.cluesView.setContext(contextAccessor.getContext());
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.UPDATE_CONTEXT:
        Trace.WriteLine("puzzle swap triggered in game clues controller");
        this.clues = contextAccessor.getContext().cluesModel;
        cluesView.setContext(contextAccessor.getContext());
        break;
    }
  }

}

}
