namespace UI {

using Event;
using Terminal.Gui;

public class BrowserView : Window
{
    private bool _picker = true;
    private PuzzlePickerView _puzzlePickerView;
    private PuzzleInstallerView _puzzleInstallerView;

    public BrowserView(PuzzlePickerView puzzlePickerView, 
        PuzzleInstallerView puzzleInstallerView,
        EventBus eventBus)
    {
      _puzzlePickerView = puzzlePickerView;
      _puzzleInstallerView = puzzleInstallerView;

      Add(_puzzlePickerView);

      KeyDown += (sender,args) => {
        if (args.KeyCode == KeyCode.Tab) {
          _picker=!_picker;
          RemoveAll();
          Add( _picker ? _puzzlePickerView : _puzzleInstallerView );
        }
        };

      KeyBindings.Clear();


    }
}

}
