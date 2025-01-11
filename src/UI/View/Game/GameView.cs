namespace UI.View.Game
{
    using Event;
    using Terminal.Gui;
    using UI.KeyMaping;
    using UI.KeyMapping;
    using UI.View.Game.Clues;

    public class GameView : Toplevel
    {

        private CluesView _cluesView;
        private GridView _gridView;
        private EventBus _eventBus;
        private KeySequenceInterpreter _insertKeySequenceInterpreter;
        private KeySequenceInterpreter _normalKeySequenceInterpreter;
        private bool _isInsertMode = false;

        public GameView(CluesView cluesView, GridView gridView, EventBus eventBus)
        {

            _gridView = gridView;
            _cluesView = cluesView;
            _eventBus = eventBus;

            _gridView.X = 0;
            _gridView.Visible = true;

            _cluesView.X = Pos.Right(_gridView);
            _cluesView.Width = Dim.Fill();
            _cluesView.Visible = true;

            Add(gridView);
            Add(cluesView);

            _normalKeySequenceInterpreter = new KeySequenceInterpreter(KeyMaps.BuildNormalKeyMaps());
            _insertKeySequenceInterpreter = new KeySequenceInterpreter(KeyMaps.BuildInsertKeyMaps());

        }

        public override bool OnKeyDown(Key key)
        {

            (bool partialMatch, UICommand? command) = _isInsertMode ?
              _insertKeySequenceInterpreter.ProcessKey(key) :
              _normalKeySequenceInterpreter.ProcessKey(key);

            if (command == null) {
              return true;
            }

            _eventBus.PostEvent(command);

            switch ( command.Type ) {

              case UICommandType.ENTER_INSERT_MODE:
                _isInsertMode = true;
                break;
              case UICommandType.ENTER_NORMAL_MODE:
                _isInsertMode = false;
                break;
              case UICommandType.CHANGE_WORD:
                _isInsertMode = true;
                break;
              case UICommandType.CHANGE_INNER_WORD:
                _isInsertMode = true;
                break;

              case UICommandType.EXIT_PUZZLE:
                MessageBox.Query(30, 5, "System", "Ending Puzzle", "OK");
                //todo save puzzle
                Application.Shutdown();
                break;
            }

            return true;

        }

    }



}
