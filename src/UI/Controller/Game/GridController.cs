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
      model.crosswordId = args.puzzleId;
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

  private void CheckForCompletion() {
    if (model.IsComplete()) {
      eventDispatcher.DispatchEvent(new PuzzleCompleteArgs(model.crosswordId));
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

      case CommandType.FIND_CHAR:
          model.FindChar((ConsoleKey)command.Key);
        break;

      //TODO : FIND_BACK_CHAR

      case CommandType.MOVE_WORD:
        PerformAndNotifyGridWordChange( () => { model.MoveWord();} );
        break;

      case CommandType.MOVE_BACK_WORD:
        PerformAndNotifyGridWordChange( () => { model.MoveBackWord();} );
        break;

      case CommandType.MOVE_ANSWER:
        PerformAndNotifyGridWordChange( () => { model.MoveAnswer();} );
        break;

      case CommandType.MOVE_BACK_ANSWER:
        PerformAndNotifyGridWordChange( () => { model.MoveBackAnswer();} );
        break;


      case CommandType.CHECK_WORD:
        model.CheckWord();
        break;


      case CommandType.DEL_WORD:
        model.DeleteWord();
        break;

      case CommandType.CHANGE_WORD:
        model.DeleteWord();
        KeySeqInterpreter.InterpretMode = CommandMode.INSERT;
        break;

      case CommandType.REPLACE_CHAR:
        model.DeleteKey(false);
        model.InsertKey((ConsoleKey)command.Key,false);
        CheckForCompletion();
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
        CheckForCompletion();
        break;

      // this can be fired from normal or insert mode, interpretation changes
      case CommandType.DEL_CHAR:
        if ( command.Mode == CommandMode.INSERT ) {
          model.DeleteKey(true);
        } else {
          model.DeleteKey(false);
        }
        break;

    }
  }


  private void buildKeySeqInterpreter() {

    Dictionary<List<ConsoleKey>,Command> insertCommandMap = CommandUtils.InsertAlphaMap();
    Dictionary<List<ConsoleKey>,Command> normalCommandMap = new ();


    //normal (movement)
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.L}] = new Command(CommandMode.NORMAL,CommandType.MOVE_RIGHT);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.H}] = new Command(CommandMode.NORMAL,CommandType.MOVE_LEFT);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.J}] = new Command(CommandMode.NORMAL,CommandType.MOVE_DOWN);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.K}] = new Command(CommandMode.NORMAL,CommandType.MOVE_UP);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.D4}] = new Command(CommandMode.NORMAL,CommandType.MOVE_WORD_END);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.D6}] = new Command(CommandMode.NORMAL,CommandType.MOVE_WORD_START);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.W}] = new Command(CommandMode.NORMAL,CommandType.MOVE_WORD);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.G,ConsoleKey.W}] = new Command(CommandMode.NORMAL,CommandType.MOVE_ANSWER);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.G,ConsoleKey.B}] = new Command(CommandMode.NORMAL,CommandType.MOVE_BACK_ANSWER);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.B}] = new Command(CommandMode.NORMAL,CommandType.MOVE_BACK_WORD);


    //normal (edit)
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.X}] = new Command(CommandMode.NORMAL,CommandType.DEL_CHAR);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.D,ConsoleKey.W}] = new Command(CommandMode.NORMAL,CommandType.DEL_WORD);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.C,ConsoleKey.W}] = new Command(CommandMode.NORMAL,CommandType.CHANGE_WORD);

    //normal (replace)
    foreach ( KeyValuePair<List<ConsoleKey>,Command> pair in CommandUtils.ReplaceAlphaMap() ) {
      normalCommandMap[new List<ConsoleKey>(pair.Key)] = new Command(pair.Value);
    }

    //normal (find char)
    foreach ( KeyValuePair<List<ConsoleKey>,Command> pair in CommandUtils.FindAlphaMap() ) {
      normalCommandMap[new List<ConsoleKey>(pair.Key)] = new Command(pair.Value);
    }

    //normal (etc)
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.Spacebar}] = new Command(CommandMode.NORMAL,CommandType.SWAP_ORIENTATION);
    normalCommandMap[new List<ConsoleKey>(){ConsoleKey.Z}] = new Command(CommandMode.NORMAL,CommandType.CHECK_WORD);

    keySeqInterpreter = new KeySeqInterpreter(normalCommandMap,insertCommandMap);
  }

}

}
