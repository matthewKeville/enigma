using System.Diagnostics;
using Entity;
using Model;
using Services;

namespace Context {

  public class ContextAccessor {

    private CrosswordService crosswordService;
    private ApplicationContext context;

    public ContextAccessor(CrosswordService crosswordService) {
      this.crosswordService = crosswordService;
      this.context = buildDefaultContext();
    }

    public void updateContext(Crossword crossword) {

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

    public ApplicationContext getContext() {
      if ( context is null ) {
        Trace.WriteLine("no context");
        Environment.Exit(1);
      }
      return this.context;
    }

    public ApplicationContext buildDefaultContext() {
      ApplicationContext context = new ApplicationContext();
      context.rootModel = new RootModel();
      context.browserModel = new BrowserModel();
      context.browserModel.headers = crosswordService.getCrosswordHeaders();
      context.statusModel = new StatusModel();
      context.statusModel.title = "Ohhhhhhh";
      context.helpModel = new HelpModel();    
      return context;
    }

  }

}
