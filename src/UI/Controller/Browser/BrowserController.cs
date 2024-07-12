using Enums;
using UI.Command;
using UI.Model.Browser;
using UI.View.Spectre;

namespace UI.Controller.Browser {

public class BrowserController : Controller<BrowserModel> {

  private BrowserView browserView;
  private PickerController pickerController;
  private InstallerController installerController;

  public BrowserController(BrowserView browserView,PickerController pickerController,InstallerController installerController) {
    this.model= new BrowserModel();

    this.browserView = browserView;
    this.browserView.SetModel(this.model);

    this.pickerController = pickerController;
    this.installerController = installerController;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      case Command.Command.SWAP_PANE:
        model.SwitchTab();
        break;
      default:
        switch (model.activeTab ) {
          case BrowserTab.PICKER: 
            pickerController.ProcessCommandEvent(this,commandEventArgs);
            break;
          case BrowserTab.INSTALLER: 
            installerController.ProcessCommandEvent(this,commandEventArgs);
            break;
      }
      break;
    }
  }



}

}
