using System.Collections;
using UnityEngine;

namespace DT {
  public static class Vector2Extensions {
    public static Vector2 SetXY(this Vector2 v, float x, float y) {
      v.x = x;
      v.y = y;
      return v;
    }
    
    public static Vector2 SetX(this Vector2 v, float x) {
      v.x = x;
      return v;
    }
    
    public static Vector2 SetY(this Vector2 v, float y) {
      v.y = y;
      return v;
    }
    
    public static Vector2 AddX(this Vector2 v, float x) {
      v.x = v.x + x;
      return v;
    }
    
    public static Vector2 AddY(this Vector2 v, float y) {
      v.y = v.y + y;
      return v;
    }
    
    public static Vector2 PerpendicularDirection(this Vector2 v) {
      Vector2 p = new Vector2(v.y, v.x);
      return p.normalized;
    }
  }
}
