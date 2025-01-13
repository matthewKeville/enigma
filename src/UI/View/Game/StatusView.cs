namespace UI.View.Game
{
    using Event;
    using Terminal.Gui;
    using UI.Model;

    public class StatusView : Toplevel
    {
        private EventBus _eventBus;
        private GameModel _gameModel;
        private Label _titleLabel;
        private Label _elapsedLabel;
        private Label _charCheckCountLabel;
        private Label _wordCheckCountLabel;
        private Label _puzzleCheckCountLabel;
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
                if (args is CheckChangeEventArgs)
                {
                    OnCheckChange((CheckChangeEventArgs)args);
                }
            });

            SetupViews();

        }

        private void SetupViews() {

          _titleLabel = new Label();
          _titleLabel.X = 2;
          _titleLabel.Y = 0;
          // _titleLabel.Width = 6;
          _titleLabel.Width = Dim.Fill();

          _elapsedLabel = new Label();
          _elapsedLabel.X = 2;
          _elapsedLabel.Y = 2;
          _elapsedLabel.Width = Dim.Fill();
          // _elapsedLabel.Width = 6;

          _charCheckCountLabel = new Label();
          _charCheckCountLabel.X = 2;
          _charCheckCountLabel.Y = 4;
          _charCheckCountLabel.Width = Dim.Fill();
          // _charCheckCountLabel.Width = 6;

          _wordCheckCountLabel = new Label();
          _wordCheckCountLabel.X = 2;
          _wordCheckCountLabel.Y = 6;
          _wordCheckCountLabel.Width = Dim.Fill();
          // _wordCheckCountLabel.Width = 6;

          _puzzleCheckCountLabel = new Label();
          _puzzleCheckCountLabel.X = 2;
          _puzzleCheckCountLabel.Y = 8;
          _puzzleCheckCountLabel.Width = Dim.Fill();
          // _puzzleCheckCountLabel.Width = 6;


          Add(_titleLabel);
          Add(_elapsedLabel);
          Add(_charCheckCountLabel);
          Add(_wordCheckCountLabel);
          Add(_puzzleCheckCountLabel);
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
                  if (et.Hours > 0) {
                    _elapsedLabel.Text = $"{et.Hours}h {et.Minutes}m {et.Seconds}s";
                  }  else if (et.Minutes > 0 ) {
                    _elapsedLabel.Text = $"{et.Minutes}m {et.Seconds}s";
                  } else {
                    _elapsedLabel.Text = $"{et.Seconds}s";
                  }
                  SetNeedsDisplay();
                });
              },
              null, TimeSpan.Zero, TimeSpan.FromSeconds(1)
            );
            _titleLabel.Text = _gameModel.Title;
            _charCheckCountLabel.Text = $"⊠   {_gameModel.CharacterCheckCount}";
            _wordCheckCountLabel.Text = $"   {_gameModel.WordCheckCount}";
            _puzzleCheckCountLabel.Text = $"   {_gameModel.PuzzleCheckCount}";
        }

        private void OnCheckChange(CheckChangeEventArgs args)
        {
          _charCheckCountLabel.Text = $"⊠   {_gameModel.CharacterCheckCount}";
          _wordCheckCountLabel.Text = $"   {_gameModel.WordCheckCount}";
          _puzzleCheckCountLabel.Text = $"   {_gameModel.PuzzleCheckCount}";
        }


    }

}
