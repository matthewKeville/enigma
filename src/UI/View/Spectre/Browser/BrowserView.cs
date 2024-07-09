using Context;
using Enums;
using Spectre.Console.Rendering;
using UI.Model.Browser;
using UI.View.Spectre.Browser;
namespace UI.View.Spectre;

public class BrowserView : SpectreView<BrowserModel> {

  private PickerView pickerView;
  private InstallerView installerView;

  public BrowserView(ContextAccessor ctx, PickerView pickerView, InstallerView installerView) {
    Register(ctx);
    this.pickerView = pickerView;
    this.installerView = installerView;
  }

  protected override IRenderable render() {
    switch ( model.activeTab ) {
      case BrowserTab.PICKER:
        return pickerView.Render();
      case BrowserTab.INSTALLER:
        return installerView.Render();
      default:
        Environment.Exit(5);
        return null;
    }
  }

}
