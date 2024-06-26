using Spectre.Console;
using UI.ViewModel;

namespace UI.View.Spectre.Game {

public class StatusView {

  private Layout layout = new Layout("Status");
  private Table table = new Table();

  private ClockView clockView;
  private StatusViewModel statusViewModel;

  public StatusView(ClockView clockView,StatusViewModel statusViewModel) {
    Console.WriteLine("in statusview cons");
    this.clockView = clockView;
    this.statusViewModel = statusViewModel;
    init();
  }

  public void init() {
    table.NoBorder();
    table.HideHeaders();
    table.Centered();

    table.AddColumn("Clock");
    table.AddColumn("Title");

    table.AddRow(clockView.Render());
    table.AddRow(statusViewModel.getTitle());
  }

  public Layout Render() {
    Panel name = new Panel(statusViewModel.getTitle());
    name.PadLeft(2).PadRight(2);
    name.NoBorder();
    table.UpdateCell(0,0,clockView.Render());
    table.UpdateCell(1,0,name);
    layout.Update(table);
    return layout;
  }

}

}
