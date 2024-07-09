using Context;
using UI.Command;
using UI.Model.Browser;

namespace UI.Controller.Browser {

public class InstallerController : Controller<InstallerModel> {

  private ContextAccessor contextAccessor;

  public InstallerController(ContextAccessor ctx) {
    this.contextAccessor = ctx;
    Register(ctx);
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.MOVE_UP:
        model.MoveUp();
        break;
      case Command.Command.MOVE_DOWN:
        model.MoveDown();
        break;
    }
  }

}

}
