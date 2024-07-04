using Context;
using Model;
using Spectre.Console;

namespace UI.View.Spectre.Help {

  public class HelpView {

    public HelpModel helpModel;
    private ContextAccessor contextAccesor;

    public void setContext(ApplicationContext context) {
      this.helpModel = context.helpModel;
    }

    public Layout Render() {

      if (this.helpModel is null) {
        return new Layout();
      }

      Layout root = new Layout();
      root.Update(new Panel("helphelphelp"));
      return root;
    }

  }
}
