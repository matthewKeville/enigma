namespace UI.View.ViewModel {

class ClockViewModel {

  public DateTime start { get; private set; }

  public void reset() {
    start = DateTime.UtcNow;
  }
}

}
