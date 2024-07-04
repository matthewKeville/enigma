using System.Diagnostics;
using Entity;
using Model;
using Services;

namespace Context {

  public class ContextAccessor {

    public ContextAccessor() {
      buildDefaultContext();
    }

    private ApplicationContext context;

    public void newPuzzle(Puzzle puzzle) {

      //ignore passed puzzle for now
      NYDebugCrosswordGenerator generator = new NYDebugCrosswordGenerator();
      CrosswordModel crossword = generator.sample2();

      ApplicationContext newContext = new ApplicationContext();

      //keep
      newContext.rootModel = context.rootModel;
      newContext.browserModel = context.browserModel;
      newContext.helpModel = context.helpModel;

      //context changed
      newContext.statusModel = new StatusModel();
      newContext.statusModel.title = "Ohhhhhhh";
      newContext.gameModel = new GameModel();    
      newContext.gridModel = new GridModel(crossword.colCount,crossword.rowCount,crossword.words);
      newContext.cluesModel = new CluesModel(crossword,newContext.gridModel); //this is bad, pls fixme
      newContext.clockModel = new ClockModel();    

      this.context = newContext;
    }

    public void loadPuzzle(Puzzle puzzle) {
      //build context from puzzle
    }

    public ApplicationContext getContext() {
      if ( context is null ) {
        Trace.WriteLine("no context");
        Environment.Exit(1);
      }
      return this.context;
    }

    public void buildDefaultContext() {
      ApplicationContext context = new ApplicationContext();
      context.rootModel = new RootModel();
      context.browserModel = new BrowserModel();
      context.browserModel.headers = new List<PuzzleHeader>() {
        new PuzzleHeader(){
          puzzleId = 0,
          date = DateTime.UtcNow.AddMonths(1),
        },
        new PuzzleHeader(){
          puzzleId=1,
          date = DateTime.UtcNow.AddMonths(3),
        },
        new PuzzleHeader(){
          puzzleId=2,
          date = DateTime.UtcNow.AddMonths(5),
        }
      };
      context.statusModel = new StatusModel();
      context.statusModel.title = "Ohhhhhhh";
      context.helpModel = new HelpModel();    
      this.context = context;

    }

  }

}
