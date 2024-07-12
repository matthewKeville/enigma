using Services;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Browser;
using UI.View.Spectre.Browser;

namespace UI.Controller.Browser {

public class PickerController : Controller<PickerModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private PickerView pickerView;

  public PickerController(EventDispatcher eventDispatcher,CrosswordService crosswordService,PickerView pickerView) {
    this.model = new PickerModel();
    this.model.headers = crosswordService.GetCrosswordHeaders();

    this.pickerView = pickerView;
    this.pickerView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;
    this.crosswordService = crosswordService;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {

      case Command.Command.MOVE_UP:
        model.MoveUp();
        break;
      case Command.Command.MOVE_DOWN:
        model.MoveDown();
        break;

      case Command.Command.CONFIRM:
        eventDispatcher.DispatchEvent(new LoadPuzzleEventArgs(model.getActiveHeader.PuzzleId));
        break;

    }
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {
    if (eventArgs.GetType() == typeof(PuzzleInstalledEvent)) {
      Trace.WriteLine("Puzzle Install Event recieved in PickerController");
      this.model = new PickerModel();
      this.model.headers = crosswordService.GetCrosswordHeaders();
      this.pickerView.SetModel(this.model);
    }
  }

}

}
