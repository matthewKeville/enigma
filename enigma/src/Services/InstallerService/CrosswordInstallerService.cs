namespace Services.CrosswordInstaller {
    using Entity;
    using Enums;
    using Models.Plugin.V1;
using Services.CrosswordService;

  public class CrosswordInstallerService {

    private DatabaseContext _dbCtx;
    private CrosswordService crosswordService;

    public CrosswordInstallerService(DatabaseContext dbCtx, CrosswordService crosswordService) {
      this._dbCtx = dbCtx;
      this.crosswordService = crosswordService;
    }

    public void Install(FetchResponse fetch) {
      Trace.WriteLine("installing crossword");

      List<Clue> clues = new List<Clue>();

      List<GridChar> chars = Enumerable
        .Range(0,fetch.Rows*fetch.Columns)
        .Select( i => new GridChar () 
            { 
              IsBlock = true,
              X = i % fetch.Columns,
              Y = i / fetch.Columns,
            }
        )
        .ToList();


      foreach (  Models.Plugin.V1.FetchResponse.Clue fclue in fetch.Clues ) {

        //Clue transformation

        Clue clue = 
          new Clue() {
            I = fclue.I,
            X = fclue.X,
            Y = fclue.Y,
            Direction = fclue.D == Models.Plugin.V1.FetchResponse.Clue.Direction.Across ?
              Direction.Across : Direction.Down,
            Prompt = fclue.Prompt,
            Answer = fclue.Answer
          };

        clues.Add(clue);

        //GridChar transformation

        if (clue.Direction == Direction.Across) {

          int step = 0;
          foreach ( char c in clue.Answer ) {

            int flatIndex= (clue.Y * fetch.Columns) + (clue.X) + step;
            GridChar refChar = chars[flatIndex];

            refChar.IsBlock = false;
            refChar.X = clue.X + step;
            refChar.Y = clue.Y;
            refChar.AnswerChar = c;

            step++;

          } 

        } else {

          int step = 0;
          foreach ( char c in clue.Answer ) {

            int flatIndex = ((clue.Y + step) * fetch.Columns) + clue.X;
            GridChar refChar = chars[flatIndex];

            refChar.IsBlock = false;
            refChar.X = clue.X;
            refChar.Y = clue.Y + step;
            refChar.AnswerChar = c;
            refChar.UserChar = ' ';

            step++;

          } 

        }

      }


      Crossword crossword = new Crossword() {
        Type = fetch.Meta.Plugin,
        Title = fetch.Title,
        //Published = fetch.ReleaseDate,
        Rows = fetch.Rows,
        Columns = fetch.Columns,
        GridChars = chars,
        Clues = clues
      };

      crosswordService.AddCrossword(crossword);

    }
  }
}
