using System.Diagnostics;
using Context;
using Entity;
using Model;
using Repository;

namespace Services {
  public class CrosswordService {

    private CrosswordRepository crosswordRepository;

    public CrosswordService(CrosswordRepository crosswordRepository ) {
      this.crosswordRepository = crosswordRepository;
    }

    public Crossword getCrossword(int crosswordId) {

      if (!crosswordRepository.has(crosswordId)) {
        Trace.WriteLine($" crossword {crosswordId} not found");
        Environment.Exit(4);
      }
      Crossword crossword = crosswordRepository.getById(crosswordId);

      // Started?
      if (crossword.startDate is not null) {
        return crossword;
      }

      // New 
      crossword.startDate = DateTime.UtcNow;
      return crossword;
 
    }

    public List<CrosswordHeader> getCrosswordHeaders() {
      return crosswordRepository.getAll()
        .Select( cw => {
            return new CrosswordHeader(){
              puzzleId = cw.id,
              date = cw.published
            };
        }).ToList();
    }

  }
}
