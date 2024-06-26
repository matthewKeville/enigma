using Spectre.Console;

namespace UI.View.Spectre.Help {

  public class HelpView {

    public Layout Render() {
      Layout root = new Layout();
      root.Update(new Panel("helphelphelp"));
      return root;
    }

  }
}
