
namespace Services.CommandServices {

  /// <summary>
  /// Base exception for CommandServices
  /// </summary>
  public class CommandServiceException : ServiceException
  {
    public CommandServiceException() {}
    public CommandServiceException(string message) 
      : base(message) {}
    public CommandServiceException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// Command args are invalid
  /// </summary>
  public class BadArgsException : CommandServiceException
  {
    public BadArgsException() {}
    public BadArgsException(string message) 
      : base(message) {}
    public BadArgsException(string message,Exception inner) 
      : base(message,inner) {}
  }

  namespace StartService {

    /// <summary>
    /// Crossword not found
    /// </summary>
    public class CrosswordNotFoundException : CommandServiceException
    {
      public CrosswordNotFoundException() {}
      public CrosswordNotFoundException(string message) 
        : base(message) {}
      public CrosswordNotFoundException(string message,Exception inner) 
        : base(message,inner) {}
    }

  }



}

