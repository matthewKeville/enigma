using System.Drawing;
using Entity;
using Enums;
using Services;
using UI.Commands;
using UI.Event;
using UI.Events;
using UI.Model.Game;
using UI.View.Spectre.Game;
using static UI.Commands.KeySeqInterpreter;

namespace UI.Controller.Game {

public class GridController : Controller<GridModel> {

  private EventDispatcher eventDispatcher;
  private CrosswordService crosswordService;
  private GridView gridView;
  private KeySeqInterpreter keySeqInterpreter;

  public GridController(EventDispatcher eventDispatcher,CrosswordService crosswordService,GridView gridView) {
    this.model = new GridModel();

    this.gridView = gridView;
    this.gridView.SetModel(this.model);

    this.eventDispatcher = eventDispatcher;
    this.eventDispatcher.RaiseEvent += ProcessEvent;

    this.crosswordService = crosswordService;
    buildKeySeqInterpreter();

  }

  public void ProcessEvent(object? sender,EventArgs eventArgs) {

    if (eventArgs.GetType() == typeof(CluesWordChangeEventArgs)) {

      Trace.WriteLine(" Recieved Clue Word Change ");
      CluesWordChangeEventArgs args = ((CluesWordChangeEventArgs) eventArgs);
      this.model.Orientation = args.direction;
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

  public void ProcessKeyInput(ConsoleKey key) {

    KeySeqResponse response = keySeqInterpreter.ProcessKey(key);
    
    if ( response.Command is not null ) {
      ProcessCommand(response.Command);
    } else if ( response.Propagate ) {
      //no children
    }

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

  public void ProcessCommand(Command command ) {

    switch ( command.Type ) {

      case CommandType.MOVE_LEFT:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.LEFT);} );
        break;

      case CommandType.MOVE_RIGHT:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.RIGHT);} );
        break;

      case CommandType.MOVE_UP:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.UP);} );
        break;

      case CommandType.MOVE_DOWN:
        PerformAndNotifyGridWordChange( () => { model.MoveEntry(Move.DOWN);} );
        break;

      case CommandType.MOVE_WORD_START:
          model.MoveToWordStart();
        break;
      case CommandType.MOVE_WORD_END:
          model.MoveToWordEnd();
        break;

      case CommandType.MOVE_WORD:
        PerformAndNotifyGridWordChange( () => { model.MoveWord();} );
        break;
      case CommandType.MOVE_BACK_WORD:
        PerformAndNotifyGridWordChange( () => { model.MoveBackWord();} );
        break;

      case CommandType.CHECK_WORD:
        model.CheckWord();
        break;


      case CommandType.DEL_WORD:
        model.DeleteWord();
        break;

      case CommandType.SWAP_ORIENTATION:
        PerformAndNotifyGridWordChange( () => { model.SwapOrientation();} );
        break;

      // PLS  REVIEW (internal command)
      case CommandType.INSERT_CHAR:
        if (command.Key is null) {
          Trace.WriteLine("Critical error , INSERT_CHAR command requires a key, it is null");
          Environment.Exit(1);
        } 
        model.InsertKey((ConsoleKey)command.Key);
        break;

      // PLS REVIEW (internal command)
      case CommandType.DEL_CHAR:
        model.DeleteKey();
        break;
    }
  }


  private void buildKeySeqInterpreter() {
    Dictionary<List<ConsoleKey>,Command> commandMap = new Dictionary<List<ConsoleKey>,Command>();

    //normal (movement)
    commandMap[new List<ConsoleKey>(){ConsoleKey.L}] = new Command(CommandMode.NORMAL,CommandType.MOVE_RIGHT);
    commandMap[new List<ConsoleKey>(){ConsoleKey.H}] = new Command(CommandMode.NORMAL,CommandType.MOVE_LEFT);
    commandMap[new List<ConsoleKey>(){ConsoleKey.J}] = new Command(CommandMode.NORMAL,CommandType.MOVE_DOWN);
    commandMap[new List<ConsoleKey>(){ConsoleKey.K}] = new Command(CommandMode.NORMAL,CommandType.MOVE_UP);
    commandMap[new List<ConsoleKey>(){ConsoleKey.D4}] = new Command(CommandMode.NORMAL,CommandType.MOVE_WORD_END);
    commandMap[new List<ConsoleKey>(){ConsoleKey.D6}] = new Command(CommandMode.NORMAL,CommandType.MOVE_WORD_START);
    commandMap[new List<ConsoleKey>(){ConsoleKey.W}] = new Command(CommandMode.NORMAL,CommandType.MOVE_WORD);
    commandMap[new List<ConsoleKey>(){ConsoleKey.B}] = new Command(CommandMode.NORMAL,CommandType.MOVE_BACK_WORD);

    //normal (edit)
    commandMap[new List<ConsoleKey>(){ConsoleKey.D,ConsoleKey.W}] = new Command(CommandMode.NORMAL,CommandType.DEL_WORD);

    //normal (etc)
    commandMap[new List<ConsoleKey>(){ConsoleKey.Spacebar}] = new Command(CommandMode.NORMAL,CommandType.SWAP_ORIENTATION);
    commandMap[new List<ConsoleKey>(){ConsoleKey.Z}] = new Command(CommandMode.NORMAL,CommandType.CHECK_WORD);

    //meta 
    commandMap[new List<ConsoleKey>(){ConsoleKey.I}] = new Command(CommandMode.META,CommandType.INSERT_MODE);
    commandMap[new List<ConsoleKey>(){ConsoleKey.Escape}] = new Command(CommandMode.META,CommandType.NORMAL_MODE);

    keySeqInterpreter = new KeySeqInterpreter(commandMap);
  }

}

}
