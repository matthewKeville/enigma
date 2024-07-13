using UI.Commands;

namespace UI.Model.Help {

  public record KeyMapInfo(ConsoleKey key,CommandType commandType);

  public class HelpModel : IModel {

    public List<KeyMapInfo> commandMappings = 
      new List<KeyMapInfo>() {
        /**
        new KeyMapInfo(ConsoleKey.A,Command.Command.CONFIRM),
        new KeyMapInfo(ConsoleKey.B,Command.Command.MOVE_UP),
        new KeyMapInfo(ConsoleKey.C,Command.Command.MOVE_DOWN)
        */
      };

  }

}
