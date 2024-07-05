using Context;
using Enums;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Game;

namespace UI.Controller.Game {

public class GridController {

  private GridModel gridModel;
  private ContextAccessor contextAccessor;
  private EventDispatcher eventDispatcher;

  public GridController(ContextAccessor contextAccessor, EventDispatcher eventDispatcher) {
    this.contextAccessor = contextAccessor;
    this.gridModel = (GridModel) contextAccessor.GetModel<GridModel>();
    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;
  }

  public void PerformAndNotifyGridWordChange(Action action) {
    WordModel prevWord = gridModel.ActiveWord();
    action();
    if ( !prevWord.Equals(gridModel.ActiveWord()) ) {
      WordModel current = gridModel.ActiveWord();
      Trace.WriteLine($" move into (i,x,y) ({current.i},{current.x},{current.y}");
      eventDispatcher.DispatchEvent(new GridWordChangeEventArgs(current.i,current.direction));
    }
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {

      case Command.Command.UPDATE_CONTEXT:
        Trace.WriteLine("puzzle swap triggered in game grid controller");
        this.gridModel = (GridModel) contextAccessor.GetModel<GridModel>();
        break;

      case Command.Command.MOVE_LEFT:
        PerformAndNotifyGridWordChange( () => { gridModel.MoveEntry(Move.LEFT);} );
        break;

      case Command.Command.MOVE_RIGHT:
        PerformAndNotifyGridWordChange( () => { gridModel.MoveEntry(Move.RIGHT);} );
        break;

      case Command.Command.MOVE_UP:
        PerformAndNotifyGridWordChange( () => { gridModel.MoveEntry(Move.UP);} );
        break;

      case Command.Command.MOVE_DOWN:
        PerformAndNotifyGridWordChange( () => { gridModel.MoveEntry(Move.DOWN);} );
        break;

      case Command.Command.MOVE_WORD_START:
          gridModel.MoveToWordStart();
        break;
      case Command.Command.MOVE_WORD_END:
          gridModel.MoveToWordEnd();
        break;

      case Command.Command.INSERT_CHAR:
        if (commandEventArgs.key is null) {
          Trace.WriteLine("Critical error , INSERT_CHAR command requires a key, it is null");
          Environment.Exit(1);
        } 
        gridModel.InsertKey((ConsoleKey)commandEventArgs.key);
        break;

      case Command.Command.DEL_CHAR:
        gridModel.DeleteKey();
        break;

      case Command.Command.DEL_WORD:
        gridModel.DeleteWord();
        break;

      case Command.Command.SWAP_ORIENTATION:
        PerformAndNotifyGridWordChange( () => { gridModel.SwapOrientation();} );
        break;
    }
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {
    // respond to clues change event
  }

}

}
