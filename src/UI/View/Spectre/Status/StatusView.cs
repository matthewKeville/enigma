/**
using Spectre.Console;
using View.Spectre;

namespace UI.View.Spectre.Game {

public class StatusView : IView {

  private Layout layout = new Layout("Status");

  private Status status;
  private ClockView clockView;

  public StatusView(Status status,ClockView clockView) {
    this.status = status;
    this.clockView = clockView;
  }

  public Layout Render() {

    Table table = new Table();
    table.NoBorder();
    table.HideHeaders();
    table.Centered();

    table.AddColumn("Clock");
    table.AddColumn("Title");

    Panel name = new Panel("fixme");
    name.PadLeft(2).PadRight(2);
    name.NoBorder();

    table.AddRow(name);
    table.AddRow("debug");

    layout.Update(table);
    return layout;
  }

}

}
*/

using Context;
using Model;
using Spectre.Console;

namespace UI.View.Spectre.Status {

public class StatusView {

  private StatusModel status;

  private ClockView clockView;
  private Layout layout = new Layout("Status");
  private ContextAccessor contextAccessor;

  public StatusView(ContextAccessor contextAccessor, ClockView clockView) {
    this.contextAccessor = contextAccessor;
    this.status = contextAccessor.getContext().statusModel;
    this.clockView = clockView;
  }

  public Layout Render() {

    Table table = new Table();
    table.NoBorder();
    table.HideHeaders();
    table.Centered();

    table.AddColumn("Clock");
    table.AddColumn("Title");

    Panel name = new Panel(status.title);
    name.PadLeft(2).PadRight(2);
    name.NoBorder();

    table.AddRow(clockView.Render());
    table.AddRow(name);

    layout.Update(table);
    return layout;
  }

}

}
