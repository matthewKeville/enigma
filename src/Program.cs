using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.CrosswordInstaller;
using Services.CrosswordInstaller.NYT;
using Event;
using Terminal.Gui;
using UI.View.Game;
using UI.View.Game.Clues;
using Settings.Theme;
using Microsoft.Extensions.Configuration;
using Settings;

HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
AddServices(builder.Services);
AddConfigs(builder.Configuration);
IHost host = builder.Build();

DatabaseContext dbContext = host.Services.GetRequiredService<DatabaseContext>();
UpdateOrCreateDB(dbContext);

host.Start();

Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
Trace.AutoFlush = true;

// command flags args

if ( args.Count() == 0 ) {
  Console.WriteLine("invalid arguments");
  Console.WriteLine($"see help for available commands");
  return;
}

String command = args[0];
switch ( command ) {
  case "list":
    if ( args.Count() > 1 ) {
      var flags = parseFlags(args,1);

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
    break;
  case "install":
    Trace.WriteLine("installation request");
    if ( args.Count() < 3 ) {
      Console.Error.WriteLine("invalid number of arguments");
      Console.Error.WriteLine("usage : install <type> <type-args>");
      return;
    }

    InstallationRequest request;
    switch (args[1]) {
      case "nyt":
          try {
          //  mm/dd/yyyy
          String dateString = args[2];
          DateOnly date = DateOnly.Parse(dateString);
          request = new NYTInstallationRequest() {
            Date = date
          };
          } catch (FormatException ex) {
            Console.Error.WriteLine($"bad date format, use : dd/mm/yyyy ");
            return;
          }
        break;
      default:
        Console.Error.WriteLine($"unknown crossword type : {args[1]}");
        return;
    }
    Trace.WriteLine("valid installation request");
    install(request);
    break;
  case "help":
    help();
    break;
  case "start":
    if ( args.Count() < 2 ) {
      Console.Error.WriteLine("invalid number of arguments");
      Console.Error.WriteLine("usage : start <puzzleId> (flags)");
      return;
    }
    try {
      int puzzleId = Int32.Parse(args[1]);
      startGame(puzzleId);
    } catch (Exception exception) {
      Console.Error.WriteLine("invalid argument");
      Console.Error.WriteLine("puzzleId is an int");
    }
    break;
  default:
    Console.WriteLine($"unknown command {command}");
    Console.WriteLine($"see help for available commands");
    break;

}

/**
if ( args.Count() < 2 ) {
  Console.Error.WriteLine("you must supply a puzzle id");
  return;
}

try {
  int puzzleId = Int32.Parse(args[1]);
  startGame(puzzleId);
} catch (Exception exception) {
  if ( exception is FormatException ) {
    Console.WriteLine("puzzle id is an int");
    return;
  }
  if ( exception is OverflowException ) {
    Console.WriteLine("puzzle id is too large");
    return;
  }
  Console.WriteLine("unhandled exception " + exception.ToString());
  return;
  
}
*/

List<char> parseFlags(string[] args,int flagStart) {
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

void help() {
  Console.WriteLine("Available Commands");
  Console.WriteLine();
  Console.WriteLine("help".PadRight(10) + "display this menu");
  Console.WriteLine("list".PadRight(10) + "list installed crosswords");
  Console.WriteLine("start".PadRight(10) + "start the interactive crossword player");
}

void list(
    bool hideStarted = false,
    bool hideUnstarted = false,
    bool hideFinished = false,
    bool onlyStarted = false,
    bool onlyUnstarted = false,
    bool onlyComplete = false
    ) {

  CrosswordService crosswordService = host.Services.GetService<CrosswordService>()!;
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

void install(InstallationRequest request) {
  CrosswordInstallerService installer = host.Services.GetService<CrosswordInstallerService>()!;
  installer.Install(request);
}


void startGame(int puzzleId) {

  EventBus eventBus = host.Services.GetService<EventBus>()!;
  GameView gameView = host.Services.GetService<GameView>()!;
  eventBus.PostEvent(new StartPuzzleEventArgs(puzzleId));


  Application.Init();
  Application.KeyBindings.Clear();
  Application.KeyDown += (sender,key) => {
    //Default was escape
    if (key.Equals(Key.C.WithCtrl)) {
      Application.RequestStop();
    }
  };

  //Application.Force16Colors = true;
  Terminal.Gui.ConfigurationManager.Themes.Theme = "Light";
  Terminal.Gui.ConfigurationManager.Apply();

  Application.Run(gameView);
  Application.Shutdown ();
}

void AddServices(IServiceCollection services) {
  builder.Services.AddSingleton<DatabaseContext, DatabaseContext>();
  builder.Services.AddSingleton<CrosswordService, CrosswordService>();
  builder.Services.AddSingleton<NYTCrosswordFetcher, NYTCrosswordFetcher>();
  builder.Services.AddSingleton<NYTCrosswordParser, NYTCrosswordParser>();
  builder.Services.AddSingleton<CrosswordInstallerService, CrosswordInstallerService>();
  builder.Services.AddSingleton<EventBus, EventBus>();
  builder.Services.AddSingleton<Theme, Theme>();
  builder.Services.AddSingleton<GameView, GameView>();
  builder.Services.AddSingleton<GridView, GridView>();
  builder.Services.AddSingleton<CluesView, CluesView>();
  builder.Services.AddSingleton<CluesSingleView, CluesSingleView>();
  builder.Services.AddSingleton<CluesSplitView, CluesSplitView>();
  builder.Services.AddSingleton<AppSettings, AppSettings>();
}

void AddConfigs(IConfigurationBuilder builder) {
  builder.AddJsonFile("appsettings.json");
  builder.AddEnvironmentVariables();
}

void UpdateOrCreateDB(DatabaseContext dbContext) {
  dbContext.Database.Migrate();
}

