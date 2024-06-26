namespace View.ViewModel {

  public enum Window {
    GAME,
    HELP
  }

public class RootViewModel {

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
