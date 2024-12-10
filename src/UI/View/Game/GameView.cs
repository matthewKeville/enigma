namespace UI.View.Game
{


    using Event;
    using Terminal.Gui;
    using UI.View.Game.Clues;

    public class GameView : Window
    {

        private CluesView _cluesView;
        private GridView _gridView;
        private EventBus _eventBus;

        private View _focusedView;

        public GameView(CluesView cluesView, GridView gridView, EventBus eventBus)
        {

            _gridView = gridView;
            _cluesView = cluesView;
            _eventBus = eventBus;

            _gridView.X = 0;
            _gridView.Width = Dim.Percent(50);

            _cluesView.X = Pos.Percent(50);
            _cluesView.Width = Dim.Percent(50);
            _cluesView.CanFocus = false;

            /**
            _cluesSingleView = cluesSingleView;
            _cluesSplitView = cluesSplitView;

            _cluesSingleView.X = Pos.Percent(50);
            _cluesSingleView.Width = Dim.Percent(50);
            _cluesSingleView.CanFocus = false;

            _cluesSplitView.X = Pos.Percent(50);
            _cluesSplitView.Width = Dim.Percent(50);
            _cluesSplitView.CanFocus = false;
            _cluesView = _cluesSingleView;
            Add(_cluesView);
            */


            Add(gridView);
            Add(cluesView);

            KeyDown += (sender, args) =>
            {
                if (args.KeyCode == KeyCode.F8)
                {
                    MessageBox.Query(30, 5, "System", "Ending Puzzle", "OK");
                    _eventBus.PostEvent(new EndPuzzleEventArgs());
                }
                if (args.KeyCode == KeyCode.Tab)
                {
                    cluesView.ToggleLayout();
                }
            };

        }
    }

}
