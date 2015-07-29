using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DT.FiniteStateMachine {
	public class StateTransition<TEnum> {
		protected State<TEnum> _destinationState;
		public State<TEnum> DestinationState {
			get { return _destinationState; }
		}
		protected int _weight;
		public int Weight {
			get { return _weight; }
		}

		public StateTransition (State<TEnum> destinationState, int weight) {
			_destinationState = destinationState;
			_weight = weight;
		}
	}



	public class State<TEnum> {
		// PRAGMA MARK - INTERFACE 
		public string TransitionToNextStateKey(State<TEnum> stateNext) {
			return this.TransitionToNextStateIdKey(stateNext.Id);
		}
		
		public string TransitionToNextStateIdKey(TEnum nextStateId) {
			return String.Format("Handle{0}To{1}", _id.ToString(), nextStateId.ToString());
		}
		
		public string EnterKey() {
			return String.Format("{0}Enter", _id.ToString());
		}
		
		public string TickKey() {
			return String.Format("{0}Tick", _id.ToString());
		}
		
		public string ExitKey() {
			return String.Format("{0}Exit", _id.ToString());
		}
		
		public void SetMinMaxTime(float min, float max) {
			_minTime = min;
			_maxTime = max;

			if (min < 0.0f && max < 0.0f) {
				_isFinite = false;
			}
		}

		public bool CanTransitionWithTime() {
			return _isFinite;
		}

		public void AddTransition(StateTransition<TEnum> transition) {
			_transitions.Add(transition);
			_cumulativeWeight += transition.Weight;
		}

		public State<TEnum> ChooseNextState() {
			if (_transitions.Count <= 0) {
				Locator.Logger.LogError("ChooseNextState() called when there are no states to transition to");
				throw new UnityException();
			}

			int chosen = UnityEngine.Random.Range(0, _cumulativeWeight);
			foreach (StateTransition<TEnum> transition in _transitions) {
				chosen -= transition.Weight;
				if (chosen <= 0) {
					return transition.DestinationState;
				}
			}
			return _transitions[0].DestinationState;
		}

		public virtual float GenerateTime() {
			return UnityEngine.Random.Range(_minTime, _maxTime);
		}
		
		protected TEnum _id;
		public TEnum Id {
			get { return _id; }
		}
		
		// PRAGMA MARK - INTERNAL
		protected List<StateTransition<TEnum>> _transitions;
		protected int _cumulativeWeight;
		protected float _minTime, _maxTime;
		protected bool _isFinite;

		public State() {
			_transitions = new List<StateTransition<TEnum>>();
			_cumulativeWeight = 0;
			_minTime = 0.0f;
			_maxTime = 1.0f;
			_isFinite = true;
		}
		
		public State(TEnum id) : this() {
			_id = id;
		}
	}
	
	
	
	/*
	 * Example usage:
	 * (Note: Lots of credit towards StateKit[https://github.com/prime31/StateKit] for the Enum setup!)
	 *
	 *  	using DT.FiniteStateMachine;
	 *  
	 *   	protected CowStateEnum {
	 *   	  Walk,
	 *   		Idle
	 *   	}
	 *   	
	 *   	public class CowStateMachine : FiniteStateMachine<CowStateEnum>() {
	 *   		protected void WalkEnter() {}
	 *   		protected void WalkTick() {}
	 *   		protected void WalkExit() {}
	 *   		
	 *   		protected void HandleWalkToIdle() {}
	 *   		
	 *   		protected void IdleEnter() {}
	 *   		protected void IdleTick() {}
	 *   		protected void IdleExit() {}
	 *   		
	 *   		protected void HandleIdleToWalk() {}
	 *   	}
	 *  
	 *	  Overridable Methods:
	 *		Example states: Walk, Idle
	 *		HandleWalkToIdle - called when the state machine changes from state0 to state1
	 *		WalkEnter 			 - called when the Walk state is entered
	 *		WalkTick 				 - called when the Walk state is updated
	 *		WalkExit 				 - called when the Walk state is exited
	 * */
	public class FiniteStateMachine<TEnum> : MonoBehaviour where TEnum : IConvertible, IComparable, IFormattable {
		// PRAGMA MARK - INTERFACE
		public void AddTransition(TEnum stateIdFrom, TEnum stateIdTo, int weight) {
			if (!_stateHash.ContainsKey(stateIdFrom) || !_stateHash.ContainsKey(stateIdTo)) {
				Locator.Logger.LogWarning("FiniteStateMachine::AddTransition - invalid state; stateFrom: " + stateIdFrom + ". stateTo: " + stateIdTo);
				return;
			}

			State<TEnum> stateFrom = _stateHash[stateIdFrom];
			State<TEnum> stateTo = _stateHash[stateIdTo];

			StateTransition<TEnum> newTransition = new StateTransition<TEnum>(stateTo, weight);
			stateFrom.AddTransition(newTransition);
		}

		public void SetStartState(TEnum startEnumId) {
			if (!_stateHash.ContainsKey(startEnumId)) {
				Locator.Logger.LogWarning("FiniteStateMachine::SetStartState - invalid start state id: " + startEnumId);
			}

			State<TEnum> startState = _stateHash[startEnumId];

			_currentState = startState;
			_remainingTimeInState = _currentState.GenerateTime();
		}
		
		public void ResetWithState(TEnum startStateId) {
			this.SetStartState(startStateId);
		}

		public void AdvanceCurrentState() {
			State<TEnum> nextState = _currentState.ChooseNextState();
			this.TransitionToState(nextState);
		}

		public void ForceTransitionToStateEnum(TEnum stateId) {
			State<TEnum> state = _stateHash[stateId];

			this.TransitionToState(state);
		}

		public TEnum CurrentStateId {
			get { return _currentState.Id; }
		}
		
		// PRAGMA MARK - INTERNAL 
		protected float _remainingTimeInState;
		protected State<TEnum> _currentState;
		
		protected Dictionary<string, Action> _stateActionCache;
		protected Dictionary<TEnum, State<TEnum>> _stateHash;
		
		protected virtual void Awake() {
			if (!typeof(TEnum).IsEnum) {
				Locator.Logger.LogError("FiniteStateMachine::() - TEnum generic constraint not satisfied!");
			}
			
			// cache all of our state methods
			TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
			foreach (TEnum e in enumValues) {
				State<TEnum> state = new State<TEnum>(e);
				_stateHash.Add(state.Id, state);
				this.InitializeState(state, enumValues);
			}
			
			_remainingTimeInState = 0.0f;
			_stateHash = new Dictionary<TEnum, State<TEnum>>();
		}

		protected void Update() {
			_remainingTimeInState -= Time.deltaTime;
			if (_remainingTimeInState <= 0) {
				if (_currentState == null) {
					Locator.Logger.LogError("FiniteStateMachine::Tick - current state null (did you forget to set the start state?)");
				} else {
					if (_currentState.CanTransitionWithTime()) {
						this.AdvanceCurrentState();
					} 
				}
			}
			
			this.CallActionForMethod(_currentState.TickKey());
		}
		
		protected void InitializeState(State<TEnum> state, TEnum[] allEnumValues) {
			foreach (TEnum e in allEnumValues) {
				if (state.Id.Equals(e)) {
					continue;
				}
				
				this.SetupActionForMethod(state.TransitionToNextStateIdKey(e));
			}
			
			this.SetupActionForMethod(state.EnterKey());
			this.SetupActionForMethod(state.TickKey());
			this.SetupActionForMethod(state.ExitKey());
		}
		
		protected void SetupActionForMethod(string methodName) {
			if (_stateActionCache.ContainsKey(methodName)) {
				Locator.Logger.LogError("FiniteStateMachine::SetupActionForMethod - Method " + methodName + " already exists!");
				return;
			}
			
			MethodInfo methodInfo = this.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Instance | 
																																	 System.Reflection.BindingFlags.Public | 
																																	 System.Reflection.BindingFlags.NonPublic);
																												
			if (methodInfo != null) {
				_stateActionCache[methodName] = Delegate.CreateDelegate(typeof(Action), this, methodInfo) as Action;
			}
		}
		
		protected void CallActionForMethod(string methodName) {
			Action action = _stateActionCache[methodName];
			if (action != null) {
				action();
			}
		}
		
		protected void TransitionToState(State<TEnum> nextState) {
			if (nextState == null) {
				Locator.Logger.LogError("FiniteStateMachine::TransitionToState - called with invalid state!");
				return;
			}
			
			// Delegate handling
			this.CallActionForMethod(_currentState.ExitKey());
			this.CallActionForMethod(_currentState.TransitionToNextStateKey(nextState));
			this.CallActionForMethod(nextState.EnterKey());
			
			_currentState = nextState;
			_remainingTimeInState = _currentState.GenerateTime();
		}
	}
}
