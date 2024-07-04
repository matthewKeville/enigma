using Context;
using Spectre.Console;
using UI.Model.Status;

namespace UI.View.Spectre.Status {

public class ClockView : ISpectreView<Panel> {

  private ClockModel clock;
  private ContextAccessor contextAccessor;

  public ClockView(ContextAccessor contextAccessor) {
    this.contextAccessor = contextAccessor;
    this.clock = contextAccessor.GetContext().clockModel;
  }

  public void SetContext(ApplicationContext context) {
  }

  public Panel Render() {

    if (this.clock is null) {
      return new Panel("");
    }

    TimeSpan elapsed = (DateTime.UtcNow - clock.start);
    String minutes = elapsed.Minutes > 9 ? ""+elapsed.Minutes : "0"+elapsed.Minutes;
    String seconds = elapsed.Seconds > 9 ? ""+elapsed.Seconds : "0"+elapsed.Seconds;
    String timestring = string.Format("{0}:{1}",minutes,seconds);
    Panel panel = new Panel(timestring);
    return panel;
  }

}

}
