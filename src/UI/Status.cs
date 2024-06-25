using Model;
using Spectre.Console;

namespace UI {


class Status {

  public Crossword crossword { get; set; }
  private Layout layout = new Layout("Status");
  private Table table = new Table();
  private Clock clock = new Clock();

  public Status(Crossword crossword) {
    this.crossword = crossword;
    init();
  }

  public void init() {
    table.NoBorder();
    table.HideHeaders();
    table.Centered();

    table.AddColumn("Clock");
    table.AddColumn("Title");

    table.AddRow(clock.Render());
    table.AddRow(crossword.name);
  }

  public Layout Render() {
    Panel name = new Panel(crossword.name);
    name.PadLeft(2).PadRight(2);
    name.NoBorder();
    table.UpdateCell(0,0,clock.Render());
    table.UpdateCell(1,0,name);
    layout.Update(table);
    return layout;
  }

}

}
