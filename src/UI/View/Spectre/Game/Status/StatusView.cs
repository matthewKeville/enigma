using Spectre.Console;
using UI.Model.Status;

namespace UI.View.Spectre.Status {

public class StatusView : SpectreView<StatusModel> {

  private ClockView clockView;

  public StatusView(ClockView clockView) {
    this.clockView = clockView;
  }

  protected override Table render() {

    Table table = new Table();
    table.Centered();
    table.AddColumn(new TableColumn("Clock").Centered());
    table.HideHeaders();
    table.NoBorder();
    table.AddRow(new Markup(model.title));
    table.AddRow(new Markup(" ----<>---- "));
    table.AddRow(clockView.Render());
    return table;
  }

}

}
