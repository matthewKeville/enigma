using Context;
using Model;
using Spectre.Console;

namespace UI.View.Spectre.Game {

public class GridView {

  private const char block = ' ';
  private GridModel grid;

  public void setContext(ApplicationContext context) {
    this.grid = context.gridModel;
  }

  //render character matrix to Table
  public Layout Render() {

    if ( this.grid is null ) {
      return new Layout();
    }

    Table table = new Table();
    table.Border(TableBorder.Heavy);
    Layout layout = new Layout("Grid");

    //skeleton
    
    for ( int i = 0; i < grid.ColumnCount; i++ ) {
      table.AddColumn(""+i);
    }
    for ( int j = 0; j < grid.RowCount; j++ ) {
      string[] rowChars = new string[grid.ColumnCount];
      for ( int x = 0; x < grid.ColumnCount; x++ ) {
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
  
    for ( int i = 0; i < grid.ColumnCount; i++ ) {
      for ( int j = 0; j < grid.RowCount; j++ ) {

        String charDisplay;
        // render blocks
        
        if ( grid.charMatrix[i,j] == '\0' ) {
          charDisplay = "[bold][invert]"+block+"[/][/]";
        } else {

        // render characters

          // render active characer
          if ( grid.entry.X == i && grid.entry.Y == j ) {
            // char
            if ( grid.charMatrix[i,j] != ' ' ) {
              charDisplay = "[yellow]"+grid.charMatrix[i,j]+"[/]";
            } else {
              charDisplay = "[yellow]*[/]";
            }
            // empty
          // render characters in the current word
          } else if (grid.InActiveWord(i,j)) {
            charDisplay = "[purple]"+grid.charMatrix[i,j]+"[/]";
          } else {
            charDisplay = "[white]"+grid.charMatrix[i,j]+"[/]";
          }

        }
        table.UpdateCell(j,i,charDisplay);

      }
    }

    Panel testPanel = new Panel(string.Format("x,y : {0},{1}",grid.entry.X,grid.entry.Y));
    layout["Bottom"].Size(8);
    layout["Bottom"].Update(testPanel);

    return layout;

  }

}

}
