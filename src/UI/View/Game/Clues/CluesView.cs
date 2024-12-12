namespace UI.View.Game.Clues
{

    using Terminal.Gui;

    public class CluesView : Toplevel
    {
        private CluesSplitView _cluesSplitView;
        private CluesSingleView _cluesSingleView;
        private View _activeView;

        public CluesView(CluesSplitView cluesSplitView, CluesSingleView cluesSingleView)
        {
            _cluesSingleView = cluesSingleView;
            _cluesSplitView = cluesSplitView;

            _cluesSingleView.Visible = true;
            _cluesSplitView.Visible = false;
            _activeView = _cluesSingleView;

            Add(_cluesSingleView);
            Add(_cluesSplitView);
        }

        public void ToggleLayout()
        {
            if (_activeView == _cluesSingleView)
            {
                _activeView = _cluesSplitView;
                _cluesSplitView.Visible = true;
                _cluesSingleView.Visible = false;
            }
            else
            {
                _activeView = _cluesSingleView;
                _cluesSingleView.Visible = true;
                _cluesSplitView.Visible = false;
            }
        }
    }

}
