namespace Event {

  public class EventBus() {

    private List<(Object,Action<EventArgs>)> listeners = new List<(Object,Action<EventArgs>)>();

    public void Register(object listener,Action<EventArgs> action) {
      listeners.Add((listener,action));
    }

    public void Unregister(object listener) {
      listeners.RemoveAll( x => x.Item1.Equals(listener) );
    }

    public void PostEvent(EventArgs e) {
      foreach ((Object,Action<EventArgs>) listener in listeners) {
        listener.Item2(e);
      }
    }

  }
}
