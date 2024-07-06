namespace UI.Model.Help {

  public record KeyMapInfo(ConsoleKey key,Command.Command command);

  public class HelpModel : IModel {
    public List<KeyMapInfo> commandMappings = 
      new List<KeyMapInfo>() {
        new KeyMapInfo(ConsoleKey.A,Command.Command.CONFIRM),
        new KeyMapInfo(ConsoleKey.B,Command.Command.MOVE_UP),
        new KeyMapInfo(ConsoleKey.C,Command.Command.MOVE_DOWN)
      };
  }

}
