using Context;
using Entity;
using Services;
using UI.Command;
using UI.Model.Browser;

namespace UI.Controller.Browser {

public class BrowserController : Controller<BrowserModel> {

  private ContextAccessor contextAccessor;
  private CrosswordService crosswordService;

  public BrowserController(ContextAccessor ctx,CrosswordService crosswordService) {
    this.contextAccessor = ctx;
    Register(ctx);
    this.crosswordService = crosswordService;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {

      case Command.Command.MOVE_UP:
        model.selection = Math.Max(0,model.selection-1);
        break;
      case Command.Command.MOVE_DOWN:
        model.selection = Math.Min(model.selection+1,model.headers.Count()-1);
        break;
      case Command.Command.CONFIRM:
        int crosswordId = ((BrowserModel)contextAccessor.GetModel<BrowserModel>()).getActiveHeader.puzzleId;
        Trace.WriteLine($"requesting crossword id {crosswordId} from Browser Controller");
        Crossword crossword = crosswordService.GetCrossword(crosswordId);
        Trace.WriteLine($"retrieved {crosswordId} from Browser Controller");
        Trace.WriteLine($"{crossword.Columns},{crossword.Rows}");
        Trace.WriteLine($"printing words");
        foreach ( Word word in crossword.Words ) {
          Trace.WriteLine(word.Answer);
        }
        contextAccessor.UpdateContext(crossword);
        break;

    }
  }

}

}
