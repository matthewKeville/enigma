using Entity;
using Repository;
using UI.Model.Browser;

namespace Services {
  public class CrosswordService {

    private CrosswordRepository crosswordRepository;

    public CrosswordService(CrosswordRepository crosswordRepository) {
      this.crosswordRepository = crosswordRepository;
    }

    public Crossword GetCrossword(int crosswordId) {

      if (!crosswordRepository.Has(crosswordId)) {
        Trace.WriteLine($" crossword {crosswordId} not found");
        Environment.Exit(4);
      }
      Crossword crossword = crosswordRepository.GetById(crosswordId);

      // Started?
      if (crossword.startDate is not null) {
        return crossword;
      }

      // New 
      crossword.startDate = DateTime.UtcNow;
      return crossword;
 
    }

    public List<CrosswordHeader> GetCrosswordHeaders() {
      return crosswordRepository.GetAll()
        .Select( cw => {
            return new CrosswordHeader(){
              puzzleId = cw.id,
              date = cw.published
            };
        }).ToList();
    }

  }
}
