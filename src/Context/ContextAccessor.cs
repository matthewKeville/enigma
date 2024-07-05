using System.Drawing;
using Entity;
using Enums;
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
      //TOOD sync inital data between clues & grid

      //keep
      newContext.rootModel = context.rootModel;
      newContext.browserModel = context.browserModel;
      newContext.helpModel = context.helpModel;

      //context changed
      newContext.statusModel = new StatusModel();
      newContext.statusModel.title = "Ohhhhhhh";

      // GAME

      newContext.gameModel = new GameModel();    

      // GAME :-> GRID

      newContext.gridModel = new GridModel();
      newContext.gridModel.ColumnCount = crossword.model.colCount;
      newContext.gridModel.RowCount = crossword.model.rowCount;
      newContext.gridModel.Entry = new Point(0,0);
      newContext.gridModel.Orientation = Direction.Across;
      newContext.gridModel.Words = crossword.model.words;

      char[,] charMatrix = new char[crossword.model.colCount,crossword.model.rowCount];

      //set all to block 
      for ( int i = 0; i < crossword.model.colCount; i++ ) {
        for ( int j = 0; j < crossword.model.rowCount; j++ ) {
          charMatrix[i,j] = '\0';
        }
      }

      //set words as spaces
      foreach ( WordModel word in crossword.model.words ) {
        for ( int x = 0; x < word.answer.Count(); x++ ) {
          if ( word.direction == Direction.Across ) {
            charMatrix[word.x+x,word.y] = ' ';
          } else {
            charMatrix[word.x,word.y+x] = ' ';
          }
        }
      }

      newContext.gridModel.CharMatrix = charMatrix;

      // GAME :-> CLUES

      newContext.cluesModel = new CluesModel()
      {
          Across = crossword.model.words.Where( w => w.direction == Direction.Across )
            .OrderBy( w => w.i )
            .Select( w => new ClueModel(w.i,w.prompt))
            .ToList(),
          Down = crossword.model.words
            .Where( w => w.direction == Direction.Down )
            .OrderBy( w => w.i )
            .Select( w => new ClueModel(w.i,w.prompt))
            .ToList(),
          ActiveClue = (0,Direction.Across)
      };

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
