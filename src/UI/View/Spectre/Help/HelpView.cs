using Spectre.Console;
using UI.Model.Help;

namespace UI.View.Spectre.Help {

  public class HelpView : SpectreView<HelpModel> {

    protected override Layout render() {
      Layout root = new Layout();
      root.Update(new Panel("helphelphelp"));
      return root;
    }

  }
}
