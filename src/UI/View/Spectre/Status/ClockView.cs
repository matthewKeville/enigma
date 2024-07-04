using Context;
using Spectre.Console;
using UI.Model.Status;

namespace UI.View.Spectre.Status {

public class ClockView : SpectreView<ClockModel> {

  protected override Panel render() {
    TimeSpan elapsed = (DateTime.UtcNow - model.start);
    String minutes = elapsed.Minutes > 9 ? ""+elapsed.Minutes : "0"+elapsed.Minutes;
    String seconds = elapsed.Seconds > 9 ? ""+elapsed.Seconds : "0"+elapsed.Seconds;
    String timestring = string.Format("{0}:{1}",minutes,seconds);
    Panel panel = new Panel(timestring);
    return panel;
  }

}

}
