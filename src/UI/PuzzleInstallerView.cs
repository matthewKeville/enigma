namespace UI {

using System.Data;
using Terminal.Gui;

public class PuzzleInstallerView : Window
{

    public PuzzleInstallerView ()
    {

        Title = $"Puzzle Installer :  ({Application.QuitKey} to quit)";
        var label = new Label { Text = " Install a puzzle " };

        var dt = new DataTable();

        dt.Columns.Add("Date");
        dt.Columns.Add("Status");

        dt.Rows.Add(new String [] {"11/8/2024","NOT INSTALLED"});
        dt.Rows.Add(new String [] {"11/9/2024","INSTALLED"});

        var tableView = new TableView() {
          X = 0,
          Y= 0,
          Width = Dim.Fill(),
          Height = 6
        };

        tableView.Table = new DataTableSource(dt);
        tableView.FullRowSelect = true;
        tableView.KeyDown += (sender,args) => {

          if (args.KeyCode == KeyCode.Enter) {
            int r = tableView.SelectedRow;
            String rowString = $"{tableView.Table[r,0]} {tableView.Table[r,1]}";
            int option = MessageBox.Query(30,5,"Install Puzzle ",rowString,"install","cancel");
            if ( option == 0 ) {
              //do the install
            } 
          }

          //note the accessor checks for oob
          if (args.KeyCode == KeyCode.J) {
            tableView.SelectedRow++;
          }

          if (args.KeyCode == KeyCode.K) {
            tableView.SelectedRow--;
          }

        };

        // Add the views to the Window
        Add(tableView);
    }
}

}
