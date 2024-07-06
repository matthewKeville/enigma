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

    public event EventHandler<EventArgs> RaiseContextChangeEvent;

    public void UpdateContext(Crossword crossword) {

      ApplicationContext newContext = new ApplicationContext();

      List<WordModel> wordModels = new List<WordModel>();
      foreach ( Word eword in crossword.Words ) {
        wordModels.Add( new WordModel(){
          x = eword.X,
          y = eword.Y,
          i = eword.I,
          direction = eword.Direction,
          answer = eword.Answer,
          prompt = eword.Clue
        });
      }

      //TOOD sync inital models between clues & grid

      //keep
      newContext.rootModel = context.rootModel;
      newContext.rootModel.activeWindow = Window.GAME;
      newContext.browserModel = context.browserModel;
      newContext.helpModel = context.helpModel;

      //context changed
      newContext.statusModel = new StatusModel();
      newContext.statusModel.title = crossword.Published.ToShortDateString();


      // GAME

      newContext.gameModel = new GameModel();    

      // GAME :-> GRID

      newContext.gridModel = new GridModel();
      newContext.gridModel.ColumnCount = crossword.Columns;
      newContext.gridModel.RowCount = crossword.Rows;
      newContext.gridModel.Entry = new Point(0,0);
      newContext.gridModel.Orientation = Direction.Across;
      newContext.gridModel.Words = wordModels;

      char[,] charMatrix = new char[crossword.Columns,crossword.Rows];

      //set all to block 
      for ( int i = 0; i < crossword.Columns; i++ ) {
        for ( int j = 0; j < crossword.Rows; j++ ) {
          charMatrix[i,j] = '\0';
        }
      }

      //set words as spaces
      foreach ( Word word in crossword.Words ) {
        for ( int x = 0; x < word.Answer.Count(); x++ ) {
          if ( word.Direction == Direction.Across ) {
            charMatrix[word.X+x,word.Y] = ' ';
          } else {
            charMatrix[word.X,word.Y+x] = ' ';
          }
        }
      }

      newContext.gridModel.CharMatrix = charMatrix;

      // GAME :-> CLUES

      newContext.cluesModel = new CluesModel()
      {
          Across = crossword.Words.Where( w => w.Direction == Direction.Across )
            .OrderBy( w => w.I )
            .Select( w => new ClueModel(w.I,w.Clue))
            .ToList(),
          Down = crossword.Words
            .Where( w => w.Direction == Direction.Down )
            .OrderBy( w => w.I )
            .Select( w => new ClueModel(w.I,w.Clue))
            .ToList(),
          ActiveClue = (0,Direction.Across)
      };

      newContext.clockModel = new ClockModel(){
        Elapsed = TimeSpan.Zero,
        LastResumed = DateTime.UtcNow
      };


      this.context = newContext;

      RaiseContextChangeEvent(this,new EventArgs());

    }

    public ApplicationContext GetContext() {
      if ( context is null ) {
        Trace.WriteLine("no context");
        Environment.Exit(1);
      }
      return this.context;
    }

    public IModel GetModel<K>() where K : IModel {
      Type type = typeof(K);
      if ( type == typeof(RootModel)) {
        return context.rootModel;
      } else if (type == typeof(HelpModel)) {
        return context.helpModel;
      } else if (type == typeof(GameModel)) {
        return context.gameModel;
      } else if (type == typeof(CluesModel)) {
        return context.cluesModel;
      } else if (type == typeof(GridModel)) {
        return context.gridModel;
      } else if (type == typeof(ClockModel)) {
        return context.clockModel;
      } else if (type == typeof(StatusModel)) {
        return context.statusModel;
      } else if (type == typeof(BrowserModel)) {
        return context.browserModel;
      } else {
        Trace.WriteLine($"Critical error : unable to resolve type for GetModel<{type.ToString}>");
        Environment.Exit(0);
        return default(K);
      }
    }

    private ApplicationContext buildDefaultContext() {
      ApplicationContext context = new ApplicationContext();
      context.rootModel = new RootModel();
      context.clockModel = new ClockModel();
      context.browserModel = new BrowserModel();
      context.browserModel.headers = crosswordService.GetCrosswordHeaders();
      context.statusModel = new StatusModel();
      context.statusModel.title = "";
      context.helpModel = new HelpModel();    
      context.gridModel = new GridModel();
      context.cluesModel = new CluesModel();
      return context;
    }

  }

}
