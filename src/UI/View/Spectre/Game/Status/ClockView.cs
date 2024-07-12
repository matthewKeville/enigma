using Spectre.Console;
using UI.Model.Status;

namespace UI.View.Spectre.Status {

public class ClockView : SpectreView<ClockModel> {

  protected override Markup render() {
    String minutes = model.SessionElapsed().Minutes > 9 ? ""+model.SessionElapsed().Minutes : "0"+model.SessionElapsed().Minutes;
    String seconds = model.SessionElapsed().Seconds > 9 ? ""+model.SessionElapsed().Seconds : "0"+model.SessionElapsed().Seconds;
    String timestring = string.Format("{0}:{1}",minutes,seconds);
    return new Markup(timestring);
  }

}

}
