using Model;
using System.ComponentModel;
namespace Services {
  public interface ICrosswordProvider : INotifyPropertyChanged {
    public Crossword crossword { get; set; }
  }
}
