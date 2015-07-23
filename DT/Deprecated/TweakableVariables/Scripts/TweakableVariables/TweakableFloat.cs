using UnityEngine;
using System.Collections;

namespace DT.TweakableVariables {
	public class TweakableFloat : TweakableVariable<float> {
		public TweakableFloat(string name, float min, float max, float start) : base(name, min, max, start) {
		}
		
		override protected void RegisterWithManager() {
			TweakableVariableManager.Instance.Register(this);
		}
	}
}
