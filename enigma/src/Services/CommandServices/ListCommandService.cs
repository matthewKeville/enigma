namespace Services.CommandServices {

using Services.CrosswordService;

public class ListCommandService {

  private Services.CrosswordService.CrosswordService crosswordService;

  public ListCommandService(CrosswordService crosswordService) {
    this.crosswordService = crosswordService;
  }

  /// <summary>
  /// list the installed crosswords to standard out
  /// </summary>
  public void List(string[] args) {

    if ( args.Count() > 1 ) {
      var flags = FlagParser.ParseFlags(args,1);

      bool hideStarted = flags.Contains('S');
      bool hideUnstarted = flags.Contains('U');
      bool hideComplete = flags.Contains('C');

      bool onlyStarted = flags.Contains('s');
      bool onlyUnstarted = flags.Contains('u');
      bool onlyComplete = flags.Contains('c');
      list(
          hideStarted : hideStarted,
          hideUnstarted : hideUnstarted,
          hideFinished : hideComplete,
          onlyStarted : onlyStarted,
          onlyUnstarted : onlyUnstarted,
          onlyComplete : onlyComplete
          );
    } else {
      list();
    }

  }

  private void list(
    bool hideStarted = false,
    bool hideUnstarted = false,
    bool hideFinished = false,
    bool onlyStarted = false,
    bool onlyUnstarted = false,
    bool onlyComplete = false
    ) {

    List<CrosswordHeader> headers = crosswordService.GetCrosswordHeaders();

    // only flags override any behaviour off hide flags
    // additive filters
    if ( onlyStarted || onlyComplete || onlyUnstarted ) {

      headers = headers.FindAll( h => {
        if ( onlyStarted ) {
          return h.Started && !h.Complete;
        }
        if ( onlyUnstarted ) {
          return !h.Started;
        }
        if ( onlyComplete ) {
          return h.Complete;
        }
        return false;
      });

    // subtractive filters
    } else {

      headers = headers.FindAll( h => {
        if ( hideUnstarted ) {
          return h.Started;
        }
        if ( hideStarted ) {
          return !h.Started;
        }
        if ( hideFinished ) {
          return !h.Complete;
        }
        return true;
      });

    }

    // ID TYPE TITLE ELAPSED STATUS
    String colSpace = "  ";

    Console.Write("ID".PadLeft(4));
    Console.Write(colSpace);
    Console.Write("Type".PadLeft(10));
    Console.Write(colSpace);
    Console.Write("Title".PadLeft(24));
    Console.Write(colSpace);
    Console.Write("Elapsed".PadLeft(8));
    Console.Write(colSpace);
    Console.Write("Status".PadLeft(12));
    Console.WriteLine();
    Console.WriteLine();

    foreach ( var h in headers ) {
      Console.Write(h.PuzzleId.ToString().PadLeft(4));
      Console.Write(colSpace);
      Console.Write(h.Type.ToString().PadLeft(10));
      Console.Write(colSpace);
      Console.Write(h.Title.ToString().PadLeft(24));

      // Console.Write(h.Elapsed.ToString().PadLeft(36));
      String elapsed = "";
      elapsed += h.Elapsed.Hours.ToString().PadLeft(2);
      elapsed += ":" + h.Elapsed.Minutes.ToString().PadLeft(2);
      elapsed += ":" +h.Elapsed.Seconds.ToString().PadLeft(2);

      Console.Write(colSpace);
      Console.Write(elapsed.PadLeft(8));

      String status = "";
      if ( h.Complete ) {
        status = "Complete";
      } else {
        status = h.Started ? "Started" : "Not Started";
      }

      Console.Write(colSpace);
      Console.Write(status.PadLeft(12));

      Console.WriteLine();
    }
  
  }
}

} 

