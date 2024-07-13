using Enums;
using UI.Commands;
using UI.Model.Browser;
using UI.View.Spectre;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller.Browser {

public class BrowserController : Controller<BrowserModel> {

  private BrowserView browserView;
  private PickerController pickerController;
  private InstallerController installerController;
  private KeySeqInterpreter keySeqInterpreter;

  public BrowserController(BrowserView browserView,PickerController pickerController,InstallerController installerController) {
    this.model= new BrowserModel();

    this.browserView = browserView;
    this.browserView.SetModel(this.model);

    this.pickerController = pickerController;
    this.installerController = installerController;

    buildKeySeqInterpreter();
  }

  public void ProcessKeyInput(ConsoleKey key) {

    KeySeqResponse response = keySeqInterpreter.ProcessKey(key);
    
    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } else if ( response.Propagate ) {
      PropagateKeys(response.Sequence);
    }

  }

  private void ProcessCommand(Command command) {
    switch ( command.Type ) {
      case CommandType.SWAP_PANE:
        model.SwitchTab();
        break;
    }
  }

  private void PropagateKeys(List<ConsoleKey> keys) {
    foreach ( ConsoleKey key in keys) {
      switch (model.activeTab ) {
        case BrowserTab.PICKER: 
          pickerController.ProcessKeyInput(key);
          break;
        case BrowserTab.INSTALLER: 
          installerController.ProcessKeyInput(key);
          break;
      }
    }
  }

  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();
    commandMap[new List<ConsoleKey>(){ConsoleKey.Tab}] = new Command(CommandMode.NORMAL,CommandType.SWAP_PANE);
    keySeqInterpreter = new KeySeqInterpreter(commandMap);
  }


}

}
