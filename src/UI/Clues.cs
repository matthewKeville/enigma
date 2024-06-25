using Model;
using Spectre.Console;

namespace UI {


class Clues {

  public Crossword crossword { get; set; }
  private Layout layout = new Layout("Clues");
  private Table table = new Table();
  private int clueCutoff = 40;

  public Clues(Crossword crossword) {
    this.crossword = crossword;
    initLayout();
  }

  private void initLayout() {
    table.AddColumn("Across");
    table.AddColumn("Down");
    table.Width = 200;
    table.Columns[0].Alignment(Justify.Left);
    table.Columns[0].Width = 90;
    table.Columns[1].Alignment(Justify.Left);
    table.Columns[1].Width = 90;

    List<Word>  across = crossword.words
      .Where( w => w.direction == Direction.Across )
      .OrderBy( w => w.i )
      .ToList();

    List<Word>  down = crossword.words
      .Where( w => w.direction == Direction.Down )
      .OrderBy( w => w.i )
      .ToList();

    while ( across.Count() != 0 || down.Count() != 0 ) {
      String aclue = "";
      String dclue = "";
      if (across.Any()) {
        aclue = string.Format("{0}  {1}",
            across[0].i.ToString().PadRight(2),
            across[0].prompt);
        across.RemoveAt(0);
      }
      if (down.Any()) {
        dclue = string.Format("{0}  {1}",
            down[0].i.ToString().PadRight(2),
            down[0].prompt);
        down.RemoveAt(0);
      }
      table.AddRow(
          aclue.Substring(0,Math.Min(aclue.Count(),clueCutoff)),
          dclue.Substring(0,Math.Min(dclue.Count(),clueCutoff))
      );
    }

    layout.Update(table);
  }

  public Layout Render() {
    return layout;
  }

}

}
