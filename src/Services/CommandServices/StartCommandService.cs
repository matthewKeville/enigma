using Event;
using UI.View.Game;
using Terminal.Gui;

namespace Services.CommandServices {

public class StartCommandService {

  private EventBus eventBus;
  private GameView gameView;

  public StartCommandService(EventBus eventBus, GameView gameView) {
    this.eventBus = eventBus;
    this.gameView = gameView;
  }

  public void Start(string[] args) {
    if ( args.Count() < 2 ) {
      Console.Error.WriteLine("invalid number of arguments");
      Console.Error.WriteLine("usage : start <puzzleId> (flags)");
      return;
    }
    try {
      int puzzleId = Int32.Parse(args[1]);
      startGame(puzzleId);
      Application.Shutdown();
    } catch (Exception exception) {
      Console.Error.WriteLine("invalid argument");
      Console.Error.WriteLine("puzzleId is an int");
      Console.Error.WriteLine(exception.ToString());
    }
  }

  private void startGame(int puzzleId) {

    eventBus.PostEvent(new StartPuzzleEventArgs(puzzleId));

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

    Application.Run(gameView);
  }

}

}
