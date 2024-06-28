using Enums;

namespace Model {

public class RootModel : IModel {

  public Window activeWindow = Window.GAME;

  public void SwitchWindow() {
    if (activeWindow == Window.GAME) {
      activeWindow = Window.HELP;
    } else {
      activeWindow = Window.GAME;
    }
  }

}

}
