using Context;
using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Model.Game;

namespace UI.View.Spectre.Game {

public class GridView : SpectreView<GridModel> {

  private const char block = ' ';

  public GridView(ContextAccessor ctx) {
    Register(ctx);
  }

  private Table buildDebugTable() {
    Table DebugTable = new Table();
    DebugTable.HideHeaders();
    DebugTable.NoBorder();
    DebugTable.AddColumn("Coordinate");
    DebugTable.AddColumn("Orientation");
    DebugTable.AddRow(
        new Panel(string.Format("x,y : {0},{1}",model.Entry.X,model.Entry.Y)),
        new Panel(string.Format("{0}",model.Orientation))
    );
    return DebugTable;
  }

  private Table buildGridTable() {
    //skeleton
    Table table = new Table();
    table.Border(TableBorder.Heavy);
    table.ShowRowSeparators();
    table.HideHeaders();
    
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

    return table;
  }

  protected override /**Layout*/IRenderable render() {


    Table debugTable = buildDebugTable();
    Table gridTable = buildGridTable();

    Table container = new Table();
    container.Centered();
    container.AddColumn(new TableColumn(""));
    container.AddRow(gridTable);
    container.AddRow(debugTable);
    container.HideHeaders();
    container.NoBorder();
    return container;

  }

}

}
