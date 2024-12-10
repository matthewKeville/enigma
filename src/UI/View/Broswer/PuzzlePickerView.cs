namespace UI.View.Browser
{

    using System.Data;
    using Event;
    using Services;
    using Terminal.Gui;

    public class PuzzlePickerView : Window
    {

        private CrosswordService _crosswordService;
        private EventBus _eventBus;

        public PuzzlePickerView(CrosswordService crosswordService, EventBus eventBus)
        {
            _crosswordService = crosswordService;
            _eventBus = eventBus;

            Title = $"Puzzle Picker :  ({Application.QuitKey} to quit)";
            var label = new Label { Text = " Select a Puzzle " };

            var dt = new DataTable();

            dt.Columns.Add("Type");
            dt.Columns.Add("Title");
            dt.Columns.Add("Complete");
            dt.Columns.Add("Elapsed");

            List<CrosswordHeader> headers = crosswordService.GetCrosswordHeaders();
            headers.ForEach(ch =>
            {
                dt.Rows.Add(new String[] { $"{ch.Type}", $"{ch.Title}", $"{ch.Complete}", $"{ch.Elapsed}" });
            });

            var tableView = new TableView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            tableView.Table = new DataTableSource(dt);
            tableView.FullRowSelect = true;
            tableView.KeyDown += (sender, args) =>
            {

                if (args.KeyCode == KeyCode.Enter)
                {
                    int r = tableView.SelectedRow;
                    String rowString = $"{tableView.Table[r, 0]} {tableView.Table[r, 1]}";
                    int option = MessageBox.Query(30, 5, "Start Puzzle ?", rowString, "ok", "cancel");
                    if (option == 0)
                    {
                        int puzzleId = headers[r].PuzzleId;
                        _eventBus.PostEvent(new StartPuzzleEventArgs(puzzleId));
                    }
                }

                //note the accessor checks for oob
                if (args.KeyCode == KeyCode.J)
                {
                    tableView.SelectedRow++;
                }

                if (args.KeyCode == KeyCode.K)
                {
                    tableView.SelectedRow--;
                }

            };


            // Add the views to the Window
            Add(tableView);
        }
    }

}
