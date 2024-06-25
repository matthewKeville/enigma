using Model;
using Spectre.Console;

namespace UI {


class Clues {

  public Crossword crossword { get; set; }
  private Layout layout = new Layout("Clues");
  private Table table = new Table();

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
        aclue = across[0].answer;
        across.RemoveAt(0);
      }
      if (down.Any()) {
        dclue = down[0].answer;
        down.RemoveAt(0);
      }
      table.AddRow(aclue,dclue);
    }

    layout.Update(table);
  }

  public Layout Render() {
    return layout;
  }

}

}
