using Enums;
using UI.Commands;
using UI.Controller.Browser;
using UI.Controller.Game;
using UI.Controller.Help;
using UI.Event;
using UI.Events;
using UI.Model;
using UI.View.Spectre;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller {

public class RootController : Controller<RootModel> {

  private RootView rootView;
  private GameController gameController;
  private BrowserController  browserController;
  private HelpController  helpController;
  private KeySeqInterpreter  keySeqInterpreter;

  public RootController(
      KeyDispatcher keyDispatcher,
      EventDispatcher eventDispatcher,
      RootView rootView,
      GameController gameController,
      BrowserController browserController,
      HelpController helpController) 
  {

    this.model = new RootModel();

    this.rootView = rootView;
    this.rootView.SetModel(this.model);

    this.browserController = browserController;
    this.gameController = gameController;
    this.helpController = helpController;

    eventDispatcher.RaiseEvent += ProcessEvent;
    keyDispatcher.RaiseKeyInputEvent += ProcessKeyInputEvent;

    buildKeySeqInterpreter();

  }

  private void ProcessKeyInputEvent(object? sender, KeyInputEventArgs keyInputEventArgs) {

    KeySeqResponse response = keySeqInterpreter.ProcessKey(keyInputEventArgs.key);

    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } else if ( response.Propagate ) {
      PropagateKeys(response.Sequence);
    }

  }

  public void PropagateKeys(List<ConsoleKey> keys) {
    Trace.WriteLine("root is propagating keys");
    foreach ( ConsoleKey key in keys ) {
      switch ( model.activeWindow ) {
        case Window.BROWSER:
          browserController.ProcessKeyInput(key);
          break;
        case Window.GAME:
          gameController.ProcessKeyInput(key);
          break;
      }
    }
  }

  public void ProcessCommand(Command command) {
    Trace.WriteLine("Root Controller recieved command");
    switch ( command.Type ) {
      case CommandType.TOGGLE_HELP:
        model.ToggleHelp();
        break;
    }
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {
      Trace.WriteLine("root controller recieved load puzzle");
      model.activeWindow = Window.GAME;
    }

    if (eventArgs.GetType() == typeof(ExitPuzzleEventArgs)) {
      Trace.WriteLine("root controller exit puzzle");
      model.activeWindow = Window.BROWSER;
    }
  }

  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();
    keySeqInterpreter = new KeySeqInterpreter(commandMap);
  }

}

}
