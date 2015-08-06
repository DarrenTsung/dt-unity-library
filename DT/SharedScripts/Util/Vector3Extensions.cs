using System.Collections;
using UnityEngine;

namespace DT {
  public static class Vector3Extensions {
    public static Vector3 ChangeXYZ (this Vector3 v, float x, float y, float z) {
      v.x = x;
      v.y = y;
      v.z = z;
      return v;
    }
    
    public static Vector3 ChangeXY (this Vector3 v, float x, float y) {
      v.x = x;
      v.y = y;
      return v;
    }
    
    public static Vector3 ChangeXZ (this Vector3 v, float x, float z) {
      v.x = x;
      v.z = z;
      return v;
    }
    
    public static Vector3 ChangeYZ (this Vector3 v, float y, float z) {
      v.y = y;
      v.z = z;
      return v;
    }
    
    public static Vector3 ChangeX (this Vector3 v, float x) {
      v.x = x;
      return v;
    }
    
    public static Vector3 ChangeY (this Vector3 v, float y) {
      v.y = y;
      return v;
    }
    
    public static Vector3 ChangeZ (this Vector3 v, float z) {
      v.z = z;
      return v;
    }
  }
}
