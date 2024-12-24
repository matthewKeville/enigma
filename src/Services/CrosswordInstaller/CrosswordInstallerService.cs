using Entity;
using Enums;
using Services.CrosswordInstaller.NYT;

namespace Services.CrosswordInstaller {

  public abstract class InstallationRequest {}

  public class NYTInstallationRequest : InstallationRequest {
    public DateOnly Date;
  }

  public class CrosswordInstallerService {

    private DatabaseContext _dbCtx;
    private NYTCrosswordFetcher _nytFetcher;

    public CrosswordInstallerService(DatabaseContext dbCtx,NYTCrosswordFetcher nytFetcher) {
      this._dbCtx = dbCtx;
      this._nytFetcher = nytFetcher;
    }

    public void Install(InstallationRequest request) {

      Trace.WriteLine("processing installation request");

      if ( request is NYTInstallationRequest ) {
          NYTInstallationRequest nytRequest = (NYTInstallationRequest) request;
          Crossword? crossword = _nytFetcher.Fetch(nytRequest.Date).GetAwaiter().GetResult();
          if ( crossword is not null ) {
            _dbCtx.Crosswords.Add(crossword);
            _dbCtx.SaveChanges();
          }
      } else {
        //pass
      }

    }
  }
}
