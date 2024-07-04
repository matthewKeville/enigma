using Enums;
using Spectre.Console.Rendering;
using UI.Model;
using UI.View.Spectre.Browser;
using UI.View.Spectre.Game;
using UI.View.Spectre.Help;
namespace UI.View.Spectre;

public class RootView : SpectreView<RootModel> {

  private HelpView helpView;
  private GameView gameView;
  private BrowserView browserView ;

  public RootView(HelpView helpView,GameView gameView,BrowserView browserView) {
    this.gameView = gameView;
    this.helpView = helpView;
    this.browserView = browserView;
  }

  protected override IRenderable render() {
    switch ( model.activeWindow ) {
      case Window.BROWSER:
        return browserView.Render();
      case Window.GAME:
        return gameView.Render();
      case Window.HELP:
        return helpView.Render();
      default:
        Environment.Exit(5);
        return null;
    }
  }

}
