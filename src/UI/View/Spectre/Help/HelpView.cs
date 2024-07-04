using Context;
using Spectre.Console;
using UI.Model.Help;

namespace UI.View.Spectre.Help {

  public class HelpView : ISpectreView<Layout> {

    private HelpModel helpModel;
    private ContextAccessor contextAccesor;

    public void SetContext(ApplicationContext context) {
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
