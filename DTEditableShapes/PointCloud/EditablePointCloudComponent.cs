using DT;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
﻿using UnityEngine;

namespace DT.EditableShapes {
	[Serializable]
	public class PointCloudPoint {
		public static Vector2[] ExtractLocalPoints(PointCloudPoint[] points) {
			Vector2[] localPoints = new Vector2[points.Length];
			for (int i=0; i<points.Length; i++) {
				localPoints[i] = points[i].LocalPosition;
			}
			return localPoints;
		}
		
		[SerializeField]
		public Vector2 LocalPosition;
		
		public PointCloudPoint() {}
		public PointCloudPoint(Vector2 localPos) {
			this.LocalPosition = localPos;
		}
	}

	public class EditablePointCloudComponent<T, D> : MonoBehaviour where T : PointCloudPoint 
																																 where D : IEditablePointCloudDelegate<T> {
		// PRAGMA MARK - INTERFACE
		public T[] Points {
			get { return _points.ToArray(); }
		}
		
		public virtual void SetPoint(int index, T newPoint) {
			_points[index] = newPoint;
			this.HandlePointsUpdated();
		}
		
		public virtual void RemoveIndex(int index) {
			try {
				_points.RemoveAt(index);
			} catch (ArgumentOutOfRangeException e) {
				Debug.LogWarning("EditablePointCloudComponent::RemoveIndex - invalid index! " + e.Message);
			}
			this.HandlePointsUpdated();
		}
		
		public virtual void AddPoint(Vector2 localPoint) {
			T newPoint = (T)Activator.CreateInstance(typeof(T), new object[] { localPoint });
			
			// Go through each PointCloud segment and compute the distance to 
			int bestLineSegmentIndex = 0;
			float bestLineSegmentDistance = float.MaxValue;
			for (int index = 0; index < _points.Count; index++) {
				Vector2 lineStart = _points[index].LocalPosition;
				Vector2 lineEnd = _points[(index + 1) % _points.Count].LocalPosition;
				float distanceToSegment = newPoint.LocalPosition.MinimumDistanceToLine(lineStart, lineEnd);
				if (distanceToSegment < bestLineSegmentDistance) {
					bestLineSegmentDistance = distanceToSegment;
					bestLineSegmentIndex = index;
				}
			}
			_points.Insert(bestLineSegmentIndex + 1, newPoint);
			this.HandlePointsUpdated();
		}

		public void HandlePointsUpdated() {
			T[] points = this.Points;
			
			D[] delegates = this.GetDelegates();
			foreach (D del in delegates) {
				del.HandlePointsUpdated(points);
			}
			SceneView.RepaintAll();
		}
		
		// PRAGMA MARK - INTERNAL
		[SerializeField]
		protected List<T> _points = new List<T>();
		protected D[] _delegates;
		
		protected D[] GetDelegates() {
			if (_delegates == null) {
				_delegates = this.GetComponents<D>();
			}
			return _delegates;
		}
		
		protected virtual void Awake() {
			
		}
		
		protected virtual void OnDrawGizmosSelected() {
			foreach (T point in _points) {
				Vector3 worldPoint = (Vector3)point.LocalPosition + transform.position;
				Gizmos.DrawSphere(worldPoint, 0.3f);
			}
		}
		
		protected Vector3 WorldPointAtIndex(int index) {
			if (index < 0 || index > _points.Count - 1) {
				Debug.LogError("WorldPointAtIndex - invalid index: " + index);
				return Vector3.zero;
			}
			return (Vector3)_points[index].LocalPosition + transform.position;
		}
	}
}
