using Terminal.Gui;

namespace UI.KeyMapping {

  public class KeySequenceInterpreter {
    private List<Key> _keyBuffer = new ();
    private float _autoFlushTimeMS = 2000;
    private DateTime _lastProcessTime = DateTime.UtcNow;
    public List<(List<Key>,UICommand)> keyMaps;

    public void DumpSequences() {
      this.keyMaps.ForEach( km => dumpSequence(km.Item1));
    }

    public KeySequenceInterpreter(List<(List<Key>,UICommand)> keyMaps) {
      this.keyMaps = keyMaps;
    }

    public KeySequenceInterpreter(List<(List<Key>,UICommand)> keyMaps, float flushTime) {
      this.keyMaps = keyMaps;
      this._autoFlushTimeMS = flushTime;
    }

    public (bool partialMatch, UICommand? command) ProcessKey(Key key) {

      // dump expired keys

      if ( DateTime.UtcNow > _lastProcessTime.AddMilliseconds(_autoFlushTimeMS) ) {
        Trace.WriteLine($"flush time exceeded, clearing key buffer : {_keyBuffer.Count()} keys flushed");
        _keyBuffer.Clear();
      }

      _lastProcessTime = DateTime.UtcNow;

      // try match key sequence

      _keyBuffer.Add(key);

      List<(List<Key>,UICommand)> partialMatches = keyMaps
        .FindAll( km => { return km.Item1.Count() >= _keyBuffer.Count(); } )
        .FindAll( km => {
          for ( int i = 0; i < _keyBuffer.Count(); i++ ) {
            if ( !km.Item1[i].Equals(_keyBuffer[i])) {
              return false;
            }
          }
          return true;
        });

      if (partialMatches.Count() == 0) {
        Debug.WriteLine("no command matches");
        dumpSequence(_keyBuffer);
        _keyBuffer.Clear();
        return (false,null);
      }

      //exact match?
      List<(List<Key>,UICommand)> exactMatches = partialMatches
        .FindAll( km => { return km.Item1.Count() == _keyBuffer.Count(); });

      if (exactMatches.Any()) {

        //Debug.WriteLine("hit keysequence matches " + exactMatches.Count());
        //exactMatches.ForEach( m => dumpSequence(m.Item1));

        _keyBuffer.Clear();
        return (true,exactMatches[0].Item2);
      }

      return (true,null);

    }

    private void dumpSequence( List<Key> sequence ) {
      String msg = "";
      sequence.ForEach( key => {
        msg += key.ToString() + " , ";
      });
      Debug.WriteLine(msg);
    }
  }

}
