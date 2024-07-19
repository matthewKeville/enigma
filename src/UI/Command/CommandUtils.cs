namespace UI.Commands {

  public class CommandUtils {

    public static Dictionary<List<ConsoleKey>,Command> InsertAlphaMap() {
      Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();
      for ( int i = 65; i <= 90; i++ ) {
        ConsoleKey key = (ConsoleKey) i;
        commandMap[new List<ConsoleKey>(){key}] = new Command(CommandMode.INSERT,CommandType.INSERT_CHAR,key);
      }
      commandMap[new List<ConsoleKey>(){ConsoleKey.Backspace}] = new Command(CommandMode.INSERT,CommandType.DEL_CHAR);
      return commandMap;
    }

    public static Dictionary<List<ConsoleKey>,Command> ReplaceAlphaMap() {
      Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();
      for ( int i = 65; i <= 90; i++ ) {
        ConsoleKey key = (ConsoleKey) i;
        commandMap[new List<ConsoleKey>(){ConsoleKey.R,key}] = new Command(CommandMode.NORMAL,CommandType.REPLACE_CHAR,key);
      }
      return commandMap;
    }

    public static Dictionary<List<ConsoleKey>,Command> FindAlphaMap() {
      Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();
      for ( int i = 65; i <= 90; i++ ) {
        ConsoleKey key = (ConsoleKey) i;
        commandMap[new List<ConsoleKey>(){ConsoleKey.F,key}] = new Command(CommandMode.NORMAL,CommandType.FIND_CHAR,key);
      }
      return commandMap;
    }

  }

}
