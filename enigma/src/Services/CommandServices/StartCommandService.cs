using Event;
using UI.View.Game;
using Terminal.Gui;
using Services.CommandServices.Exceptions;
using Serilog;
using Logging;

namespace Services.CommandServices {

public class StartCommandService {

  private EventBus eventBus;
  private GameView gameView;
  private static ILogger _logger = Logger.For<StartCommandService>();

  public StartCommandService(EventBus eventBus, GameView gameView) {
    this.eventBus = eventBus;
    this.gameView = gameView;
  }

  /// <summary>
  /// start the crossword with a given ID
  /// </summary>
  /// <exception cref="BadArgsException"></exception>
  /// <exception cref="CrosswordNotFoundException"></exception>
  public void Start(string[] args) {

    if ( args.Count() < 2 ) {
      throw new BadArgsException("invalid number of arguments");
    }

    int puzzleId = -1;
    try {
      puzzleId = Int32.Parse(args[1]);
      startGame(puzzleId);
      Application.Shutdown();
    } catch (Exception exception) {
      _logger.Error(exception.ToString());
      throw new BadArgsException($"puzzleId> {args[1]}",exception);
    }

    //TODO need to verify input is non-zero and maps to a known puzzleId
    //and throw new CrosswordNotFoundException()

    startGame(puzzleId);
    Application.Shutdown();

  }

  /// TODO : This needs to check if the puzzleId exists first...
  private void startGame(int puzzleId) {

    _logger.Information($"starting game for {puzzleId}");

    eventBus.PostEvent(new StartPuzzleEventArgs(puzzleId));

    _logger.Debug($"Intializing Terminal.GUI");
    Application.Init(); // Terminal.GUI
    
    //clear all predefined bindings, but preserve Command.Tab for the
    //MessageQuery class
    KeyBinding keyBinding = Application.KeyBindings.Get(Key.Tab);
    Application.KeyBindings.Clear();
    Application.KeyBindings.Bindings.Add(Key.Tab,keyBinding);

    Application.KeyDown += (sender,key) => {
      //Default was escape
      if (key.Equals(Key.C.WithCtrl)) {
        Application.RequestStop();
      }
    };

    //Application.Force16Colors = true;
    Terminal.Gui.ConfigurationManager.Themes.Theme = "Light";
    Terminal.Gui.ConfigurationManager.Apply();

    _logger.Debug($"Running Game");

    Application.Run(gameView);
  }

}

}
