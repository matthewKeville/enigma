namespace UI.Game {

using System.Drawing;
    using System.Text;
    using Event;
using Terminal.Gui;
    using UI.Model.Game;

    public class GridView : Window
{
  private DatabaseContext _dbContext;
  private EventBus _eventBus;
  private GridModel _gridModel;
  private bool _isInsertMode = false;

  public GridView(DatabaseContext dbContext,EventBus eventBus) 
  {
      _dbContext = dbContext;
      _eventBus = eventBus;
      _eventBus.Register(this,(args) => { 
        if ( args is StartPuzzleEventArgs ) {
          OnStartPuzzleEvent((StartPuzzleEventArgs) args);
        }
      });

      _eventBus.Register(this,(eventArgs) => {
        if ( eventArgs is StartPuzzleEventArgs ) {
          SetFocus();
        }}
      );

      KeyBindings.Clear();
  }

  public override bool OnKeyDown(Key key) {

    //context
    switch (key.KeyCode) {
      //prevent further key processing, with early return
      case KeyCode.I:
        _isInsertMode = true;
        return true;
      case KeyCode.Esc:
        _isInsertMode = false;
        return true;
      default:
        break;
    }

    if (!_isInsertMode) {

      switch (key.KeyCode) {

        //movement
        case KeyCode.J:
          _gridModel.MoveDown();
          break;
        case KeyCode.K:
          _gridModel.MoveUp();
          break;
        case KeyCode.H:
          _gridModel.MoveLeft();
          break;
        case KeyCode.L:
          _gridModel.MoveRight();
          break;

        //editing
        case KeyCode.X:
          _gridModel.DeleteChar();
          break;

        //orientation
        case KeyCode.Space:
          _gridModel.SwapOrientation();
          break;

        default:
          return false;

      }

    } else {
      char insertChar = ((char)key.KeyCode);
      _gridModel.InsertChar(insertChar);
    }

    SetNeedsDisplay();
    return true;

  }

  public override void OnDrawContent(Rectangle contentArea) {

    base.OnDrawContent(contentArea);
    var ctx = Driver;

    foreach ( GridCharModel gcm in _gridModel.GridCharModels ) {
      Rune rune = gcm.IsBlock  ? new Rune('#') : new Rune(gcm.C);
      Move(gcm.X, gcm.Y);
      ctx.AddRune(rune);
    }

    GridCharModel selection = _gridModel.Selection;
    Rune selectRune = new Rune(selection.C);
    var attr = new Terminal.Gui.Attribute(Terminal.Gui.Color.Red,Terminal.Gui.Color.Green);
    Driver.SetAttribute(attr);
    Move(selection.X,selection.Y);
    ctx.AddRune(selectRune);

  }

  private void Init(int crosswordId) {
    _gridModel = new GridModel(_dbContext.GridChars.Where( gc => gc.CrosswordId == crosswordId ).ToList());
    SetNeedsDisplay();
  }

  private void OnStartPuzzleEvent(StartPuzzleEventArgs args) {
    Init(args.CrosswordId);
  }
}

}
