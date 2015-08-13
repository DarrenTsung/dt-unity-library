using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEditor;
﻿using UnityEngine;

namespace DT.EditableShapes {
	[CustomEditor(typeof(EditablePolygonComponent))]
	public class EditablePolygonComponentEditor : Editor {
		// PRAGMA MARK - INTERNAL
		protected int _lastIndexInteractedWith = 0;
		
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
			EditablePolygonComponent shape = this.Shape();
			Vector2 localPosition = (Vector2)(Event.current.MouseWorldPosition() - shape.gameObject.transform.position);
			shape.AddPoint(new PolygonPoint(localPosition));
		}
		
		protected void HandleDeletePoint() {
			this.Shape().RemoveIndex(_lastIndexInteractedWith);
		}
		
		protected void AddDraggableHandles() {
			EditablePolygonComponent shape = this.Shape();
			PolygonPoint[] points = shape.Points;
			
			int index = 0;
			foreach (PolygonPoint point in points) {
				Vector3 worldPosition = shape.transform.position + (Vector3)point.LocalPosition;
				
				CustomHandle.DragHandleResult dhResult;
				string label = index.ToString();
				if (index == _lastIndexInteractedWith) {
					label += "[Last Interacted]";
				}
				Handles.Label(worldPosition + new Vector3(0.5f, 0, 0), label);
				Vector3 movedWorldPosition = CustomHandle.DragHandle(worldPosition, 0.3f, Handles.SphereCap, Color.red, out dhResult);
				Vector3 movedLocalPosition = movedWorldPosition - shape.gameObject.transform.position;
				
				switch (dhResult) {
					case CustomHandle.DragHandleResult.LMBDrag: {
						PolygonPoint copy = point;
						copy.LocalPosition = movedLocalPosition;
						shape.SetPoint(index, copy);
						break;
					}
					case CustomHandle.DragHandleResult.LMBRelease:
						_lastIndexInteractedWith = index;
						break;
				}
				
				if (GUI.changed) {
					EditorUtility.SetDirty(shape);
				}
				
				index++;
			}
		}
		
		protected EditablePolygonComponent Shape() {
			return (EditablePolygonComponent)target;
		}
	}
}