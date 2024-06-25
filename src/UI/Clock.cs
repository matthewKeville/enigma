using Model;
using Spectre.Console;

namespace UI {


class Clock {

  public Crossword crossword { get; set; }
  private DateTime start;

  public Clock() {
    reset();
  }

  public void reset() {
    start = DateTime.UtcNow;
  }

  public Panel Render() {
    TimeSpan elapsed = (DateTime.UtcNow - start);
    String minutes = elapsed.Minutes > 9 ? ""+elapsed.Minutes : "0"+elapsed.Minutes;
    String seconds = elapsed.Seconds > 9 ? ""+elapsed.Seconds : "0"+elapsed.Seconds;
    String timestring = string.Format("{0}:{1}",minutes,seconds);
    Panel panel = new Panel(timestring);
    return panel;
  }

}

}
