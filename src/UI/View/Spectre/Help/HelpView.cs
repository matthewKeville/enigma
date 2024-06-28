using Context;
using Model;
using Spectre.Console;

namespace UI.View.Spectre.Help {

  public class HelpView {

    public HelpModel help;
    private ContextAccessor contextAccesor;

    public HelpView(ContextAccessor contextAccesor) {
      this.contextAccesor = contextAccesor;
      this.help = contextAccesor.getContext().helpModel;
    }

    public Layout Render() {
      Layout root = new Layout();
      root.Update(new Panel("helphelphelp"));
      return root;
    }

  }
}
