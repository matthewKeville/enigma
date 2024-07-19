using Enums;

namespace UI.Commands {

  public class KeySequenceEqual : EqualityComparer<List<ConsoleKey>>
  {
      public override bool Equals(List<ConsoleKey> kl1, List<ConsoleKey> kl2)
      {
          if (object.ReferenceEquals(kl1, kl2))
              return true;

          if (kl1 is null || kl2 is null)
              return false;

          return kl1.SequenceEqual(kl2);

      }

      public override int GetHashCode(List<ConsoleKey> kl) {
        int hash = 41 * kl.Count();
        foreach ( ConsoleKey key in kl ) {
          hash*=((int)key);
        }
        return hash;
      }
  }

  /** Not sure if this implementation is any good,
   * the key problem is that we want to have instances of the KeySeqInterpreter
   * per UI element, so as to seperate the bindings. However, the current use
   * of this is a chain of KeySeqInterpreter, and we want that chain to lock
   * on insert mode. That is a KSI higher in the chain, shouldn't issue normal
   * commands while the insert mode is set from a child element.
   */
  public class KeySeqInterpreter {

    public static CommandMode InterpretMode = CommandMode.NORMAL;

    // command    : command to execute
    // propagate  : propagate the keys
    // sequence   : the sequence to propagate
    public class KeySeqResponse {
      public Command? Command;
      public bool Propagate;
      public List<ConsoleKey>? Sequence;
      public KeySeqResponse(Command? command,bool propagate,List<ConsoleKey>? sequence) {
        Command = command;
        Propagate = propagate;
        Sequence = sequence;
      }

      // key sequence consumed, but no command to return : internal command
      public static KeySeqResponse Noop() { 
        return new KeySeqResponse(null,false,null);
      }

    }

    private static String KeySequenceToString(List<ConsoleKey> keyList) {
      String result = "";
      foreach ( ConsoleKey key in keyList  ) {
        result += " " + key.ToString();
      }
      return result;
    }

    private List<ConsoleKey> keyPresses = new List<ConsoleKey>();
    private Dictionary<List<ConsoleKey>,Command> normalCommandMap;
    private Dictionary<List<ConsoleKey>,Command> insertCommandMap;
    // if true, insert mode can be activated 
    private static ConsoleKey INSERT_MODE_KEY = ConsoleKey.I;
    private static ConsoleKey NORMAL_MODE_KEY = ConsoleKey.Escape;

    //modal 
    public KeySeqInterpreter(
        Dictionary<List<ConsoleKey>,Command> normalCommandMap,
        Dictionary<List<ConsoleKey>,Command> insertCommandMap
      ) 
    {
      this.normalCommandMap = new Dictionary<List<ConsoleKey>, Command>(normalCommandMap,new KeySequenceEqual());
      this.normalCommandMap[new List<ConsoleKey>(){INSERT_MODE_KEY}] = new Command(CommandMode.META,CommandType.INSERT_MODE);

      this.insertCommandMap = new Dictionary<List<ConsoleKey>, Command>(insertCommandMap,new KeySequenceEqual());
      this.insertCommandMap[new List<ConsoleKey>(){NORMAL_MODE_KEY}] = new Command(CommandMode.META,CommandType.NORMAL_MODE);
    }

    //non modal
    public KeySeqInterpreter(
        Dictionary<List<ConsoleKey>,Command> normalCommandMap
      ) 
    {
      this.normalCommandMap = new Dictionary<List<ConsoleKey>, Command>(normalCommandMap,new KeySequenceEqual());
      this.insertCommandMap = new(new KeySequenceEqual());
    }

    /**
     * @Return V1 : the command mapped to the key sequnce
     * @Return V2 : whether the key sequence is a partial match
     * Matching, Partial or  Full depends on parity between InterpretMode
     * and the Command's Mode
     */
    private (Command?,bool) tryGetCommand(List<ConsoleKey> keys) {

       Dictionary<List<ConsoleKey>,Command> commandMap;
       commandMap = InterpretMode == CommandMode.NORMAL ? normalCommandMap : insertCommandMap;

      if ( commandMap.Count() == 0 ) {
        return (null,false);
      }

      //exact match?
      if ( commandMap.ContainsKey(keys) ) {
        return (commandMap[keys],true);
      } 

      //partial match?
      foreach ( List<ConsoleKey> keyList in commandMap.Keys ) {
        if ( keyList.Count >= keys.Count ) {
          bool pmatch = true;
          int k = 0;
          while ( k < keys.Count && pmatch ) {
            pmatch = keyList[k].Equals(keys[k]);
            k++;
          }
          if ( pmatch ) {
            return (null,true);
          }
        }
      }

      //no partial match
      return (null,false);

    }


    public KeySeqResponse ProcessKey(ConsoleKey key) {

      keyPresses.Add(key);

      (Command? possibleCommand,bool partialMatch) = tryGetCommand(keyPresses);

      if (!partialMatch) {
        Trace.WriteLine($" no key sequence match : {KeySequenceToString(keyPresses)}");
        KeySeqResponse response = new KeySeqResponse(null,true,new List<ConsoleKey>(keyPresses));
        keyPresses.Clear();
        return response;
      }

      if (possibleCommand is null) {
        Trace.WriteLine($" key sequence partial match : {KeySequenceToString(keyPresses)}");
        return new KeySeqResponse(
            null,
            false,
            null
        );
      }

      Trace.WriteLine($" key sequence match : {KeySequenceToString(keyPresses)}");
      Command command = (Command) possibleCommand;

      // Process Meta Command

      if ( command.Mode == CommandMode.META ) {
        Trace.WriteLine("hit a meta command");
        InterpretMode = command.Type == CommandType.NORMAL_MODE ? CommandMode.NORMAL : CommandMode.INSERT;
        keyPresses.Clear();
        return KeySeqResponse.Noop();
      }

      // Return non-Meta command

      keyPresses.Clear();
      return new KeySeqResponse(command,false,null);

    }

  }

}
