using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEditor;
﻿using UnityEngine;

namespace DT.EditableShapes {
	public class EditablePointCloudComponentEditor<T, D> : Editor where T : PointCloudPoint 
																														    where D : IEditablePointCloudDelegate<T> {
		// PRAGMA MARK - INTERNAL
		protected AutoSnap _autoSnapInstance = AutoSnap.Instance();
		protected int _lastIndexInteractedWith = 0;
		protected EditablePointCloudComponent<T, D> _shape;
		
		protected void OnEnable() {
			_shape = target as EditablePointCloudComponent<T, D>;
			Undo.undoRedoPerformed += UndoRedoPerformed;
 		}
		
		protected void OnDisable() {
			Undo.undoRedoPerformed -= UndoRedoPerformed;
		}
		
		protected void UndoRedoPerformed() {
			_shape.HandlePointsUpdated();
		}
		
		protected void OnSceneGUI() {
			this.ListenForEvents();
			this.AddDraggableHandles();
		}
		
		protected void ListenForEvents() {
			Event e = Event.current;
			switch (e.type) {
				case EventType.KeyDown:
					this.HandleKeyDown(e.character);
					break;
			}
		}
		
		protected void HandleKeyDown(char character) {
			switch (character) {
				case 'a':
					this.HandleAddPoint();
					break;
				case 'd':
					this.HandleDeletePoint();
					break;
			}
		}
		
		protected void HandleAddPoint() {
			Vector2 localPosition = (Vector2)(Event.current.MouseWorldPosition() - _shape.gameObject.transform.position);
			localPosition = _autoSnapInstance.SnapToValues(localPosition);
			Undo.RecordObject(_shape, "AddPoint");
			_shape.AddPoint(localPosition);
		}
		
		protected void HandleDeletePoint() {
			Undo.RecordObject(_shape, "DeletePoint");
			_shape.RemoveIndex(_lastIndexInteractedWith);
		}
		
		protected void AddDraggableHandles() {
			PointCloudPoint[] points = _shape.Points;
			
			int index = 0;
			foreach (T point in points) {
				Vector3 worldPosition = _shape.transform.position + (Vector3)point.LocalPosition;
				
				CustomHandle.DragHandleResult dhResult;
				string label = index.ToString();
				if (index == _lastIndexInteractedWith) {
					label += "[Last Interacted]";
				}
				Handles.Label(worldPosition + new Vector3(0.5f, 0, 0), label);
				Vector3 movedWorldPosition = CustomHandle.DragHandle(worldPosition, 0.3f, Handles.SphereCap, Color.red, out dhResult);
				Vector3 movedLocalPosition = movedWorldPosition - _shape.gameObject.transform.position;
				
				switch (dhResult) {
					case CustomHandle.DragHandleResult.LMBDrag: {
						T copy = point;
						movedLocalPosition = _autoSnapInstance.SnapToValues(movedLocalPosition);
						copy.LocalPosition = movedLocalPosition;
						Undo.RecordObject(_shape, "SetPoint");
						_shape.SetPoint(index, copy);
						break;
					}
					case CustomHandle.DragHandleResult.LMBRelease:
					case CustomHandle.DragHandleResult.LMBClick:
						_lastIndexInteractedWith = index;
						break;
				}
				
				if (GUI.changed) {
					EditorUtility.SetDirty(_shape);
				}
				
				index++;
			}
		}
	}
}