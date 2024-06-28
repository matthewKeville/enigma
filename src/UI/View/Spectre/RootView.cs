using Context;
using Enums;
using Model;
using Spectre.Console;
using UI.View.Spectre.Game;
using UI.View.Spectre.Help;
namespace UI.View.Spectre;

public class RootView {

  private RootModel root;

  private HelpView helpView;
  private GameView gameView;
  private ContextAccessor contextAccessor;

  //public RootView(RootModel root, GameView gameView, HelpView helpView) {
  public RootView(ContextAccessor contextAccessor,HelpView helpView,GameView gameView) {
    this.contextAccessor = contextAccessor;
    this.root = contextAccessor.getContext().rootModel;
    this.gameView = gameView;
    this.helpView = helpView;
  }

  public void update(IModel model) {
    this.root = (RootModel) model;
  }

  public Layout Render() {
    if ( root.activeWindow == Window.GAME ) {
      return gameView.Render();
    } else {
      return helpView.Render();
    }
  }

}
