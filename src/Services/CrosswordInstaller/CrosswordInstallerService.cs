using Entity;
using Enums;
using Services.CrosswordInstaller.NYT;

namespace Services.CrosswordInstaller {

  public class NYTInstallationRequestArgs : InstallationRequestArgs {
    public DateOnly RealDate;     //the date set in the API request
  }

  public interface InstallationRequestArgs {}
  public delegate void OnInstallationSuccess();

  public class InstallationRequest() {
    public InstallationRequestStatus Status;
    public InstallationRequestArgs Args;
    public OnInstallationSuccess OnSuccess;
  }

  public class CrosswordInstallerService {

    private CrosswordService crosswordService;
    private NYTCrosswordInstaller nytCrosswordInstaller;

    public CrosswordInstallerService(CrosswordService crosswordService,NYTCrosswordInstaller nytCrosswordInstaller) {
      this.nytCrosswordInstaller = nytCrosswordInstaller;
      this.crosswordService = crosswordService;
    }

    public async Task InstallPuzle(InstallationRequest request) {
      if ( request.Args.GetType() == typeof(NYTInstallationRequestArgs)) {
          Crossword? crossword = await nytCrosswordInstaller.BuildCrossword(request);
          if ( crossword is not null ) {
            crosswordService.AddCrossword(crossword);
            request.Status = InstallationRequestStatus.COMPLETE;
            request.OnSuccess();
          } else {
            request.Status = InstallationRequestStatus.FAILED;
          }
      }
    }
  }
}
