namespace UI.View.Browser
{

    using Event;
    using Terminal.Gui;

    public class BrowserView : Toplevel
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

            _puzzlePickerView.Visible = true;
            _puzzleInstallerView.Visible = false;

            Add(_puzzlePickerView);
            Add(_puzzleInstallerView);

            KeyDown += (sender, args) =>
            {
                //Toggle Active Browser View Window
                if (args.KeyCode == KeyCode.Tab)
                {
                  _puzzleInstallerView.Visible ^= true;
                  _puzzlePickerView.Visible ^= true;
                }
            };

            KeyBindings.Clear();


        }
    }

}
