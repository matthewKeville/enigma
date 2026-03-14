using Exceptions;

namespace Services {

  /// <summary>
  /// Base exception for service layer
  /// </summary>
  public class ServiceException : EnigmaException
  {
    public ServiceException() {}
    public ServiceException(string message) 
      : base(message) {}
    public ServiceException(string message,Exception inner) 
      : base(message,inner) {}
  }

}

