using Enums;

namespace UI.Model {

public class RootModel : IModel {

  public Window activeWindow = Window.BROWSER;
  public Window lastWindow = Window.BROWSER;
  private bool showHelp = false;

  public void ToggleHelp() {
    if ( !showHelp ) {
      showHelp = true;
      lastWindow = activeWindow;
      activeWindow = Window.HELP;
    } else {
      showHelp = false;
      activeWindow = lastWindow;
    }
  }

}

}
