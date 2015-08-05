using System.Collections;
using UnityEngine;

namespace DT {
  public static class Vector3Extensions {
    public static Vector3 changeXYZ (this Vector3 v, float x, float y, float z) {
      v.x = x;
      v.y = y;
      v.z = z;
      return v;
    }
    
    public static Vector3 changeXY (this Vector3 v, float x, float y) {
      v.x = x;
      v.y = y;
      return v;
    }
    
    public static Vector3 changeXZ (this Vector3 v, float x, float z) {
      v.x = x;
      v.z = z;
      return v;
    }
    
    public static Vector3 changeYZ (this Vector3 v, float y, float z) {
      v.y = y;
      v.z = z;
      return v;
    }
    
    public static Vector3 changeX (this Vector3 v, float x) {
      v.x = x;
      return v;
    }
    
    public static Vector3 changeY (this Vector3 v, float y) {
      v.y = y;
      return v;
    }
    
    public static Vector3 changeZ (this Vector3 v, float z) {
      v.z = z;
      return v;
    }
  }
}
