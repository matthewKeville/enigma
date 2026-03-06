namespace Services.CommandServices {

public static class HelpCommandService {
 
  /*
   *  Not loving this implementation. Perhaps I can do a similar thing
   *  to the NYT fetcher, where this uses reflection to look at
   *  a CommadMethod abstraction and hook into a decorator for the exposed
   *  functions. This way the info is scoped into the command.
   */
  public static void Help() {

    Console.WriteLine("Available Commands");
    Console.WriteLine();
    Console.WriteLine("help".PadRight(40) + "display this menu");
    Console.WriteLine("list".PadRight(40) + "list installed crosswords");
    Console.WriteLine("start".PadRight(40) + "start the interactive crossword player");
    Console.WriteLine("sync".PadRight(40) + "sync plugin configuration");

    Console.WriteLine("plugin list".PadRight(40) + "list installed plugins");
    Console.WriteLine("plugin info <plugin>".PadRight(40) + "info for a specific plugin");
    Console.WriteLine("plugin methods <plugin>".PadRight(40) + "list install methods for a plugin");
    Console.WriteLine("plugin list".PadRight(40) + "list installed plugins");
    Console.WriteLine("plugin install <plugin> <method> <args>".PadRight(40) + "install a crossword from a plugin");
  }

}

} 

