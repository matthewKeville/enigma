using Spectre.Console;
using UI.View.ViewModel;

namespace UI.View.Spectre {

public class CluesView {

  private int clueCutoff = 40;
  private CluesViewModel cluesViewModel;

  public CluesView(CluesViewModel cluesViewModel) {
    this.cluesViewModel = cluesViewModel;
  }

  public Layout Render() {

    Layout layout = new Layout("Clues");
    Table table = new Table();

    table.AddColumn("Across");
    table.AddColumn("Down");
    table.Width = 200;
    table.Columns[0].Alignment(Justify.Left);
    table.Columns[0].Width = 90;
    table.Columns[1].Alignment(Justify.Left);
    table.Columns[1].Width = 90;

    List<ClueViewModel> across = new List<ClueViewModel>(cluesViewModel.across);
    List<ClueViewModel> down = new List<ClueViewModel>(cluesViewModel.down);

    while ( across.Count() != 0 || down.Count() != 0 ) {
      String aclue = "";
      String dclue = "";
      if (across.Any()) {
        aclue = string.Format("{0}  {1}",
            across[0].ordinal.ToString().PadRight(2),
            across[0].value);
        across.RemoveAt(0);
      }
      if (down.Any()) {
        dclue = string.Format("{0}  {1}",
            down[0].ordinal.ToString().PadRight(2),
            down[0].value);
        down.RemoveAt(0);
      }
      table.AddRow(
          aclue.Substring(0,Math.Min(aclue.Count(),clueCutoff)),
          dclue.Substring(0,Math.Min(dclue.Count(),clueCutoff))
      );
    }

    layout.Update(table);

    return layout;
  }

}

}
