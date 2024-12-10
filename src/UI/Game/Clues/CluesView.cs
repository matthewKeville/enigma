namespace UI.Game.Clues {

using Terminal.Gui; 

public class CluesView : Window
{
    private CluesSplitView _cluesSplitView;
    private CluesSingleView _cluesSingleView;
    private View _activeView;

    public CluesView(CluesSplitView cluesSplitView,CluesSingleView cluesSingleView) {
        _cluesSingleView = cluesSingleView;
        _cluesSplitView = cluesSplitView;

        // _activeView = _cluesSingleView;
        // Add(_cluesSingleView);
        _activeView = _cluesSplitView;
        Add(_cluesSplitView);
    }

    public void ToggleLayout() {
      if ( _activeView == _cluesSingleView ) {
        Remove(_cluesSingleView);
        Add(_cluesSplitView);
        _activeView = _cluesSplitView;
      } else {
        Remove(_cluesSplitView);
        Add(_cluesSingleView);
        _activeView = _cluesSingleView;
      }
    }
}

}
