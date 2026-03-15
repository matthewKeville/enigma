using System.Text;
using Serilog;

namespace Logging {

public class TeeTextWriter : TextWriter
{
  
    public enum StreamType {
      OUT,
      ERR,
    }

    private readonly TextWriter _original;
    private readonly Serilog.ILogger _logger;
    private readonly StreamType _streamType;

    public TeeTextWriter(TextWriter original, ILogger logger, StreamType streamType)
    {
        _original = original;
        _logger = logger;
        _streamType = streamType;
    }

    public override Encoding Encoding => _original.Encoding;

    public override void WriteLine(string? value)
    {
        _original.WriteLine(value);

        switch (_streamType)
        {
            case StreamType.OUT: _logger.Information(value ?? ""); break;
            case StreamType.ERR: _logger.Error(value ?? ""); break;
        }
    }

    public override void Write(char value)
    {
        _original.Write(value);
        switch (_streamType)
        {
            case StreamType.OUT: _logger.Information(""+value); break;
            case StreamType.ERR: _logger.Error(""+value); break;
        }
    }

    public override void Write(string? value)
    {
        _original.Write(value);
        switch (_streamType)
        {
            case StreamType.OUT: _logger.Information(value ?? ""); break;
            case StreamType.ERR: _logger.Error(value ?? ""); break;
        }
    }
}

}
