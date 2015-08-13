using DT;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.EditableShapes {
	[Serializable]
	public struct PolygonPoint {
		public static Vector2[] ExtractLocalPoints(PolygonPoint[] points) {
			Vector2[] localPoints = new Vector2[points.Length];
			for (int i=0; i<points.Length; i++) {
				localPoints[i] = points[i].LocalPosition;
			}
			return localPoints;
		}
		
		public Vector2 LocalPosition;
		
		public PolygonPoint(Vector2 localPos) {
			LocalPosition = localPos;
		}
	}

	public class EditablePolygonComponent : MonoBehaviour {
		// PRAGMA MARK - INTERFACE
		public PolygonPoint[] Points {
			get { return _points.ToArray(); }
		}
		
		public void SetPoint(int index, PolygonPoint newPoint) {
			_points[index] = newPoint;
			this.HandlePointsUpdated();
		}
		
		public void RemoveIndex(int index) {
			try {
				_points.RemoveAt(index);
			} catch (ArgumentOutOfRangeException e) {
				Debug.LogWarning("EditablePolygonComponent::RemoveIndex - invalid index! " + e.Message);
			}
			this.HandlePointsUpdated();
		}
		
		public void AddPoint(PolygonPoint newPoint) {
			// calculate closest point and add after that point
			PolygonPoint[] sortedPoints = _points.OrderBy(p => (p.LocalPosition - newPoint.LocalPosition).magnitude).ToArray();
			
			int bestIndex = _points.IndexOf(sortedPoints[0]);
			int secondBestIndex = _points.IndexOf(sortedPoints[1]);
			
			int smallerIndex = (bestIndex < secondBestIndex) ? bestIndex : secondBestIndex;
			_points.Insert(smallerIndex + 1, newPoint);
			this.HandlePointsUpdated();
		}
		
		// PRAGMA MARK - INTERNAL
		[SerializeField]
		protected List<PolygonPoint> _points = new List<PolygonPoint>();
		protected PolygonCollider2D _collider;
		protected IEditablePolygonDelegate[] _delegates;
		
		protected void Awake() {
			_collider = this.GetRequiredComponent<PolygonCollider2D>();
		}

		protected void HandlePointsUpdated() {
			PolygonPoint[] points = this.Points;
			
			IEditablePolygonDelegate[] delegates = this.GetDelegates();
			foreach (IEditablePolygonDelegate d in delegates) {
				d.HandlePointsUpdated(points);
			}
		}
		
		protected IEditablePolygonDelegate[] GetDelegates() {
			if (_delegates == null) {
				_delegates = this.GetComponents<IEditablePolygonDelegate>();
			}
			return _delegates;
		}
		
		protected void OnDrawGizmosSelected() {
			Vector3 previousWorldPoint = (Vector3)_points[_points.Count - 1].LocalPosition + transform.position;
			foreach (PolygonPoint point in _points) {
				Vector3 worldPoint = (Vector3)point.LocalPosition + transform.position;
				Gizmos.DrawLine(previousWorldPoint, worldPoint);
				Gizmos.DrawSphere(worldPoint, 0.3f);
				previousWorldPoint = worldPoint;
			}
		}
	}
}
