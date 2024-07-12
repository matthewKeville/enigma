using Enums;
using Spectre.Console;
using UI.Model.Game;

namespace UI.View.Spectre.Game {

public class CluesView : SpectreView<CluesModel> {

  private int clueCutoff = 40;
  private int columnWidth = 90;
  private int tableWidth = 200;
  private bool nowrap = true;

  protected override Table render() {

    Table table = new Table();

    table.AddColumn("Across");
    table.AddColumn("Down");
    table.Width = tableWidth;
    table.Columns[0].Alignment(Justify.Left);
    table.Columns[0].Width = columnWidth;
    table.Columns[1].Alignment(Justify.Left);
    table.Columns[1].Width = columnWidth;
    if ( nowrap ) {
      table.Columns[0].NoWrap();
      table.Columns[1].NoWrap();
    }
    clueCutoff = columnWidth - 6;

    List<ClueModel> across = new List<ClueModel>(model.Across);
    List<ClueModel> down = new List<ClueModel>(model.Down);

    var (activeOrdinal,activeDirection) = model.ActiveClue;

    while ( across.Count() != 0 || down.Count() != 0 ) {
      String aclue = "";
      String dclue = "";
      bool activeAClue = false;
      bool activeDClue = false;
      if (across.Any()) {
        ClueModel clue = across[0];
        activeAClue = clue.ordinal == activeOrdinal && activeDirection == Direction.Across;
        aclue = string.Format("{0}  {1}",
            across[0].ordinal.ToString().PadRight(2),
            across[0].value);
        across.RemoveAt(0);
      }
      if (down.Any()) {
        ClueModel clue = down[0];
        activeDClue = clue.ordinal == activeOrdinal && activeDirection == Direction.Down;
        dclue = string.Format("{0}  {1}",
            down[0].ordinal.ToString().PadRight(2),
            down[0].value);
        down.RemoveAt(0);
      }

      if ( nowrap ) {
        aclue = aclue.Substring(0,Math.Min(aclue.Count(),clueCutoff));
        dclue = dclue.Substring(0,Math.Min(dclue.Count(),clueCutoff));
      } 

      if ( activeAClue ) {
        aclue = $"[yellow]{aclue}[/]";
      }
      if ( activeDClue ) {
        dclue = $"[yellow]{dclue}[/]";
      }

      table.AddRow(
          aclue,dclue
      );
    }


    return table;
  }

}

}
