using Context;
using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Model.Browser;

namespace UI.View.Spectre.Browser {

  public class PickerView : SpectreView<PickerModel> {

    public PickerView(ContextAccessor ctx) {
      Register(ctx);
    }

    override protected IRenderable render() {

      FigletText fig = new FigletText("Enigma").Centered();
      Table table = new Table();
      table.AddColumn(new TableColumn("").Centered());
      table.AddRow(fig);
      table.HideHeaders();
      table.NoBorder();

      table.AddEmptyRow();
      table.AddEmptyRow();
      table.AddEmptyRow();
      table.Centered();

      Table headerTable = new Table();
      headerTable.Centered();
      headerTable.HideHeaders();
      headerTable.NoBorder();
      //headerTable.AddColumn("Cursor");
      headerTable.AddColumn(new TableColumn("Cursor"));
      headerTable.AddColumn(new TableColumn("Title").Padding(4,0,4,0));
      headerTable.AddColumn(new TableColumn("Type").Padding(4,0,4,0));
      headerTable.AddColumn(new TableColumn("Status").Padding(4,0,4,0));
      model.headers.ForEach(
        x => { 

          Text cursor;

          if ( x.PuzzleId == model.headers[model.selection].PuzzleId ) {
            cursor = new Text(">", new Style(Color.Green1));  
          } else {
            cursor = new Text(" ");
          }

          Text title = new Text(x.Title);
          Text type = new Text(x.Type.ToString());
          Text status;

          if ( x.Started && !x.Complete) {
            status = new Text(String.Format("{0,-16}",$" In Progress {(int) x.Elapsed.TotalMinutes}"));
          } else if ( x.Complete ) {
            status = new Text(String.Format("{0,-16}",$" Complete {(int) x.Elapsed.TotalMinutes}"));
          } else {
            status = new Text(String.Format("{0,-16}",$" Not Started"));
          }

          headerTable.AddRow(cursor,title,type,status);
        }
      );

      table.AddRow(headerTable);
      return table;
    }

  }
}
