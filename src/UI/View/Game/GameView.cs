namespace UI.View.Game
{


    using Event;
    using Terminal.Gui;
    using UI.View.Game.Clues;

    public class GameView : Toplevel
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
            //_gridView.Width = Dim.Percent(40);
            _gridView.Visible = true;

            _cluesView.X = Pos.Right(_gridView);
            //_cluesView.Width = Dim.Percent(60);
            _cluesView.Width = Dim.Fill();
            _cluesView.Visible = true;

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
