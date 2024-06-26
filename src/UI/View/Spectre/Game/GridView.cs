using Spectre.Console;
using UI.View.ViewModel;

namespace UI.View.Spectre.Game {

public class GridView {

  private const char block = ' ';
  private GridViewModel gridViewModel;

  public GridView(GridViewModel gridViewModel) {
    this.gridViewModel = gridViewModel;
  }

  //render character matrix to Table
  public Layout Render() {

    lock(gridViewModel) {

    Table table = new Table();
    table.Border(TableBorder.Heavy);
    Layout layout = new Layout("Grid");

    //skeleton
    
    for ( int i = 0; i < gridViewModel.ColumnCount; i++ ) {
      table.AddColumn(""+i);
    }
    for ( int j = 0; j < gridViewModel.RowCount; j++ ) {
      string[] rowChars = new string[gridViewModel.ColumnCount];
      for ( int x = 0; x < gridViewModel.ColumnCount; x++ ) {
        rowChars[x] =  " ";
      }
      table.AddRow(rowChars);
    }
    table.ShowRowSeparators();
    table.HideHeaders();

    layout.SplitRows(new Layout("Top"),new Layout("Bottom"));
    layout["Top"].Update(table);
    layout["Top"].Size(40);

    //render view model
   
    for ( int i = 0; i < gridViewModel.ColumnCount; i++ ) {
      for ( int j = 0; j < gridViewModel.RowCount; j++ ) {

        String charDisplay;
        // render blocks
        
        if ( gridViewModel.charMatrix[i,j] == '\0' ) {
          charDisplay = "[bold][invert]"+block+"[/][/]";
        } else {

        // render characters

          // render active characer
          if ( gridViewModel.entry.X == i && gridViewModel.entry.Y == j ) {
            charDisplay = "[yellow]"+gridViewModel.charMatrix[i,j]+"[/]";
          // render characters in the current word
          } else if (gridViewModel.InActiveWord(i,j)) {
            charDisplay = "[purple]"+gridViewModel.charMatrix[i,j]+"[/]";
          } else {
            charDisplay = "[white]"+gridViewModel.charMatrix[i,j]+"[/]";
          }

        }
        table.UpdateCell(j,i,charDisplay);

      }
    }

    Panel testPanel = new Panel(string.Format("x,y : {0},{1}",gridViewModel.entry.X,gridViewModel.entry.Y));
    //testPanel.Padding = new Padding(0,0,0,0);
    layout["Bottom"].Size(8);
    layout["Bottom"].Update(testPanel);

    return layout;
  }

  }

}

}
