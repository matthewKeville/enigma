using System.Drawing;
using Entity;
using Enums;
using Services;
using UI.Model;
using UI.Model.Browser;
using UI.Model.Game;
using UI.Model.Help;
using UI.Model.Status;
using UI.View.Spectre;

namespace Context {

  public class ContextAccessor {

    private CrosswordService crosswordService;

    private RootContext rootContext;
    private GameContext gameContext;
    private HelpContext helpContext;
    private BrowserContext browserContext;

    public ContextAccessor(CrosswordService crosswordService) {
      this.crosswordService = crosswordService;
      buildDefaultContext();
    }

    public event EventHandler<EventArgs> RaiseContextChangeEvent;

    public void SaveCrossword() {

      //convert charMatrix into character string
      for ( int i = 0; i < gameContext.crossword.Columns; i++ ) {
        for ( int j = 0; j < gameContext.crossword.Rows; j++ ) {
          GridChar gc = gameContext.crossword.GridChars
            .Find( g => {
              return g.X == i && g.Y == j;
            });
          gc.C = gameContext.gridModel.CharMatrix[i,j];
        }
      }

      gameContext.crossword.Elapsed += DateTime.UtcNow - gameContext.clockModel.LastResumed;
      gameContext.crossword.WordCheckCount = gameContext.gridModel.WordCheckCount;

      crosswordService.UpdateCrossword(gameContext.crossword);
      rootContext.rootModel.activeWindow = Window.BROWSER;
      RaiseContextChangeEvent(this,new EventArgs());

    }

    public void LoadCrossword(Crossword crossword) {

      lock(SpectreRenderer.Rendering) {

        GameContext newGameContext = buildGameContext();
        newGameContext.crossword = crossword;

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


        //context changed
        newGameContext.statusModel = new StatusModel();
        newGameContext.statusModel.title = crossword.Published.ToShortDateString();


        // GAME

        newGameContext.gameModel = new GameModel();    

        // GAME :-> GRID

        newGameContext.gridModel = new GridModel();
        newGameContext.gridModel.ColumnCount = crossword.Columns;
        newGameContext.gridModel.RowCount = crossword.Rows;
        newGameContext.gridModel.Entry = new Point(0,0);
        newGameContext.gridModel.Orientation = Direction.Across;
        newGameContext.gridModel.Words = wordModels;
        newGameContext.gridModel.WordCheckCount = crossword.WordCheckCount;

        char[,] charMatrix = new char[crossword.Columns,crossword.Rows];

        foreach ( GridChar gc in crossword.GridChars ) {
          charMatrix[gc.X,gc.Y] = gc.C;
        }

        newGameContext.gridModel.CharMatrix = charMatrix;
        newGameContext.gridModel.StatusMatrix = new int[crossword.Columns,crossword.Rows];

        // GAME :-> CLUES

        newGameContext.cluesModel = new CluesModel()
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

        newGameContext.clockModel = new ClockModel(){
          Elapsed = crossword.Elapsed,
          LastResumed = DateTime.UtcNow
        };


        rootContext.rootModel.activeWindow = Window.GAME;
        gameContext = newGameContext;

        RaiseContextChangeEvent(this,new EventArgs());

      }

    }

    public IModel GetModel<K>() where K : IModel {
      Type type = typeof(K);

      if ( type == typeof(RootModel)) {
        return rootContext.rootModel;
      } else if (type == typeof(HelpModel)) {
        return helpContext.helpModel;
      } else if (type == typeof(GameModel)) {
        return gameContext?.gameModel;
      } else if (type == typeof(CluesModel)) {
        return gameContext?.cluesModel;
      } else if (type == typeof(GridModel)) {
        return gameContext?.gridModel;
      } else if (type == typeof(ClockModel)) {
        return gameContext?.clockModel;
      } else if (type == typeof(StatusModel)) {
        return gameContext?.statusModel;
      } else if (type == typeof(BrowserModel)) {
        return browserContext.browserModel;
      } else {
        Trace.WriteLine($"Critical error : unable to resolve type for GetModel<{type.ToString}>");
        Environment.Exit(0);
        return default(K);
      }

    }

    private GameContext buildGameContext() {
      GameContext gameContext = new GameContext();
      gameContext.clockModel = new ClockModel();
      gameContext.statusModel = new StatusModel();
      gameContext.statusModel.title = "";
      gameContext.gridModel = new GridModel();
      gameContext.cluesModel = new CluesModel();
      return gameContext;
    }

    private BrowserContext buildBrowserContext() {
      BrowserContext browserContext = new BrowserContext();
      browserContext.browserModel = new BrowserModel();
      browserContext.browserModel.headers = crosswordService.GetCrosswordHeaders();
      return browserContext;
    }

    private HelpContext buildHelpContext() {
      return new HelpContext(){
        helpModel = new HelpModel()
      };
    }

    private RootContext buildRootContext() {
      RootContext context = new RootContext();
      context.rootModel = new RootModel();
      return context;
    }

    private void buildDefaultContext() {
      rootContext = buildRootContext();
      browserContext = buildBrowserContext();
      helpContext = buildHelpContext();
    }

  }

}
