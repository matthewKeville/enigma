using UI.Command;
using UI.Model.Help;
using UI.View.Spectre.Help;

namespace UI.Controller.Help {

public class HelpController : Controller<HelpModel> {

  private HelpView helpView;

  public HelpController(HelpView helpView) {
    this.model = new HelpModel();

    this.helpView = helpView;
    this.helpView.SetModel(model);
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      default:
        break;
      }
  }

}

}
