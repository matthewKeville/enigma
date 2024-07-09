namespace UI.Model.Browser {

  public class InstallerModel : IModel {

    public static String flag = "InstallModel";

    public int selection = 0;
    public int page = 0;
    public int datesPerPage = 30;

    public DateOnly earliestDate = DateOnly.Parse("03/21/2021");
    public List<DateOnly> blackList = new List<DateOnly>();
    public List<DateOnly> pageDates = new List<DateOnly>();

    public InstallerModel() {
      DateOnly date = DateOnly.FromDateTime(DateTime.UtcNow);
      while ( date > earliestDate && pageDates.Count() != datesPerPage ) {
        if ( !blackList.Contains(date) ) {
          pageDates.Add(date);
        }
        date = date.AddDays(-1);
      }
    }

    private void changePage(bool down) {

      lock(flag) {

        if (down) {

          DateOnly date = pageDates.Last();
          List<DateOnly> newPageDates = new List<DateOnly>();

          while ( date > earliestDate && newPageDates.Count() != datesPerPage ) {
            if ( !blackList.Contains(date) ) {
              newPageDates.Add(date);
            }
            date = date.AddDays(-1);
          }

          page++;
          pageDates = newPageDates;

        } else {

          DateOnly date = pageDates[0];
          List<DateOnly> newPageDates = new List<DateOnly>();

          while ( date < DateOnly.FromDateTime(DateTime.UtcNow) && newPageDates.Count() != datesPerPage ) {
            if ( !blackList.Contains(date) ) {
              newPageDates.Add(date);
            }
            date = date.AddDays(1);
          }
          page--;
          newPageDates.Reverse();
          pageDates = newPageDates;

        }

      }
    }

    public void MoveUp() {
      if ( selection == 0 && page == 0) {
        //no, nothing left
      } else if ( selection == 0 ) {
        changePage(false);
        selection = pageDates.Count()-1;
      } else {
        selection--;
      }
    }

    public void MoveDown() {
      if ( selection == pageDates.Count-1 && pageDates.Last() <= earliestDate ) {
        //no more dates
      } else if ( selection == pageDates.Count()-1 ) {
        changePage(true);
        selection = 0;
      } else {
        selection++;
      }
    }

    public DateOnly GetActiveDate() {
      return pageDates[selection];
    }


  }
}
