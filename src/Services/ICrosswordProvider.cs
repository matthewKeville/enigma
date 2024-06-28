using Model;
using System.ComponentModel;
namespace Services {
  public interface ICrosswordProvider : INotifyPropertyChanged {
    public CrosswordModel crossword { get; set; }
  }
}
