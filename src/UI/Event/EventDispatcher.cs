namespace UI.Event {

  public class EventDispatcher {

    public event EventHandler<EventArgs> RaiseEvent;

    public void DispatchEvent(EventArgs args) {
      RaiseEvent(this,args);
    }

  }


}
