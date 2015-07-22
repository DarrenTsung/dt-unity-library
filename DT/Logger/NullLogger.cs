using System.Collections;
using System.Collections.Generic;

namespace DT {
  /// <summary>
  /// Empty logger, does nothing
  /// </summary>
  public class NullLogger : Logger {
    public override void Log(string input) {
      
    }
    
    public override void LogWarning(string input) {
      
    }
    
    public override void LogError(string input) {
      
    }
  }
}