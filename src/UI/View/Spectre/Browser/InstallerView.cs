using Context;
using Services.CrosswordInstaller;
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
      puzzleTable.AddColumn("Status");

      lock(InstallerModel.flag) {

        model.pageDates.ForEach(
          d => { 
            Text cursor;

            if ( d == model.GetActiveDate() ) {
              cursor = new Text(">", new Style(Color.Green1));  
            } else {
              cursor = new Text(" ");
            }

            //show the dates as the puzzle that would be retrieved from the endpoint
            //not the actuall query parameter.
            DateOnly displayDate = d.AddDays(model.DayDisplayDelayDays);
            Text title = new Text($"{displayDate.DayOfWeek.ToString():-10}    {displayDate.ToShortDateString():+20}");

            Text status;
            InstallationRequest? installInfo = model.GetInstallationRequestInfo(d);
            if ( installInfo is null ) {
              status = new Text("");
            } else {
              status = new Text(((InstallationRequest) installInfo).Status.ToString());
            }

            puzzleTable.AddRow(cursor,title,status);
          }
        );

      }

      table.AddRow(puzzleTable);
      return table;

    }

  }
}
