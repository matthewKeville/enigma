namespace UI.Command {

  public class CommandDispatcher {

    public event EventHandler<UI.Command.CommandEventArgs> raiseCommandEvent;
      private void triggerCommand(Command command) {
        raiseCommandEvent(this, new CommandEventArgs(command));
      }

    public void dispatchCommand(CommandEventArgs args) {
      raiseCommandEvent(this,args);
    }
  }

}
