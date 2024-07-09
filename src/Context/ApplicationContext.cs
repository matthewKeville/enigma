using Entity;
using UI.Model;
using UI.Model.Browser;
using UI.Model.Game;
using UI.Model.Help;
using UI.Model.Status;

namespace Context {

  public class RootContext {
    public RootModel rootModel;
  }

  public class BrowserContext {
    public BrowserModel browserModel;
    public PickerModel pickerModel;
    public InstallerModel installerModel;
  }

  public class HelpContext {
    public HelpModel helpModel;
  }

  public class GameContext {
    public GameModel gameModel;
    public CluesModel cluesModel;
    public GridModel gridModel;
    public ClockModel clockModel;
    public StatusModel statusModel;

    public Crossword crossword;
  }

}
