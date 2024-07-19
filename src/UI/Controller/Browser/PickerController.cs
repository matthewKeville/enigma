using Services;
using UI.Commands;
using UI.Event;
using UI.Events;
using UI.Model.Browser;
using UI.View.Spectre.Browser;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller.Browser {

public class PickerController : Controller<PickerModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private PickerView pickerView;
  private KeySeqInterpreter keySeqInterpreter;

  public PickerController(EventDispatcher eventDispatcher,CrosswordService crosswordService,PickerView pickerView) {
    this.model = new PickerModel();
    this.model.headers = crosswordService.GetCrosswordHeaders();

    this.pickerView = pickerView;
    this.pickerView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;
    this.crosswordService = crosswordService;
    
    buildKeySeqInterpreter();
  }

  public void ProcessKeyInput(ConsoleKey key) {
    Trace.WriteLine($" key is {key.ToString()}");
    KeySeqResponse response = keySeqInterpreter.ProcessKey(key);
    
    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } else if ( response.Propagate ) {
      // no child
    }
  }


  private void ProcessCommand(Command command) {
    switch ( command.Type ) {
      case CommandType.MOVE_UP:
        model.MoveUp();
        break;
      case CommandType.MOVE_DOWN:
        model.MoveDown();
        break;
      case CommandType.CONFIRM:
        Trace.WriteLine("load puzzled nooped");
        eventDispatcher.DispatchEvent(new LoadPuzzleEventArgs(model.getActiveHeader.PuzzleId));
        break;

    }
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {
    if (eventArgs.GetType() == typeof(PuzzleInstalledEvent)) {
      this.model = new PickerModel();
      this.model.headers = crosswordService.GetCrosswordHeaders();
      this.pickerView.SetModel(this.model);
    }
    if (eventArgs.GetType() == typeof(ExitPuzzleEventArgs)) {
      this.model = new PickerModel();
      this.model.headers = crosswordService.GetCrosswordHeaders();
      this.pickerView.SetModel(this.model);
    }
  }

  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> normalCommandMap = new Dictionary<List<ConsoleKey>,Command>();
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.J}] = new Command(CommandMode.NORMAL,CommandType.MOVE_DOWN);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.K}] = new Command(CommandMode.NORMAL,CommandType.MOVE_UP);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.Enter}] = new Command(CommandMode.NORMAL,CommandType.CONFIRM);
    keySeqInterpreter = new KeySeqInterpreter(normalCommandMap);
  }

}

}
