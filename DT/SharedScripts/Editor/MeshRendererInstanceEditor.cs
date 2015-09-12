using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEditor;
﻿using UnityEngine;

namespace DT {
	[CustomEditor(typeof(MeshRendererInstanceComponent))]
	public class MeshRendererInstanceEditor : Editor {
		// PRAGMA MARK - INTERNAL
		// protected AutoSnap _autoSnapInstance = AutoSnap.Instance();
		protected MeshRendererInstanceComponent obj;
		
		private void OnEnable() {
      obj = target as MeshRendererInstanceComponent;
 		}
		
		public override void OnInspectorGUI() {
			// TEXTURE 
			obj.SpriteTexture = (Texture2D)EditorGUILayout.ObjectField("Texture", obj.SpriteTexture, typeof(Texture2D), false);
			
			if (GUI.changed) {
				EditorUtility.SetDirty(obj);
			}
		}
	}
}
