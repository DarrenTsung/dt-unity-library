using UnityEngine;
using System.Collections;

namespace DT.TweakableVariables {
	abstract public class TweakableVariable<T> {
		// PRAGMA MARK - CLASS
		protected string _name;
		public string Name {
			get { return _name; }
		}
		
		protected T _min, _max;
		public T Min {
			get { return _min; }
		}
		public T Max {
			get { return _max; }
		}
		
		protected T _value;
		public T Value {
			get { return _value; }
			set { _value = value; }
		}
		
		public TweakableVariable() {
			_name = "default";
		}
		
		public TweakableVariable(string name, T min, T max, T start) {
			_name = name;
			_min = min;
			_max = max;
			_value = start;
			
			RegisterWithManager();
		}
		
		abstract protected void RegisterWithManager();
	}
}