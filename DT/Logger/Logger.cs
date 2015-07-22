using System.Collections;
using System.Collections.Generic;

namespace DT {
  /// <summary>
  /// Service-locator design pattern
  /// </summary>
  public abstract class Logger {
    abstract public void Log(string input);
    abstract public void LogWarning(string input);
    abstract public void LogError(string input);
  }
}