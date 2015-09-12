using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DT.FiniteStateMachine {
	public abstract class FiniteStateMachineImplementation<T> : MonoBehaviour where T : FiniteStateMachineImplementation<T> {
		// PRAGMA MARK - INTERFACE
		public virtual void HandleStateMachineStateChanged(FSMState<T> currentState) {}
		
		// PRAGMA MARK - INTERNAL 
		[SerializeField, ReadOnly]
		protected string _currentStateName;
		[SerializeField, ReadOnly]
		protected float _elapsedTimeInCurrentState;
    protected FiniteStateMachine<T> _stateMachine;
    
    protected virtual void Start() {
      this.SetupStateMachine();
    }
    
    protected abstract void SetupStateMachine();

  	protected virtual void Update() {
      this.ChangeStateIfNeeded();
  		_stateMachine.Update(Time.deltaTime);
			this.UpdateExposedVariablesForInspector();
  	}
		
		protected virtual void UpdateExposedVariablesForInspector() {
			_currentStateName = _stateMachine.CurrentState.GetType().ToString();
			_elapsedTimeInCurrentState = _stateMachine.ElapsedTimeInCurrentState;
		}
  	
  	protected virtual void FixedUpdate() {
  		_stateMachine.FixedUpdate(Time.fixedDeltaTime);
  	}
    
    protected virtual void ChangeStateIfNeeded() {}
  }
}
