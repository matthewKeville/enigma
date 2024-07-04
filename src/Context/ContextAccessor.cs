using Entity;
using Services;
using UI.Model;
using UI.Model.Browser;
using UI.Model.Game;
using UI.Model.Help;
using UI.Model.Status;

namespace Context {

  public class ContextAccessor {

    private CrosswordService crosswordService;
    private ApplicationContext context;

    public ContextAccessor(CrosswordService crosswordService) {
      this.crosswordService = crosswordService;
      this.context = buildDefaultContext();
    }

    public void UpdateContext(Crossword crossword) {

      ApplicationContext newContext = new ApplicationContext();

      //keep
      newContext.rootModel = context.rootModel;
      newContext.browserModel = context.browserModel;
      newContext.helpModel = context.helpModel;

      //context changed
      newContext.statusModel = new StatusModel();
      newContext.statusModel.title = "Ohhhhhhh";
      newContext.gameModel = new GameModel();    

      //todo : disentagle model from entity
      newContext.gridModel = new GridModel(crossword.model.colCount,crossword.model.rowCount,crossword.model.words);
      newContext.cluesModel = new CluesModel(crossword.model,newContext.gridModel); //this is bad, pls fixme
      newContext.clockModel = new ClockModel();    

      this.context = newContext;
    }

    public ApplicationContext GetContext() {
      if ( context is null ) {
        Trace.WriteLine("no context");
        Environment.Exit(1);
      }
      return this.context;
    }

    private ApplicationContext buildDefaultContext() {
      ApplicationContext context = new ApplicationContext();
      context.rootModel = new RootModel();
      context.browserModel = new BrowserModel();
      context.browserModel.headers = crosswordService.GetCrosswordHeaders();
      context.statusModel = new StatusModel();
      context.statusModel.title = "Ohhhhhhh";
      context.helpModel = new HelpModel();    
      return context;
    }

  }

}
