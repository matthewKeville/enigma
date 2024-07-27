using UI.Commands;
using UI.Event;
using UI.Events;
using UI.Model.Game.Complete;
using UI.View.Spectre.Game.Complete;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller.Complete {

public class CompleteController : Controller<CompleteModel> {

  private EventDispatcher eventDispatcher;
  private CompleteView completeView;
  private KeySeqInterpreter keySeqInterpreter;

  public CompleteController(CompleteView completeView,EventDispatcher eventDispatcher) {

    this.model= new CompleteModel();
    this.completeView = completeView;
    this.completeView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;

    buildKeySeqInterpreter();
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(PuzzleCompleteArgs)) {
      //get puzzle information ...
    }

  }

  public void ProcessKeyInput(ConsoleKey key) {

    KeySeqResponse response = keySeqInterpreter.ProcessKey(key);
    
    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } 

  }

  private void ProcessCommand(Command command) {
    switch ( command.Type ) {
      case CommandType.EXIT:
        Trace.WriteLine($" exiting completed crossword");
        eventDispatcher.DispatchEvent(new ExitPuzzleEventArgs(model.crosswordId));
        break;
    }
  }

  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> normalCommandMap = new Dictionary<List<ConsoleKey>,Command>();
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.Q}] = new Command(CommandMode.NORMAL,CommandType.EXIT);
    keySeqInterpreter = new KeySeqInterpreter(normalCommandMap);
  }


}

}
