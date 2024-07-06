using Entity;
using Enums;

namespace Services.CrosswordFinder.Debug {
  public class DebugFinder : CrosswordFinder {

    private CrosswordService crosswordService;
    private NYDebugCrosswordGenerator dbgGenerator;

    public DebugFinder(CrosswordService crosswordService,NYDebugCrosswordGenerator dbgGenerator) {
      this.crosswordService = crosswordService;
      this.dbgGenerator = dbgGenerator;
    }

    public void Search() {

      Trace.WriteLine("finding NY dbg crossword puzzles");

      DateTime dbg1 = DateTime.Parse("03/21/2021");
      DateTime dbg2 = DateTime.Parse("04/03/2010");

      if ( !crosswordService.HasNYTCrossword(dbg1) ) {
        Crossword cross = dbgGenerator.Sample1();
        cross.Published = DateTime.Parse("03/21/2021");
        cross.Type = CrosswordType.NYTIMES;
        crosswordService.AddCrossword(cross);
      }

      if ( !crosswordService.HasNYTCrossword(dbg2) ) {
        Crossword cross = dbgGenerator.Sample2();
        cross.Published = DateTime.Parse("04/03/2010");
        cross.Type = CrosswordType.NYTIMES;
        crosswordService.AddCrossword(cross);
      }

      Trace.WriteLine("finding NY dbg crossword complete");


    }
  }
}
