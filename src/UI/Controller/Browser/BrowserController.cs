using Context;
using Enums;
using UI.Command;
using UI.Model.Browser;

namespace UI.Controller.Browser {

public class BrowserController : Controller<BrowserModel> {

  private ContextAccessor contextAccessor;
  private PickerController pickerController;
  private InstallerController installerController;

  public BrowserController(ContextAccessor ctx,PickerController pickerController,InstallerController installerController) {
    this.contextAccessor = ctx;
    this.pickerController = pickerController;
    this.installerController = installerController;
    Register(ctx);
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
