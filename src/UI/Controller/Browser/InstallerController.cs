using Services.CrosswordInstaller;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Browser;
using UI.View.Spectre.Browser;

namespace UI.Controller.Browser {

public class InstallerController : Controller<InstallerModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordInstallerService crosswordInstallerService;
  private InstallerView  installerView;

  public InstallerController(EventDispatcher eventDispatcher,CrosswordInstallerService crosswordInstallerService,InstallerView installerView) {
    this.model = new InstallerModel();
    
    this.installerView = installerView;
    this.installerView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    this.crosswordInstallerService = crosswordInstallerService;
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

          model.installationRequests[requestDate] = request;

          //Install puzzle is async, so the event might be processed
          //by the picker controller before the db updates...
          //TODO, have the event published after the installation
          //is complete by way of a callback
          crosswordInstallerService.InstallPuzle(request);
          eventDispatcher.DispatchEvent(new PuzzleInstalledEvent());

        }
        break;
    }
  }

}

}
