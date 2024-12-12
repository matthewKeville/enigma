namespace UI.View
{

    using Event;
    using Terminal.Gui;
    using UI.View.Browser;
    using UI.View.Game;

    public class RootView : Toplevel
    {
        private BrowserView _browserView;
        private GameView _gameView;
        private EventBus _eventBus;

        public RootView(BrowserView browserView, GameView gameView, EventBus eventBus)
        {
            _browserView = browserView;
            _gameView = gameView;

            _browserView.Visible = true;
            _gameView.Visible = false;

            _eventBus = eventBus;

            _eventBus.Register(this, (eventArgs) =>
            {
                if (eventArgs is StartPuzzleEventArgs)
                {
                  _browserView.Visible = false;
                  _gameView.Visible = true;
                  _gameView.SetFocus();
                }
                else if (eventArgs is EndPuzzleEventArgs)
                {
                  _browserView.Visible = true;
                  _gameView.Visible = false;
                  _browserView.SetFocus();
                }
            });

            //BorderStyle = LineStyle.None;
            // BorderStyle

            Shortcut shortcut = new Shortcut(Key.Delete,"Testing",() => {},"Nah");
            MenuBarv2 mb = new MenuBarv2(new [] {shortcut}) {
              Visible =  true,
              Width = 20,
              Y = 60,
            };

            Add(mb);
            Add(_browserView);
            Add(_gameView);
            
            //Focusing the _browserView hides the MenuBarV2 ?
            _browserView.SetFocus();


        }

    }

}
