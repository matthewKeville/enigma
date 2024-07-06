using Context;
using Spectre.Console;
using UI.Model.Status;

namespace UI.View.Spectre.Status {

public class StatusView : SpectreView<StatusModel> {

  private ClockView clockView;

  public StatusView(ContextAccessor ctx, ClockView clockView) {
    Register(ctx);
    this.clockView = clockView;
  }

  protected override Layout render() {

    Layout layout = new Layout("Status");
    Table table = new Table();
    table.Centered();
    table.AddColumn(new TableColumn("Clock").Centered());

    table.HideHeaders();
    table.NoBorder();

    table.AddRow(new Markup(model.title));
    table.AddRow(new Markup(" ----<>---- "));
    table.AddRow(clockView.Render());

    layout.Update(table);
    return layout;
  }

}

}
