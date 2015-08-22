using UnityEngine;
using UnityEngine.Events;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace DT.FiniteStateMachine {
	public abstract class FSMState<T> where T : FiniteStateMachineImplementation<T> {
    public FSMState() {}
    
    public void SetupWithMachineAndContext(FiniteStateMachine<T> machine, T context) {
      _machine = machine;
      _context = context;
      this.InitializeAfterSetup();
    }
    
		public abstract bool CheckIfShouldExit();
		
		public bool CheckIfShouldExitFromFiniteTime() {
			if (_isFiniteTimeState) {
				return _machine.ElapsedTimeInCurrentState > _chosenRandomTimeToStayInState;
			}
			return false;
		}
		
    public virtual bool CheckIfShouldTakeoverCurrentState() {
			return false;
		}
		
    public virtual void Enter() {
			if (_isFiniteTimeState) {
				_chosenRandomTimeToStayInState = UnityEngine.Random.Range(_minTime, _maxTime);
			}
		}
		
    public virtual void Update(float deltaTime) {}
    public virtual void FixedUpdate(float fixedDeltaTime) {}
    public virtual void Exit() {}
		
		public virtual void SetMinMaxTime(float min, float max) {
			_minTime = min;
			_maxTime = max;
			_isFiniteTimeState = true;
		}
		
		public bool IsFinite {
			get { return _isFiniteTimeState; }
		}
    
		// PRAGMA MARK - INTERFACE 
    protected FiniteStateMachine<T> _machine;
    protected T _context;
		
		protected float _minTime;
		protected float _maxTime;
		protected bool _isFiniteTimeState;
		protected float _chosenRandomTimeToStayInState;
    
    protected virtual void InitializeAfterSetup() {}
	}
}