using Context;
using Enums;
using Model;
using Spectre.Console;
using UI.View.Spectre.Game;
using UI.View.Spectre.Help;
namespace UI.View.Spectre;

public class RootView {

  private RootModel rootModel;

  private HelpView helpView;
  private GameView gameView;

  public RootView(HelpView helpView,GameView gameView) {
    this.gameView = gameView;
    this.helpView = helpView;
  }

  public void setContext(ApplicationContext context) {
    this.rootModel = context.rootModel;
  }

  public Layout Render() {
    if ( rootModel.activeWindow == Window.GAME ) {
      return gameView.Render();
    } else {
      return helpView.Render();
    }
  }

}
