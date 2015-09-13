using DT;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.EditableShapes {
	[Serializable]
	public class LinePoint : PointCloudPoint {
		public LinePoint(Vector2 localPos) {
			this.LocalPosition = localPos;
		}
	}

	public class EditableLineComponent : EditablePointCloudComponent<LinePoint, IEditableLineDelegate> {
		// PRAGMA MARK - INTERFACE
		
		// PRAGMA MARK - INTERNAL
		protected override void OnDrawGizmosSelected() {
			Vector3 previousWorldPoint = this.WorldPointAtIndex(0);
			for (int index = 0; index < _points.Count; index++) {
				Vector3 worldPoint = this.WorldPointAtIndex(index);
				Gizmos.DrawLine(previousWorldPoint, worldPoint);
				Gizmos.DrawSphere(worldPoint, 0.3f);
				previousWorldPoint = worldPoint;
			}
		}
	}
}
