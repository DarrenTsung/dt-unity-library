using UnityEditor;
using UnityEngine;
using System;
 
namespace DT {
	public class EdgeCollider2DEditor : EditorWindow {
		[MenuItem("DarrenTsung/EdgeCollider2DEditor")]
		public static void ShowWindow() {
			EditorWindow.GetWindow (typeof(EdgeCollider2DEditor));
		}
	 
		EdgeCollider2D edge;
		Vector2[] vertices = new Vector2[0];
	 
		void OnGUI()
		{
			GUILayout.Label ("EdgeCollider2D Editor", EditorStyles.boldLabel);
			edge = (EdgeCollider2D) EditorGUILayout.ObjectField("EdgeCollider2D to edit", edge, typeof(EdgeCollider2D), true);
			
			if (vertices.Length != 0) {
				for (int i = 0; i < vertices.Length; ++i) {
					vertices[i] = (Vector2) EditorGUILayout.Vector2Field("Element "+i, vertices[i]);
				}
			}
	 
			if (GUILayout.Button("Retrieve")) {
				vertices = edge.points;
			}
			
			if (GUILayout.Button("Set")) {
				edge.points = vertices;
			}
			
			if (GUILayout.Button("Add Point")) {
				Vector2[] newVertices = new Vector2[vertices.Length + 1];
				Array.Copy(vertices, newVertices, vertices.Length);
				newVertices[vertices.Length] = new Vector2(0.0f, 0.0f);
				edge.points = newVertices;
				OnGUI();
			}
		}
	 
		void OnSelectionChange() {
			if (Selection.gameObjects.Length == 1) {
				EdgeCollider2D aux = Selection.gameObjects[0].GetComponent<EdgeCollider2D>();
				
				if (aux) {
					edge = aux;
					vertices = edge.points;
				}
			}
		}
	}
}