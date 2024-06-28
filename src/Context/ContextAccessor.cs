using System.Diagnostics;

namespace Context {

  public class ContextAccessor {

    private ApplicationContext context;

    public void setContext(ApplicationContext context) {
      this.context = context;
    }

    public ApplicationContext getContext() {
      if ( context is null ) {
        Trace.WriteLine("no context");
        Environment.Exit(1);
      }
      return this.context;
    }
  }

}
