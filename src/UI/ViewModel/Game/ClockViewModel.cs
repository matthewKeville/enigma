namespace UI.View.ViewModel {

class ClockViewModel {

  public DateTime start { get; private set; }

  public ClockViewModel() {
    start = DateTime.UtcNow;
  }

  public void reset() {
    start = DateTime.UtcNow;
  }
}

}
