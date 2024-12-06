namespace UI.Game {


// Defines a top-level window with border and title
using Event;
using Terminal.Gui;
    using static Event.StartPuzzleEventArgs;

    public class GameView : Window
{
    private CluesView _cluesView;
    private GridView _gridView;
    private EventBus _eventBus;
    public GameView (CluesView cluesView,GridView gridView,EventBus eventBus)
    {
        _cluesView = cluesView;
        _gridView = gridView;
        _eventBus = eventBus;

        Title = $"Game :  ({Application.QuitKey} to quit)";
        var label = new Label { Text = " welcome to the puzzle " };

        gridView.X = 0;
        gridView.Width = Dim.Percent(50);

        cluesView.X = Pos.Percent(50);
        cluesView.Width = Dim.Percent(50);

        Add(gridView);
        Add(cluesView);

        KeyDown += (sender,args) => {
          if (args.KeyCode == KeyCode.F8) {
              MessageBox.Query(30,5,"System","Ending Puzzle","OK");
              _eventBus.PostEvent(new EndPuzzleEventArgs());
          }
          };

    }
}

}
