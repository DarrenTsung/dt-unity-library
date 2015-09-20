using DT;
using InControl;
using System.Collections;
﻿using UnityEngine;

#if IN_CONTROL
namespace DT {
	public class PlayerInputManager<TPlayerActions, KPlayer> : Singleton<PlayerInputManager<TPlayerActions, KPlayer>> where TPlayerActions : PlayerActions, new()
	 																																																		where KPlayer : Player {
		protected PlayerInputManager() {}
		
		// PRAGMA MARK - INTERFACE
		public bool InputDisabled {
			get { return _inputDisabled; }
			set {
				_inputDisabled = value;
			}
		}
		
		// PRAGMA MARK - INTERNAL
		[SerializeField]
		protected Player _player;
		protected bool _inputDisabled;
		protected TPlayerActions _playerActions;
		
		protected virtual void Awake() {
			PlayerManager.Instance.OnPlayerChange.AddListener((object playerObject) => { this.SetupWithPlayer(playerObject as GameObject); });
		}
		
		protected virtual void Start() {
			_playerActions = new TPlayerActions();
			this.BindDefaultActions();
		}
		
		protected void SetupWithPlayer(GameObject player) {
			_player = player.GetComponent<KPlayer>();
		}
		
		protected virtual void BindDefaultActions() {
			_playerActions.Left.AddDefaultBinding(Key.A);
			_playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
			
			_playerActions.Right.AddDefaultBinding(Key.D);
			_playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
			
			_playerActions.Up.AddDefaultBinding(Key.W);
			_playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
			
			_playerActions.Down.AddDefaultBinding(Key.S);
			_playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);
		}

		protected void Update() {
			if (_player != null && !this.InputDisabled) {
				this.UpdateInput();
			}
		}
		
		protected virtual void UpdateInput() {
			// LEFT STICK
			Vector2 direction = _playerActions.Direction.Value;
			_player.HandleDirectionVector.Invoke(direction);
		}
	}
}
#endif
