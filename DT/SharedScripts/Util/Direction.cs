using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT {
	public enum Direction {
		UP = 0,
		RIGHT = 1,
		DOWN = 2,
		LEFT = 3
	}

	public static class DirectionExtensions {
		public static Vector2 Vec2Value(this Direction direction) {
			switch (direction) {
				case Direction.UP:
					return Vector2.up;
				case Direction.RIGHT:
					return Vector2.right;
				case Direction.DOWN:
					return -Vector2.up;
				case Direction.LEFT:
					return -Vector2.right;
			}
			return Vector2.up;
		}
		
		public static Direction OppositeDirection(this Direction direction) {
			switch (direction) {
				case Direction.UP:
					return Direction.DOWN;
				case Direction.RIGHT:
					return Direction.LEFT;
				case Direction.DOWN:
					return Direction.UP;
				case Direction.LEFT:
					return Direction.RIGHT;
			}
			return Direction.UP;
		}
		
		public static Direction ClockwiseDirection(this Direction direction) {
			switch (direction) {
				case Direction.UP:
					return Direction.RIGHT;
				case Direction.RIGHT:
					return Direction.DOWN;
				case Direction.DOWN:
					return Direction.LEFT;
				case Direction.LEFT:
					return Direction.UP;
			}
			return Direction.UP;
		}
		
		public static Direction CounterClockwiseDirection(this Direction direction) {
			switch (direction) {
				case Direction.UP:
					return Direction.LEFT;
				case Direction.RIGHT:
					return Direction.UP;
				case Direction.DOWN:
					return Direction.RIGHT;
				case Direction.LEFT:
					return Direction.DOWN;
			}
			return Direction.UP;
		}
	}
}
