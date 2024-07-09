using Context;
using Services.CrosswordInstaller;
using UI.Command;
using UI.Model.Browser;

namespace UI.Controller.Browser {

public class InstallerController : Controller<InstallerModel> {

  private ContextAccessor contextAccessor;
  private CrosswordInstallerService crosswordInstallerService;

  public InstallerController(ContextAccessor ctx,CrosswordInstallerService crosswordInstallerService) {
    this.contextAccessor = ctx;
    this.crosswordInstallerService = crosswordInstallerService;
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
      case Command.Command.INSTALL:
        DateOnly requestDate = model.GetActiveDate();
        if ( !model.installationRequests.ContainsKey(requestDate)) {
          InstallationRequest request = new InstallationRequest(){
            Status = Enums.InstallationRequestStatus.WAITING,
            Args = new NYTInstallationRequestArgs() {
              RealDate = requestDate
            }
          };
          crosswordInstallerService.InstallPuzle(request);
          model.installationRequests[requestDate] = request;
        }
        break;
    }
  }

}

}
