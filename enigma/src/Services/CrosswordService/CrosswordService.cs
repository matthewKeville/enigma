using Entity;
using Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Services.CrosswordService {

  public class CrosswordService {

    private DatabaseContext dbCtx;
    private ILogger _logger = Logger.For<CrosswordService>();

    public CrosswordService(DatabaseContext dbCtx) {
      this.dbCtx = dbCtx;
    }

    public Crossword GetCrossword(int crosswordId) {
      Crossword? crosswordQ = dbCtx.Crosswords
        .Where( c => c.Id == crosswordId)
        .Include( c => c.Clues)
        .Include( c => c.GridChars)
        .FirstOrDefault();
      if ( crosswordQ is null ) {
        _logger.Error($" trying to access a crossword entity that does not exist id {crosswordId}");
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
