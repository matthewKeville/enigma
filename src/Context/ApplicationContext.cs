using Model;
using Services;

namespace Context {


  public class ApplicationContext {
    
    //private CrosswordModel crosswordModel;
    //rivate GridModel gridModely;
    //private CluesModel cluesModel;
    
    public RootModel rootModel;
    public HelpModel helpModel;
    public GameModel gameModel;
    public CluesModel cluesModel;
    public GridModel gridModel;
    public ClockModel clockModel;
    public StatusModel statusModel;

    public ApplicationContext(NYDebugCrosswordGenerator generator) {

      //this is really the persistance model

      CrosswordModel crossword = generator.crossword;

      //these are all really view models i.e. a projection of the model
      //onto application state

      this.rootModel = new RootModel();
      this.statusModel = new StatusModel();
      this.statusModel.title = "Ohhhhhhh";

      this.helpModel = new HelpModel();    

      this.gameModel = new GameModel();    
      this.gridModel = new GridModel(crossword.colCount,crossword.rowCount,crossword.words);

      this.cluesModel = new CluesModel(crossword,gridModel);
      this.clockModel = new ClockModel();    
    }

  }

}
