namespace UI {

using Event;
using static Event.StartPuzzleEventArgs;
using Terminal.Gui;
using UI.Game;

public class RootView : Window
{
    private BrowserView _browserView;
    private GameView _gameView;
    private EventBus _eventBus;

    public RootView (BrowserView browserView,GameView gameView,EventBus eventBus)
    {

      _browserView = browserView;
      _gameView = gameView;
      _eventBus = eventBus;

      _eventBus.Register(this,(eventArgs) => {
        if ( eventArgs is StartPuzzleEventArgs ) {
          showGame();
        } else if ( eventArgs is EndPuzzleEventArgs ) {
          showBrowser();
        }
      });

      BorderStyle = LineStyle.None;

      showBrowser();

    }

    private void showGame() {
      RemoveAll();
      Add( _gameView );
    }

    private void showBrowser() {
      RemoveAll();
      Add( _browserView );
    }
}

}
