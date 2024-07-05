using Context;
using UI.Command;
using UI.Model.Browser;

namespace UI.Controller.Browser {

public class BrowserController {

  private BrowserModel browserModel;
  private ContextAccessor contextAccessor;

  public BrowserController(ContextAccessor contextAccessor) {
    this.contextAccessor = contextAccessor;
    this.browserModel = (BrowserModel) contextAccessor.GetModel<BrowserModel>();
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.MOVE_UP:
        browserModel.selection = Math.Max(0,browserModel.selection-1);
        break;
      case Command.Command.MOVE_DOWN:
        browserModel.selection = Math.Min(browserModel.selection+1,browserModel.headers.Count()-1);
        break;
      case Command.Command.CONFIRM:
        Trace.WriteLine($"loading puzzle {browserModel.headers[browserModel.selection].puzzleId.ToString()}");
        break;
    }
  }

}

}
