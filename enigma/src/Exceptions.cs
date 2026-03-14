namespace Exceptions {

  /// <summary>
  /// Base exception for enigma domain
  /// </summary>
  public class EnigmaException : Exception
  {
    public EnigmaException() {}
    public EnigmaException(string message) 
      : base(message) {}
    public EnigmaException(string message,Exception inner) 
      : base(message,inner) {}
  }

}

