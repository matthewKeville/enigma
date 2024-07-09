using System.Net;
using Entity;
using Enums;

namespace Services.CrosswordInstaller.NYT {

  public class NYTCrosswordInstaller {

    private static HttpClient httpClient = new HttpClient();
    private const String baseUrl = "https://nytsyn.pzzl.com/nytsyn-crossword-mh/nytsyncrossword";

    private NYTCrosswordParser parser;

    public NYTCrosswordInstaller(NYTCrosswordParser parser) {
      this.parser = parser;
    }

    private void Fail(InstallationRequest request,String reason) {
      Trace.WriteLine($"download failed : {reason}");
      request.Status = InstallationRequestStatus.FAILED;
      return;
    }

    public async Task<Crossword?> BuildCrossword(InstallationRequest request) {

      await Task.Delay(1000);

      NYTInstallationRequestArgs args = ((NYTInstallationRequestArgs)request.Args);

      DateOnly date = args.RealDate;

      String dayStr = date.Day.ToString().PadLeft(2,'0');
      String monthStr = date.Month.ToString().PadLeft(2,'0');
      String yearStr = date.Year.ToString().Substring(2);
      String dateStr = $"?date={yearStr}{monthStr}{dayStr}";

      String url = baseUrl + dateStr;

      Trace.WriteLine($"downloading puzzle : {url}");
      request.Status = InstallationRequestStatus.DOWNLOADING;

      HttpResponseMessage response = await httpClient.GetAsync(url);

      if ( response.StatusCode != HttpStatusCode.OK ) {
        Fail(request,"Bad Return Code");
        return null;
      }

      Trace.WriteLine($"parsing puzzle");
      request.Status = InstallationRequestStatus.INSTALLING;
      StringReader reader = new StringReader(await response.Content.ReadAsStringAsync());

      if ( ! reader.ReadLine().Equals("ARCHIVE") ) {
        Fail(request,"Bad Format");
        return null;
      }

      reader.ReadLine(); //blank line

      //url date and puzzle date are offset : TODO
      reader.ReadLine(); // date line
      /**
      if ( ! reader.ReadLine().Equals(dateString) ) {
        Fail(request,"Date checksum mismatch");
      }
      */

      reader.ReadLine(); // blank line
      String bDateStr = reader.ReadLine();

      reader.ReadLine(); // blank line
      String bAuthorStr = reader.ReadLine();

      reader.ReadLine(); // blank line
      int bRows = int.Parse(reader.ReadLine());
      reader.ReadLine(); // blank line
      int bColumns = int.Parse(reader.ReadLine());
      reader.ReadLine(); // blank line
      int bAcrossCount = int.Parse(reader.ReadLine());
      reader.ReadLine(); // blank line
      int bDownCount = int.Parse(reader.ReadLine());

      reader.ReadLine(); // blank line
      //solution
      List<String> bSolution = new List<String>();
      String solutionRow = reader.ReadLine();
      while ( solutionRow.Count() != 0 ) {
        bSolution.Add(solutionRow);
        Trace.WriteLine(solutionRow);
        solutionRow = reader.ReadLine();
      }

      //across clues
      List<String> bAcrossClues = new List<String>();
      String clueRowAcross = reader.ReadLine();
      while ( clueRowAcross.Count() != 0 ) {
        bAcrossClues.Add(clueRowAcross);
        Trace.WriteLine(clueRowAcross);
        clueRowAcross = reader.ReadLine();
      }
      
      //down clues
      List<String> bDownClues = new List<String>();
      String clueRowDown = reader.ReadLine();
      while ( clueRowDown.Count() != 0 ) {
        bDownClues.Add(clueRowDown);
        Trace.WriteLine(clueRowDown);
        clueRowDown = reader.ReadLine();
      }

      try {
        return parser.Parse(bRows,bColumns,bSolution,bAcrossClues,bDownClues);
      } catch (Exception ex) {
        Trace.WriteLine($" Unexpected error parsing NYT Crossword : URL {url}");
        Trace.WriteLine(ex.ToString());
        return null;
      }

    }

  }
  
}
