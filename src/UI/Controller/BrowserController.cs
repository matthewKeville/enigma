using System.Diagnostics;
using Context;
using Model;
using UI.Command;
using UI.View.Spectre.Browser;

namespace UI.Controller {

public class BrowserController {

  private BrowserView browserView;
  private BrowserModel browserModel;
  private ContextAccessor contextAccessor;

  public BrowserController(ContextAccessor contextAccessor, BrowserView browserView) {
    this.contextAccessor = contextAccessor;
    this.browserModel = contextAccessor.getContext().browserModel;
    this.browserView = browserView;
    this.browserView.setContext(contextAccessor.getContext());
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
