using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT {
  /// <summary>
  /// Logger for the UnityEngine
  /// </summary>
  public class UnityLogger : Logger {
    public override void Log(string input) {
      Debug.Log(input);
    }
    
    public override void LogWarning(string input) {
      Debug.LogWarning(input);
    }
    
    public override void LogError(string input) {
      Debug.LogError(input);
    }
  }
}