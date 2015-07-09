using DT.LootSystem;
﻿using UnityEditor;
using UnityEngine;
using System.Collections;

namespace DT.LootSystemEditor {
	[CustomEditor(typeof(LootManager))]
	public class LootManagerEditor : Editor {

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			
			LootManager myScript = (LootManager)target;
			if(GUILayout.Button("Load LootDatabase"))
			{
				myScript.LoadDatabase();
			}
		}
	}
}