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
        
        if ( model.CharMatrix[i,j] == '\0' ) {
          charDisplay = "[bold][invert]"+block+"[/][/]";
        } else {


        // render active characer
        if ( model.Entry.X == i && model.Entry.Y == j ) {
          // char
          if ( model.CharMatrix[i,j] != ' ' ) {
            charDisplay = "[yellow]"+model.CharMatrix[i,j]+"[/]";
          } else {
            charDisplay = "[yellow]*[/]";
          }
    
 
        // render characters in the current word
        } else if (model.InActiveWord(i,j)) {
          charDisplay = "[purple]"+model.CharMatrix[i,j]+"[/]";

        // render non-word characteres
        } else {
          charDisplay = "[white]"+model.CharMatrix[i,j]+"[/]";
        }

        }
        table.UpdateCell(j,i,charDisplay);

      }
    }

    Layout DebugInfo = new Layout();
    DebugInfo.SplitRows(new Layout("Coordinate"),new Layout("Orientation"));
    DebugInfo["Coordinate"].Update(new Panel(string.Format("x,y : {0},{1}",model.Entry.X,model.Entry.Y)));
    DebugInfo["Orientation"].Update(new Panel(string.Format("{0}",model.Orientation)));
    layout["Bottom"].Update(DebugInfo);
    layout["Bottom"].Size(12);

    return layout;

  }

}

}
