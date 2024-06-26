using Spectre.Console;
using UI.View.Spectre.Game;
using UI.View.Spectre.Help;
using View.ViewModel;
namespace UI.View.Spectre;

public class RootView {

  private GameView gameView;
  private HelpView helpView;
  private RootViewModel rootViewModel;

  public RootView(RootViewModel rootViewModel, GameView gameView, HelpView helpView) {
    this.rootViewModel = rootViewModel;
    this.gameView = gameView;
    this.helpView = helpView;
  }

    public Layout Render() {
      if ( rootViewModel.activeWindow == Window.GAME ) {
        return gameView.Render();
      } else {
        return helpView.Render();
      }
    }

}
