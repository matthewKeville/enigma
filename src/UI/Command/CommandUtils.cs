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

    private Command? tryGetInsertCommand(ConsoleKey key)  {
      int keyNum = ((int)key);
      if ( 65 <= keyNum && keyNum <= 90 ) {
          return new Command(CommandMode.INSERT,CommandType.INSERT_CHAR,key);
      } else if ( 97 <= keyNum && keyNum <= 122 ) {
          return new Command(CommandMode.INSERT,CommandType.INSERT_CHAR,key);
      } else if ( key == ConsoleKey.Backspace ) {
          return new Command(CommandMode.INSERT,CommandType.DEL_CHAR,key);
      }
      return null;
    }

  }

}
