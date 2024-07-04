using Context;
using Spectre.Console;
using UI.Model.Status;

namespace UI.View.Spectre.Status {

public class StatusView : ISpectreView<Layout> {

  private StatusModel statusModel;
  private ClockView clockView;
  private ContextAccessor contextAccessor;

  public StatusView(ContextAccessor contextAccessor, ClockView clockView) {
    this.contextAccessor = contextAccessor;
    this.statusModel = contextAccessor.GetContext().statusModel;
    this.clockView = clockView;
  }

  public void SetContext(ApplicationContext context) {
  }

  public Layout Render() {

    Layout layout = new Layout("Status");

    if (this.statusModel is null) {
      return layout;
    }

    Table table = new Table();
    table.NoBorder();
    table.HideHeaders();
    table.Centered();

    table.AddColumn("Clock");
    table.AddColumn("Title");

    Panel name = new Panel(statusModel.title);
    name.PadLeft(2).PadRight(2);
    name.NoBorder();

    table.AddRow(clockView.Render());
    table.AddRow(name);

    layout.Update(table);
    return layout;
  }

}

}
