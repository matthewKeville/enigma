namespace UI.Command {

    public class CommandEventArgs : EventArgs {
      public Command command { get; set; }
      public ConsoleKey? key { get; set; }
      public CommandEventArgs(Command command) {
        Trace.WriteLine("Command event fired " + command.ToString());
        this.command = command;
      }
      public CommandEventArgs(Command command,ConsoleKey key) {
        Trace.WriteLine($"command event fired {command.ToString()} with key {key.ToString()}");
        this.command = command;
        this.key = key;
      }
    }

}
