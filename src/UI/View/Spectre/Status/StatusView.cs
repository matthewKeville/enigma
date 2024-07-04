using Context;
using Spectre.Console;
using UI.Model.Status;

namespace UI.View.Spectre.Status {

public class StatusView : SpectreView<StatusModel> {

  private ClockView clockView;

  protected override Layout render() {

    Layout layout = new Layout("Status");

    Table table = new Table();
    table.NoBorder();
    table.HideHeaders();
    table.Centered();

    table.AddColumn("Clock");
    table.AddColumn("Title");

    Panel name = new Panel(model.title);
    name.PadLeft(2).PadRight(2);
    name.NoBorder();

    table.AddRow(clockView.Render());
    table.AddRow(name);

    layout.Update(table);
    return layout;
  }

}

}
