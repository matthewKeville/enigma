using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Commands;
using UI.Model.Game;

namespace UI.View.Spectre.Game {

public class GridView : SpectreView<GridModel> {

  // WARN : Unicode character , need to enable a set to opt
  // into unicode rendering
  private const char block = '█';
  private const char cursor = '*';
  private const char line = '.';

  private Table buildDebugTable() {
    Table DebugTable = new Table();
    DebugTable.HideHeaders();
    DebugTable.NoBorder();
    DebugTable.AddColumn("Coordinate");
    DebugTable.AddColumn("Orientation");
    DebugTable.AddColumn("Checks");
    DebugTable.AddColumn("Mode");
    DebugTable.AddColumn("FPS");
    DebugTable.AddRow(
        new Panel(string.Format("x,y : {0},{1}",model.Entry.X,model.Entry.Y)),
        new Panel(string.Format("{0}",model.Orientation)),
        new Panel(string.Format("Checks {0}",model.WordCheckCount)),
        new Panel(string.Format("Mode {0}",KeySeqInterpreter.InterpretMode == CommandMode.INSERT ? "Insert" : "Normal")),
        new Panel(string.Format("FPS {0}",SpectreRenderer.Fps))
    );
    return DebugTable;
  }

  private Table buildGridTable() {

    // Table Frame
    
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

    // Render Characters
  
    for ( int i = 0; i < model.ColumnCount; i++ ) {
      for ( int j = 0; j < model.RowCount; j++ ) {

        String charDisplay;
        
        if ( model.CharMatrix[i,j] == '\0' ) {
          charDisplay = WrapBlock(block);
        } else {
        
          if ( model.Entry.X == i && model.Entry.Y == j ) {
            charDisplay = WrapEntry(model.CharMatrix[i,j]);
          } else if (model.InActiveWord(i,j)) {

            if ( model.StatusMatrix[i,j] != 0) {
              charDisplay = WrapCharStatus(model.CharMatrix[i,j],i,j);
            } else if ( model.CharMatrix[i,j] != ' ' ) {
              charDisplay = WrapActive(model.CharMatrix[i,j]);
            } else {
              charDisplay = WrapActiveEmpty(line);
            }
          
          } else {
            charDisplay = WrapCharStatus(model.CharMatrix[i,j],i,j);
          }

        }

        table.UpdateCell(j,i,charDisplay);

      }
    }

    return table;
  }

  private String WrapCharStatus(Char c,int x,int y) {
    switch ( model.StatusMatrix[x,y] ) {
      case 1:
        return WrapWrong(c);
      case 2:
        return WrapVerified(c);
      default:
        return WrapUnkown(c);
    }
  }

  private String WrapBlock(char c) {
    return "[bold][black]"+c+"[/][/]";
  }

  private String WrapEntry(Char c) {
    return c != ' ' ? "[yellow]"+c+"[/]" : "[yellow]"+cursor+"[/]";
  }

  private String WrapActive(Char c) {
    return "[purple]"+c+"[/]";
  }

  private String WrapActiveEmpty(Char c) {
    return "[cyan]"+c+"[/]";
  }

  private String WrapWrong(Char c) {
    return "[red]"+c+"[/]";
  }

  private String WrapVerified(Char c) {
    return "[green]"+c+"[/]";
  }

  private String WrapUnkown(Char c) {
    return "[white]"+c+"[/]";
  }

  protected override IRenderable render() {


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
