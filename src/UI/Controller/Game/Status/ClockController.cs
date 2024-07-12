using Entity;
using Services;
using UI.Event;
using UI.Events;
using UI.Model.Status;
using UI.View.Spectre.Status;

namespace UI.Controller.Game.Status {

public class ClockController : Controller<ClockModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private ClockView clockView;

  public ClockController(
      EventDispatcher eventDispatcher,
      CrosswordService crosswordService,
      ClockView clockView) 
  {
    this.model = new ClockModel();

    this.clockView = clockView;
    this.clockView.SetModel(model);

    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;
    this.crosswordService = crosswordService;
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {

      LoadPuzzleEventArgs args = ((LoadPuzzleEventArgs) eventArgs);
      Crossword crossword = crosswordService.GetCrossword(args.puzzleId);

      this.model = new ClockModel() {
        Elapsed = crossword.Elapsed,
        LastResumed = DateTime.UtcNow
      };

      clockView.SetModel(model);

    }


    if (eventArgs.GetType() == typeof(ExitPuzzleEventArgs)) {

      ExitPuzzleEventArgs args = ((ExitPuzzleEventArgs) eventArgs);
      Crossword crossword = crosswordService.GetCrossword(args.puzzleId);

      crossword.Elapsed += DateTime.UtcNow - model.LastResumed;
      crosswordService.UpdateCrossword(crossword);

    }

  }
}

}
