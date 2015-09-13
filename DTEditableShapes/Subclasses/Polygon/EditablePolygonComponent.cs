using DT;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.EditableShapes {
	[Serializable]
	public class PolygonPoint : PointCloudPoint {
		public PolygonPoint(Vector2 localPos) {
			this.LocalPosition = localPos;
		}
	}

	public class EditablePolygonComponent : EditablePointCloudComponent<PolygonPoint, IEditablePolygonDelegate> {
		// PRAGMA MARK - INTERFACE
		
		// PRAGMA MARK - INTERNAL
		protected override void OnDrawGizmosSelected() {
			Vector3 previousWorldPoint = this.WorldPointAtIndex(_points.Count - 1);
			for (int index = 0; index < _points.Count; index++) {
				Vector3 worldPoint = this.WorldPointAtIndex(index);
				Gizmos.DrawLine(previousWorldPoint, worldPoint);
				Gizmos.DrawSphere(worldPoint, 0.3f);
				previousWorldPoint = worldPoint;
			}
		}
	}
}
