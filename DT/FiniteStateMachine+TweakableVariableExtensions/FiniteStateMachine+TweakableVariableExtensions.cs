using UnityEngine;
using System.Collections;
using DT.FiniteStateMachine;
using DT.TweakableVariables;

namespace DT.FiniteStateMachine.TweakableVariableExtensions {
	public class TweakableState : State {
		protected TweakableFloat _min, _max;
		protected float Min {
			get { return _min.Value; }
		}
		protected float Max {
			get { return _max.Value; }
		}
		
		public TweakableState(string stateId) : base(stateId) {
			_min = null;
			_max = null;
		}
		
		public void SetTweakableMinMaxTime(TweakableFloat min, TweakableFloat max) {
			_min = min;
			_max = max;
			
			if (Min < 0.0f && Max < 0.0f) {
				isFinite = false;
			}
		}
		
		public override float GenerateTime() {
			return Random.Range(Min, Max);
		}
	}
	
	public class TweakableFiniteStateMachine : FiniteStateMachine {
		protected const float DEFAULT_VARIANCE = 5.0f; 
		
		public override void AddState(string stateId, float minTime, float maxTime) {
			TweakableState newState = new TweakableState(stateId);
			
			TweakableFloat minVariable = new TweakableFloat(stateId + ":minTime", Mathf.Max(minTime - DEFAULT_VARIANCE, 0.0f), minTime + DEFAULT_VARIANCE, minTime);
			TweakableFloat maxVariable = new TweakableFloat(stateId + ":maxTime", Mathf.Max(maxTime - DEFAULT_VARIANCE, 0.0f), maxTime + DEFAULT_VARIANCE, maxTime);
			newState.SetTweakableMinMaxTime(minVariable, maxVariable);
			
			stateHash.Add(newState.GetId(), newState);
		}
	}
}