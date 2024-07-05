using Context;
using Spectre.Console;
using UI.Model.Help;

namespace UI.View.Spectre.Help {

  public class HelpView : SpectreView<HelpModel> {

    public HelpView(ContextAccessor ctx) {
      Register(ctx);
    }

    protected override Layout render() {
      Layout root = new Layout();
      root.Update(new Panel("helphelphelp"));
      return root;
    }

  }
}
