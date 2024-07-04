using Entity;
using Services;

namespace Repository {

  public class CrosswordRepository : IRepository<Crossword> {

    private Dictionary<int,Crossword> crosswords;

    public CrosswordRepository() {

      crosswords = new Dictionary<int, Crossword>();

      NYDebugCrosswordGenerator gen = new NYDebugCrosswordGenerator();

      Crossword crossword0 = new Crossword(){
        id=0,
        model=gen.Sample1(),
        published = DateTime.Parse("03/21/2021")
      };

      Crossword crossword1 = new Crossword(){
        id=1,
        model=gen.Sample2(),
        published = DateTime.Parse("04/03/2010")
      };

      crosswords[0] = crossword0;
      crosswords[1] = crossword1;

    }

    public Crossword GetById(int id) {
      if ( !crosswords.ContainsKey(id) ) {
        Trace.WriteLine($"no crossword with id {id}");
        Environment.Exit(1);
        return null;
      }
      return crosswords[id];
    }

    public List<Crossword> GetAll() {
      return crosswords.Values.ToList();
    }

    public Crossword Save(Crossword crossword){
      Trace.WriteLine("crossword save not implemented");
      Environment.Exit(1);
      return null;
    }

    public bool Has(int id) {
      return crosswords.ContainsKey(id);
    }
  }
}
