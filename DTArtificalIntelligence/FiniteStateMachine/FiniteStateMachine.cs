using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DT.FiniteStateMachine {
  public struct TransitionMap {
    public List<System.Type> PossibleStatesToTransitionTo;
    public List<System.Type> PossibleStatesToTransitionFrom;
  }
  
	/*
	 * Example usage:
	 * (Note: All the credit goes towards StateKit [https://github.com/prime31/StateKit], awesome stuff!)
   */
	public class FiniteStateMachine<T> where T : FiniteStateMachineImplementation<T> {
		// PRAGMA MARK - INTERFACE
    public FiniteStateMachine(T context, FSMState<T> initialState) {
      _context = context;
      
      this.AddState(initialState);
      _currentState = initialState;
      _currentState.Enter();
    }
    
    public void AddState(FSMState<T> state) {
      state.SetupWithMachineAndContext(this, _context);
      _states[state.GetType()] = state;
      
      TransitionMap transitionMap = new TransitionMap();
      transitionMap.PossibleStatesToTransitionTo = new List<System.Type>();
      transitionMap.PossibleStatesToTransitionFrom = new List<System.Type>();
      _transitions[state.GetType()] = transitionMap;
    }
    
    public void AddTransition(FSMState<T> state, FSMState<T> otherState) {
      System.Type stateType = state.GetType();
      System.Type otherStateType = otherState.GetType();
      if (!_transitions.ContainsKey(stateType) || !_transitions.ContainsKey(otherStateType)) {
        Debug.LogError("FiniteStateMachine::AddTransition - Invalid state types!");
        return;
      }
      
      TransitionMap transitionMap = _transitions[stateType];
      TransitionMap otherTransitionMap = _transitions[otherStateType];
      if (transitionMap.PossibleStatesToTransitionTo.Contains(otherStateType) || otherTransitionMap.PossibleStatesToTransitionFrom.Contains(stateType)) {
        Debug.LogError("FiniteStateMachine::AddTransition - State already found inside possible states to transition to!");
        return;
      }
      
      transitionMap.PossibleStatesToTransitionTo.Add(otherStateType);
      otherTransitionMap.PossibleStatesToTransitionFrom.Add(stateType);
    }
    
    public void ChangeState<R>() where R : FSMState<T> {
      System.Type newType = typeof(R);
      if (_currentState.GetType() == newType) {
        return;
      }
      
      #if UNITY_EDITOR
			// do a sanity check while in the editor to ensure we have the given state in our state list
			if(!_states.ContainsKey(newType)) {
				string error = GetType() + ": state " + newType + " does not exist. Did you forget to add it by calling addState?";
        Debug.LogError(error);
				throw new Exception(error);
			}
			#endif  
      
      FSMState<T> newState = _states[newType];
      this.ChangeState(newState);
    }
    
    public FSMState<T> CurrentState {
      get { return _currentState; }
    }
    
    public bool IsCurrentState<R>() {
      return _currentState.GetType() == typeof(R);
    }
    
    public void Update(float deltaTime) {
      this.CheckPossibleNextStates();
      _currentState.Update(deltaTime);
      _elapsedTimeInCurrentState += deltaTime;
      
      bool shouldExit = _currentState.CheckIfShouldExit();
      bool shouldExitFromTime = _currentState.CheckIfShouldExitFromFiniteTime();
      if (shouldExit || shouldExitFromTime) {
        TransitionMap currentTransitionMap = _transitions[_currentState.GetType()];
        if (currentTransitionMap.PossibleStatesToTransitionTo.Count <= 0) {
          Debug.LogError("FiniteStateMachine::Update - current state attempting to exit, missing transitions to exit to.");
          return;
        }
        
        List<System.Type> possibleStatesToTransitionTo = currentTransitionMap.PossibleStatesToTransitionTo;
        if (shouldExitFromTime) {
          possibleStatesToTransitionTo = possibleStatesToTransitionTo.Where(type => _states[type].IsFinite).ToList();
        }
        
        System.Type nextStateType = possibleStatesToTransitionTo.PickRandom();
        FSMState<T> nextState = _states[nextStateType];
        this.ChangeState(nextState);
      }
    }
    
    public void FixedUpdate(float fixedDeltaTime) {
      _currentState.FixedUpdate(fixedDeltaTime);
    }
    
    public float ElapsedTimeInCurrentState {
      get { return _elapsedTimeInCurrentState; }
    }
    
    // PRAGMA MARK - INTERNAL
    protected T _context;
    
    protected FSMState<T> _previousState;
    
    protected float _elapsedTimeInCurrentState;
    protected FSMState<T> _currentState;
    
    protected Dictionary<System.Type, FSMState<T>> _states = new Dictionary<System.Type, FSMState<T>>();
    protected Dictionary<System.Type, TransitionMap> _transitions = new Dictionary<System.Type, TransitionMap>();
    
    protected void ChangeState(FSMState<T> newState) {
      if (_currentState != null) {
        _currentState.Exit();
      }
      
      _previousState = _currentState;
      _currentState = newState;
      _elapsedTimeInCurrentState = 0.0f;
      _currentState.Enter();
      
      _context.HandleStateMachineStateChanged(_currentState);
    }
    
    protected bool CheckPossibleNextStates() {
      TransitionMap currentTransitionMap = _transitions[_currentState.GetType()];
      foreach (System.Type stateType in currentTransitionMap.PossibleStatesToTransitionTo) {
        FSMState<T> state = _states[stateType];
        bool shouldTakeover = state.CheckIfShouldTakeoverCurrentState();
        if (shouldTakeover) {
          this.ChangeState(state);
          return true;
        }
      }
      return false;
    }
  }
}
