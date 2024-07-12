using System.Drawing;
using Entity;
using Enums;
using Services;
using UI.Command;
using UI.Event;
using UI.Events;
using UI.Model.Game;
using UI.View.Spectre.Game;

namespace UI.Controller.Game {

public class GridController : Controller<GridModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private GridView gridView;

  public GridController(EventDispatcher eventDispatcher,CrosswordService crosswordService,GridView gridView) {
    this.model = new GridModel();

    this.gridView = gridView;
    this.gridView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;

    this.crosswordService = crosswordService;

  }

  private void PerformAndNotifyGridWordChange(Action action) {
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

      case Command.Command.MOVE_WORD:
        PerformAndNotifyGridWordChange( () => { model.MoveWord();} );
        break;
      case Command.Command.MOVE_BACK_WORD:
        PerformAndNotifyGridWordChange( () => { model.MoveBackWord();} );
        break;

      case Command.Command.CHECK_WORD:
        model.CheckWord();
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

    if (eventArgs.GetType() == typeof(CluesWordChangeEventArgs)) {

      Trace.WriteLine(" Recieved Clue Word Change ");
      CluesWordChangeEventArgs args = ((CluesWordChangeEventArgs) eventArgs);
      this.model.MoveToOrdinal(args.ordinal,args.direction);

    }

    if (eventArgs.GetType() == typeof(LoadPuzzleEventArgs)) {

      LoadPuzzleEventArgs args = ((LoadPuzzleEventArgs) eventArgs);
      Crossword crossword = crosswordService.GetCrossword(args.puzzleId);

      model = new GridModel();
      model.ColumnCount = crossword.Columns;
      model.RowCount = crossword.Rows;
      model.Orientation = Direction.Across;
      model.Words = new List<WordModel>();
      foreach ( Word eword in crossword.Words ) {
        model.Words.Add( new WordModel(){
          x = eword.X,
          y = eword.Y,
          i = eword.I,
          direction = eword.Direction,
          answer = eword.Answer,
          prompt = eword.Clue
        });
      }
      model.WordCheckCount = crossword.WordCheckCount;
      model.CharMatrix = new char[crossword.Columns,crossword.Rows];
      foreach ( GridChar gc in crossword.GridChars ) {
        model.CharMatrix[gc.X,gc.Y] = gc.C;
      }
      model.StatusMatrix = new int[crossword.Columns,crossword.Rows];
      model.Entry = model.Words
        .OrderBy( w => w.i)
        .Take(1)
        .Select( w => new Point(w.x,w.y)).First();

      gridView.SetModel(model);

    }

    if (eventArgs.GetType() == typeof(ExitPuzzleEventArgs)) {

      ExitPuzzleEventArgs args = ((ExitPuzzleEventArgs) eventArgs);
      Crossword crossword = crosswordService.GetCrossword(args.puzzleId);

      //convert charMatrix into character string
      for ( int i = 0; i < model.ColumnCount; i++ ) {
        for ( int j = 0; j < model.RowCount; j++ ) {
          GridChar gc = crossword.GridChars
            .Find( g => {
              return g.X == i && g.Y == j;
            });
          gc.C = model.CharMatrix[i,j];
        }
      }
      crossword.WordCheckCount = model.WordCheckCount;

      crosswordService.UpdateCrossword(crossword);

    }

  }

}

}
