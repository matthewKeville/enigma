namespace UI.View.Game.Clues
{
    using Event;
    using Terminal.Gui;
    using UI.KeyMapping;

    public class CluesView : Toplevel
    {
        private EventBus _eventBus;
        private CluesSplitView _cluesSplitView;
        private CluesSingleView _cluesSingleView;
        private View _activeView;

        public CluesView(CluesSplitView cluesSplitView, CluesSingleView cluesSingleView,EventBus eventBus)
        {
            _cluesSingleView = cluesSingleView;
            _cluesSplitView = cluesSplitView;

            _cluesSingleView.Visible = true;
            _cluesSplitView.Visible = false;
            _activeView = _cluesSingleView;

            Add(_cluesSingleView);
            Add(_cluesSplitView);

            _eventBus = eventBus;
            _eventBus.Register(this, (args) =>
            {
                if (args is UICommand && ((UICommand) args).Type == UICommandType.TOGGLE_CLUES_VIEW)
                {
                    ToggleLayout();
                }
            });

        }

        private void ToggleLayout()
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
