
namespace Services.CommandServices.Exceptions {

  /// <summary>
  /// Base exception for CommandServices
  /// </summary>
  public class CommandServiceException : Exception
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

  /// <summary>
  /// Resource not found
  /// </summary>
  public class NotFoundException : CommandServiceException
  {
    public NotFoundException() {}
    public NotFoundException(string message) 
      : base(message) {}
    public NotFoundException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// Crossword not found
  /// </summary>
  public class CrosswordNotFoundException : NotFoundException
  {
    public CrosswordNotFoundException() {}
    public CrosswordNotFoundException(string message) 
      : base(message) {}
    public CrosswordNotFoundException(string message,Exception inner) 
      : base(message,inner) {}
  }

  /// <summary>
  /// Plugin not found
  /// </summary>
  // public class PluginNotFoundException : NotFoundException
  // {
  //   public PluginNotFoundException() {}
  //   public PluginNotFoundException(string message) 
  //     : base(message) {}
  //   public PluginNotFoundException(string message,Exception inner) 
  //     : base(message,inner) {}
  // }

  /// <summary>
  /// An install request failed dued to the fetcher being unable
  /// to resolve the request.
  /// </summary>
  public class FetchFailedException : Exception
  {
    public FetchFailedException() {}
    public FetchFailedException(string message) 
      : base(message) {}
    public FetchFailedException(string message,Exception inner) 
      : base(message,inner) {}
  }


}

