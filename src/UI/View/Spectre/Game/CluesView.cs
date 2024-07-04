using System.Diagnostics;
using Context;
using Model;
using Spectre.Console;

namespace UI.View.Spectre.Game {

public class CluesView {

  private int clueCutoff = 40;
  private CluesModel cluesModel;

  public void setContext(ApplicationContext context) {
    this.cluesModel = context.cluesModel;
    Trace.WriteLine("clues context set");
  }

  public Layout Render() {

    if ( this.cluesModel is null ) {
      return new Layout();
    }

    Layout layout = new Layout("Clues");
    Table table = new Table();

    table.AddColumn("Across");
    table.AddColumn("Down");
    table.Width = 200;
    table.Columns[0].Alignment(Justify.Left);
    table.Columns[0].Width = 90;
    table.Columns[1].Alignment(Justify.Left);
    table.Columns[1].Width = 90;

    List<ClueModel> across = new List<ClueModel>(cluesModel.across);
    List<ClueModel> down = new List<ClueModel>(cluesModel.down);

    int alt = 0;
    while ( across.Count() != 0 || down.Count() != 0 ) {
      String aclue = "";
      String dclue = "";
      bool activeAclue = false;
      bool activeDclue = false;
      if (across.Any()) {
        activeAclue = cluesModel.IsActiveClue(across[0]);
        aclue = string.Format("{0}  {1}",
            across[0].ordinal.ToString().PadRight(2),
            across[0].value);
        across.RemoveAt(0);
      }
      if (down.Any()) {
        activeDclue = cluesModel.IsActiveClue(down[0]);
        dclue = string.Format("{0}  {1}",
            down[0].ordinal.ToString().PadRight(2),
            down[0].value);
        down.RemoveAt(0);
      }

      aclue = aclue.Substring(0,Math.Min(aclue.Count(),clueCutoff));
      dclue = dclue.Substring(0,Math.Min(dclue.Count(),clueCutoff));

      if ( activeAclue ) {
        aclue = $"[yellow]{aclue}[/]";
      }
      if ( activeDclue ) {
        dclue = $"[yellow]{dclue}[/]";
      }

      /**
      alt = (alt + 1) % 2;
      if ( alt == 0 ) {
      }
      */

      table.AddRow(
          aclue,dclue
      );
    }

    layout.Update(table);

    return layout;
  }

}

}
