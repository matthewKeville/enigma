namespace Services.CrosswordInstaller {

  public class InstallationRequest {}

  public class CrosswordInstallerService {

    private DatabaseContext _dbCtx;

    public CrosswordInstallerService(DatabaseContext dbCtx) {
      this._dbCtx = dbCtx;
    }

    public void Install(InstallationRequest request) {
      Trace.WriteLine("processing installation request");
    }
  }
}
