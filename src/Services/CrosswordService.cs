using Entity;
using Microsoft.EntityFrameworkCore;
using UI.Model.Browser;

namespace Services {
  public class CrosswordService {

    private DatabaseContext dbCtx;

    public CrosswordService(DatabaseContext dbCtx) {
      this.dbCtx = dbCtx;
    }

    public Crossword GetCrossword(int crosswordId) {
      Crossword? crosswordQ = dbCtx.Crosswords
        .Where( c => c.Id == crosswordId)
        .Include( c => c.Words)
        .Include( c => c.GridChars)
        .FirstOrDefault();
      if ( crosswordQ is null ) {
        Trace.WriteLine($" trying to access a crossword entity that does not exist id {crosswordId}");
        Environment.Exit(4);
      }
      Crossword crossword = (Crossword) crosswordQ;
      //new?
      if (crossword.StartDate is null) {
        crossword.StartDate = DateTime.UtcNow;
      }
      return crossword;
    }

    public List<CrosswordHeader> GetCrosswordHeaders() {
      return dbCtx.Crosswords
        .Select( cw => new CrosswordHeader(){ 
            Title = cw.Title,
            Type = cw.Type,
            Published = cw.Published,
            StartDate = cw.StartDate,
            FinishDate = cw.FinishDate,
            Elapsed = cw.Elapsed,
            PuzzleId = cw.Id,
            } )
        .ToList();
    }

    public bool HasNYTCrossword(DateTime published) {
      return dbCtx.Crosswords.Any( 
          c => 
            c.Type == Enums.CrosswordType.NYTIMES 
            && c.Published == published
          );
    }

    public void AddCrossword(Crossword crossword) {
      dbCtx.Crosswords.Add(crossword);
      dbCtx.SaveChanges();
    }

    public void UpdateCrossword(Crossword crossword) {
      dbCtx.Update(crossword);
      dbCtx.SaveChanges();
    }

  }
}
