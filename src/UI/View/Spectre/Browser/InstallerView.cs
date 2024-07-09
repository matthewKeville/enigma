using Context;
using Spectre.Console;
using Spectre.Console.Rendering;
using UI.Model.Browser;

namespace UI.View.Spectre.Browser {

  public class InstallerView : SpectreView<InstallerModel> {

    public InstallerView(ContextAccessor ctx) {
      Register(ctx);
    }

    override protected IRenderable render() {


      FigletText fig = new FigletText("NYT Installer").Centered();
      Table table = new Table();
      table.AddColumn(new TableColumn("").Centered());
      table.AddRow(fig);
      table.HideHeaders();
      table.NoBorder();

      table.AddEmptyRow();
      table.AddEmptyRow();
      table.AddEmptyRow();
      table.Centered();

      Table puzzleTable = new Table();
      puzzleTable.Centered();
      puzzleTable.HideHeaders();
      puzzleTable.NoBorder();
      puzzleTable.AddColumn("Cursor");
      puzzleTable.AddColumn("Date");

      lock(InstallerModel.flag) {

        model.pageDates.ForEach(
          d => { 
            Text cursor;

            if ( d == model.GetActiveDate() ) {
              cursor = new Text(">", new Style(Color.Green1));  
            } else {
              cursor = new Text(" ");
            }

            //Text title = new Text(String.Format("{0:-10}  ~  {1:-10}",d.DayOfWeek.ToString(),d.ToShortDateString()));
            Text title = new Text($"{d.DayOfWeek.ToString():-10}    {d.ToShortDateString():+20}");

            puzzleTable.AddRow(cursor,title);
          }
        );

      }

      table.AddRow(puzzleTable);
      return table;

    }

  }
}
