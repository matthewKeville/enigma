using Context;
using Entity;
using Services;
using UI.Command;
using UI.Model.Browser;

namespace UI.Controller.Browser {

public class PickerController : Controller<PickerModel> {

  private ContextAccessor contextAccessor;
  private CrosswordService crosswordService;

  public PickerController(ContextAccessor ctx,CrosswordService crosswordService) {
    this.contextAccessor = ctx;
    Register(ctx);
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
        int crosswordId = ((PickerModel)contextAccessor.GetModel<PickerModel>()).getActiveHeader.PuzzleId;
        Trace.WriteLine($"requesting crossword id {crosswordId} from Browser Controller");
        Crossword crossword = crosswordService.GetCrossword(crosswordId);
        Trace.WriteLine($"retrieved {crosswordId} from Browser Controller");
        Trace.WriteLine($"{crossword.Columns},{crossword.Rows}");
        Trace.WriteLine($"printing words");
        foreach ( Word word in crossword.Words ) {
          Trace.WriteLine(word.Answer);
        }
        contextAccessor.LoadCrossword(crossword);
        break;

    }
  }

}

}
