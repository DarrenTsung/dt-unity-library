using DT;
using InControl;
using System.Collections;
﻿using UnityEngine;

#if IN_CONTROL
public class PlayerActions : PlayerActionSet {
	public PlayerAction Left;
	public PlayerAction Right;
	
	public PlayerAction Up;
	public PlayerAction Down;
	
	public PlayerTwoAxisAction Direction;
	
	public PlayerActions() {
		this.Left = this.CreatePlayerAction("Move Left");
		this.Right = this.CreatePlayerAction("Move Right");
		this.Up = this.CreatePlayerAction("Move Up");
		this.Down = this.CreatePlayerAction("Move Down");
		
		this.Direction = this.CreateTwoAxisPlayerAction(this.Left, this.Right, this.Down, this.Up);
	}
	
	// PRAGMA MARK - INTERNAL
}
#endif