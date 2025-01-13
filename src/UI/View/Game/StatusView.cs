namespace UI.View.Game
{
    using Event;
    using Terminal.Gui;
    using UI.Model;

    public class StatusView : Toplevel
    {
        private EventBus _eventBus;
        private GameModel _gameModel;
        private Label _elapsedLabel;
        private Timer _timer;

        public StatusView(EventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Register(this, (args) =>
            {
                if (args is PuzzleLoadedEventArgs)
                {
                    OnPuzzleLoaded((PuzzleLoadedEventArgs)args);
                }
            });

            SetupViews();

        }

        private void SetupViews() {
          _elapsedLabel = new Label();
          _elapsedLabel.Text="";
          _elapsedLabel.X = 2;
          _elapsedLabel.Y = 2;
          _elapsedLabel.Width = 6;

          Add(_elapsedLabel);
        }

        private void OnPuzzleLoaded(PuzzleLoadedEventArgs args)
        {
            _gameModel = args.GameModel;
            _timer = new Timer( 
              (e) => {
                // Terminal.Gui V2 docs warn against raw dogging UI changes on
                // another thread. If we want to maniupate the UI in the background,
                // wrap the UI actions in Application.Invoke(). See 'Multi-Tasking'
                // https://gui-cs.github.io/Terminal.GuiV2Docs/docs/mainloop.html
                Application.Invoke( () => {
                  TimeSpan et = 
                    (DateTime.UtcNow - _gameModel.SessionStartTime) 
                    + _gameModel.PrevElapsed;
                  _elapsedLabel.Text = $"{et.Hours}:{et.Minutes}:{et.Seconds}";
                  SetNeedsDisplay();
                  Trace.WriteLine($"{et.Hours}:{et.Minutes}:{et.Seconds}");
                });
              },
              null, TimeSpan.Zero, TimeSpan.FromSeconds(1)
            );
        }

    }

}
