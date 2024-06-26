using System.Diagnostics;
using UI.Command;
using UI.View.ViewModel;
using View.ViewModel;

namespace UI.Controller {

public class GridController {

  private GridViewModel gridViewModel;

  public GridController(GridViewModel gridViewModel) {
    this.gridViewModel = gridViewModel;
  }

  public void ProcessCommandEvent(object? sender, CommandEventArgs commandEventArgs) {
    switch ( commandEventArgs.command ) {
      //fallthrough
      case Command.Command.MOVE_LEFT:
        gridViewModel.MoveEntry(Move.LEFT);
        break;

      case Command.Command.MOVE_RIGHT:
        gridViewModel.MoveEntry(Move.RIGHT);
        break;

      case Command.Command.MOVE_UP:
        gridViewModel.MoveEntry(Move.UP);
        break;

      case Command.Command.MOVE_DOWN:
        gridViewModel.MoveEntry(Move.DOWN);
        break;

      case Command.Command.INSERT_CHAR:
        Trace.WriteLine("inserting character");
        if (commandEventArgs.key is null) {
          Trace.WriteLine("Critical error , INSERT_CHAR command requires a key, it is null");
          Environment.Exit(1);
        } 
        gridViewModel.InsertKey((ConsoleKey)commandEventArgs.key);
        break;

      case Command.Command.DEL_CHAR:
        Trace.WriteLine("deleting character");
        gridViewModel.DeleteKey();
        break;

      case Command.Command.SWAP_ORIENTATION:
        Trace.WriteLine("swapping orientation");
        gridViewModel.SwapOrientation();
        break;
    }
  }

}

}
