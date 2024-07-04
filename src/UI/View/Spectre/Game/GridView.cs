using Spectre.Console;
using UI.Model.Game;

namespace UI.View.Spectre.Game {

public class GridView : SpectreView<GridModel> {

  private const char block = ' ';

  protected override Layout render() {

    Table table = new Table();
    table.Border(TableBorder.Heavy);
    Layout layout = new Layout("Grid");

    //skeleton
    
    for ( int i = 0; i < model.ColumnCount; i++ ) {
      table.AddColumn(""+i);
    }
    for ( int j = 0; j < model.RowCount; j++ ) {
      string[] rowChars = new string[model.ColumnCount];
      for ( int x = 0; x < model.ColumnCount; x++ ) {
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
  
    for ( int i = 0; i < model.ColumnCount; i++ ) {
      for ( int j = 0; j < model.RowCount; j++ ) {

        String charDisplay;
        // render blocks
        
        if ( model.charMatrix[i,j] == '\0' ) {
          charDisplay = "[bold][invert]"+block+"[/][/]";
        } else {

        // render characters

          // render active characer
          if ( model.entry.X == i && model.entry.Y == j ) {
            // char
            if ( model.charMatrix[i,j] != ' ' ) {
              charDisplay = "[yellow]"+model.charMatrix[i,j]+"[/]";
            } else {
              charDisplay = "[yellow]*[/]";
            }
            // empty
          // render characters in the current word
          } else if (model.InActiveWord(i,j)) {
            charDisplay = "[purple]"+model.charMatrix[i,j]+"[/]";
          } else {
            charDisplay = "[white]"+model.charMatrix[i,j]+"[/]";
          }

        }
        table.UpdateCell(j,i,charDisplay);

      }
    }

    Panel testPanel = new Panel(string.Format("x,y : {0},{1}",model.entry.X,model.entry.Y));
    layout["Bottom"].Size(8);
    layout["Bottom"].Update(testPanel);

    return layout;

  }

}

}
