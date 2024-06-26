using Spectre.Console;
using UI.View.ViewModel;

namespace UI.View.Spectre {

public class ClockView {

  private ClockViewModel clockViewModel;
  public ClockView() {
    clockViewModel = new ClockViewModel();
  }

  public Panel Render() {
    TimeSpan elapsed = (DateTime.UtcNow - clockViewModel.start);
    String minutes = elapsed.Minutes > 9 ? ""+elapsed.Minutes : "0"+elapsed.Minutes;
    String seconds = elapsed.Seconds > 9 ? ""+elapsed.Seconds : "0"+elapsed.Seconds;
    String timestring = string.Format("{0}:{1}",minutes,seconds);
    Panel panel = new Panel(timestring);
    return panel;
  }

}

}
