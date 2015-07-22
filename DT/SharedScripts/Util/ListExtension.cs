using System;
using System.Collections;
using System.Collections.Generic;

namespace DT {
  public static class ListExtension {
    static Random rand = new Random();
    
    public static T PickRandom<T>(this IList<T> source) {
        return source[ListExtension.rand.Next(source.Count)];
    }
  }
}