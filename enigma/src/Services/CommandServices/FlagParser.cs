public static class FlagParser {

  public static List<char> ParseFlags(string[] args,int flagStart) {
    List<char> flags = new();
    for ( int i = flagStart; i < args.Count(); i++) {
      String word = args[i];  
      if ( word[0] == '-' ) {
        if ( word.Count() > 1 ) {
          flags.Add(word[1]);
        }
      }
    }
    return flags;
  }

}
