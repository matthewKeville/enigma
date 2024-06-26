using System.Drawing;
using System.Diagnostics;
using Spectre.Console;
using UI.View.ViewModel;

namespace UI.View.Spectre {

public class GridView {

  private const char block = '\0';
  private GridViewModel gridViewModel;

  public GridView(GridViewModel gridViewModel) {
    this.gridViewModel = gridViewModel;
  }

  //render character matrix to Table
  public Layout Render() {
    lock(gridViewModel) {

    Table table = new Table();
    Layout layout = new Layout("Grid");

    for ( int j = 0; j < gridViewModel.ColumnCount; j++ ) {
      table.AddColumn(""+j);
    }
    for ( int i = 0; i < gridViewModel.RowCount; i++ ) {
      string[] rowChars = new string[gridViewModel.ColumnCount];
      for ( int x = 0; x < gridViewModel.ColumnCount; x++ ) {
        rowChars[x] =  gridViewModel.charMatrix[i,x].ToString();
      }
      table.AddRow(rowChars);
    }
    table.ShowRowSeparators();
    table.HideHeaders();

    layout.SplitRows(new Layout("Top"),new Layout("Bottom"));

    layout["Top"].Update(table);
    layout["Top"].Size(60);


    for ( int j = 0; j < gridViewModel.ColumnCount; j++ ) {
      for ( int i = 0; i < gridViewModel.RowCount; i++ ) {
        String charDisplay;
        if ( gridViewModel.charMatrix[i,j] == '\0' ) {
          charDisplay = ""+block;
        } else {
          charDisplay = "[bold]"+gridViewModel.charMatrix[i,j]+"[/]";
          if ( gridViewModel.entry.X == j && gridViewModel.entry.Y == i ) {
            charDisplay = "[yellow]"+charDisplay+"[/]";
          }
        }
        table.UpdateCell(i,j,charDisplay);
      }
    }

    Panel testPanel = new Panel(string.Format("x,y : {0},{1}",gridViewModel.entry.X,gridViewModel.entry.Y));
    testPanel.Padding = new Padding(0,0,0,0);
    layout["Bottom"].Update(testPanel);
    layout["Bottom"].Size(3);

    return layout;
  }

  }

}

}
