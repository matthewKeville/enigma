namespace UI.Model.Status {

  public class ClockModel : IModel {

    public TimeSpan Elapsed = TimeSpan.Zero;
    public DateTime LastResumed = DateTime.UnixEpoch;

    public TimeSpan SessionElapsed() {
      TimeSpan sessionElapsed = DateTime.UtcNow - LastResumed;
      return sessionElapsed + Elapsed;
    }
  }

}
