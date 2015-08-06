using System.Collections;
using UnityEngine;

namespace DT {
  public static class Vector2Extensions {
    public static Vector2 ChangeXY (this Vector2 v, float x, float y) {
      v.x = x;
      v.y = y;
      return v;
    }
    
    public static Vector2 ChangeX (this Vector2 v, float x) {
      v.x = x;
      return v;
    }
    
    public static Vector2 ChangeY (this Vector2 v, float y) {
      v.y = y;
      return v;
    }
  }
}
