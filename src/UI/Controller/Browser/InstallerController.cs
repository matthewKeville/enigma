using Services.CrosswordInstaller;
using UI.Commands;
using UI.Event;
using UI.Events;
using UI.Model.Browser;
using UI.View.Spectre.Browser;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller.Browser {

public class InstallerController : Controller<InstallerModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordInstallerService crosswordInstallerService;
  private InstallerView  installerView;
  private KeySeqInterpreter keySeqInterpreter;

  public InstallerController(EventDispatcher eventDispatcher,CrosswordInstallerService crosswordInstallerService,InstallerView installerView) {
    this.model = new InstallerModel();
    
    this.installerView = installerView;
    this.installerView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    this.crosswordInstallerService = crosswordInstallerService;

    buildKeySeqInterpreter();
  }

  public void ProcessKeyInput(ConsoleKey key) {

    KeySeqResponse response = keySeqInterpreter.ProcessKey(key);
    
    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } else if ( response.Propagate ) {
      //no child
    }

  }

  public void ProcessCommand(Command command) {
    switch ( command.Type ) {

      case CommandType.MOVE_UP:
        model.MoveUp();
        break;

      case CommandType.MOVE_DOWN:
        model.MoveDown();
        break;

      case CommandType.INSTALL:

        DateOnly requestDate = model.GetActiveDate();

        if ( !model.installationRequests.ContainsKey(requestDate)) {
          InstallationRequest request = new InstallationRequest(){
            Status = Enums.InstallationRequestStatus.WAITING,
            Args = new NYTInstallationRequestArgs() {
              RealDate = requestDate
            },
            OnSuccess = () => { 
              eventDispatcher.DispatchEvent(new PuzzleInstalledEvent());
            }
          };

          model.installationRequests[requestDate] = request;
          crosswordInstallerService.InstallPuzle(request);

        }
        break;
    }
  }

  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();
    commandMap[new List<ConsoleKey>(){ConsoleKey.J}] = new Command(CommandMode.NORMAL,CommandType.MOVE_DOWN);
    commandMap[new List<ConsoleKey>(){ConsoleKey.K}] = new Command(CommandMode.NORMAL,CommandType.MOVE_UP);
    commandMap[new List<ConsoleKey>(){ConsoleKey.N}] = new Command(CommandMode.NORMAL,CommandType.INSTALL);
    keySeqInterpreter = new KeySeqInterpreter(commandMap);
  }

}

}
