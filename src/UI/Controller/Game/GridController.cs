using Context;
using Enums;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Game;

namespace UI.Controller.Game {

public class GridController : Controller<GridModel> {

  private ContextAccessor contextAccessor;
  private EventDispatcher eventDispatcher;

  public GridController(ContextAccessor ctx, EventDispatcher eventDispatcher) {
    this.contextAccessor = ctx;
    Register(ctx);
    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;
  }

  public void PerformAndNotifyGridWordChange(Action action) {
    WordModel prevWord = model.ActiveWord();
    action();
    if ( !prevWord.Equals(model.ActiveWord()) ) {
      WordModel current = model.ActiveWord();
      Trace.WriteLine($" move into (i,x,y) ({current.i},{current.x},{current.y}");
      eventDispatcher.DispatchEvent(new GridWordChangeEventArgs(current.i,current.direction));
    }
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {

    switch ( commandEventArgs.command ) {

      case Command.Command.MOVE_LEFT:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.LEFT);} );
        break;

      case Command.Command.MOVE_RIGHT:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.RIGHT);} );
        break;

      case Command.Command.MOVE_UP:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.UP);} );
        break;

      case Command.Command.MOVE_DOWN:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.DOWN);} );
        break;

      case Command.Command.MOVE_WORD_START:
          model.MoveToWordStart();
        break;
      case Command.Command.MOVE_WORD_END:
          model.MoveToWordEnd();
        break;

      case Command.Command.INSERT_CHAR:
        if (commandEventArgs.key is null) {
          Trace.WriteLine("Critical error , INSERT_CHAR command requires a key, it is null");
          Environment.Exit(1);
        } 
        model.InsertKey((ConsoleKey)commandEventArgs.key);
        break;

      case Command.Command.DEL_CHAR:
        model.DeleteKey();
        break;

      case Command.Command.DEL_WORD:
        model.DeleteWord();
        break;

      case Command.Command.SWAP_ORIENTATION:
        PerformAndNotifyGridWordChange( () => { model.SwapOrientation();} );
        break;
    }
  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {
    // respond to clues change event
  }

}

}
