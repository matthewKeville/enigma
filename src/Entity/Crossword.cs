using Model;

namespace Entity {
  public class Crossword : IEntity {

    public int id;
    public DateTime published;
    public DateTime? startDate;
    public TimeSpan? elapsed;
    public DateTime? finishDate;

    //temporary until Model & Entity are properly abstracted
    public CrosswordModel model;

  }
}
