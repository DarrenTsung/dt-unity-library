using System;
using DuoVia.FuzzyStrings;

namespace DT {
	/// <summary>
	/// Contains approximate string matching
	/// </summary>
	static class StringMatch 
	{
	  /// <summary>
	  /// Compute the distance between two strings.
	  /// </summary>
	  public static double ComputeDistance(string s, string t) {
			return s.FuzzyMatch(t);
		}
	}
}