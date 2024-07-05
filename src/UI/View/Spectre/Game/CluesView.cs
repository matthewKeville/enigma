using Enums;
using Spectre.Console;
using UI.Model.Game;

namespace UI.View.Spectre.Game {

public class CluesView : SpectreView<CluesModel> {

  private int clueCutoff = 40;

  protected override Layout render() {

    Layout layout = new Layout("Clues");
    Table table = new Table();

    table.AddColumn("Across");
    table.AddColumn("Down");
    table.Width = 200;
    table.Columns[0].Alignment(Justify.Left);
    table.Columns[0].Width = 90;
    table.Columns[1].Alignment(Justify.Left);
    table.Columns[1].Width = 90;

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

      aclue = aclue.Substring(0,Math.Min(aclue.Count(),clueCutoff));
      dclue = dclue.Substring(0,Math.Min(dclue.Count(),clueCutoff));

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

    layout.Update(table);

    return layout;
  }

}

}
