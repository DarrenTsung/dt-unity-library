using DT;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.EditableShapes {
	public class EditableSingleLineComponent : EditableLineComponent {
		// PRAGMA MARK - INTERFACE
		public override void AddPoint(Vector2 localPoint) {
			// do nothing, start with two points - don't allow additional points
		}
		
		public override void RemoveIndex(int index) {
			// do nothing, start with two points - don't allow removing points
		}
		
		// PRAGMA MARK - INTERNAL
	}
}
