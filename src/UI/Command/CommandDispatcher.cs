namespace UI.Command {

  public class CommandDispatcher {

    public event EventHandler<UI.Command.CommandEventArgs> RaiseCommandEvent;
      private void triggerCommand(Command command) {
        RaiseCommandEvent(this, new CommandEventArgs(command));
      }

    public void DispatchCommand(CommandEventArgs args) {
      RaiseCommandEvent(this,args);
    }
  }

}
