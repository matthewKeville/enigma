using Entity;
using Services;
using UI.Event;
using UI.Events;
using UI.Model.Status;
using UI.View.Spectre.Status;

namespace UI.Controller.Game.Status {

public class StatusController : Controller<StatusModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private StatusView statusView;

  public ClockController clockController;

  public StatusController(
      EventDispatcher eventDispatcher,
      CrosswordService crosswordService,
      ClockController clockController,
      StatusView statusView) 
  {

    this.model = new StatusModel();

    this.statusView = statusView;
    this.statusView.SetModel(model);
    this.clockController = clockController;

    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;
    this.crosswordService = crosswordService;
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

      Trace.WriteLine(" status controller load puzzle");
    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {
      LoadPuzzleEventArgs args = ((LoadPuzzleEventArgs) eventArgs);
      Crossword crossword = crosswordService.GetCrossword(args.puzzleId);

      this.model = new StatusModel() {
        title = crossword.Title
      };
      statusView.SetModel(model);

    }
  }
}

}
