namespace Services.CommandServices {

public static class HelpCommandService {
  
  public static void Help() {
    Console.WriteLine("Available Commands");
    Console.WriteLine();
    Console.WriteLine("help".PadRight(10) + "display this menu");
    Console.WriteLine("list".PadRight(10) + "list installed crosswords");
    Console.WriteLine("start".PadRight(10) + "start the interactive crossword player");
  }

}

} 

