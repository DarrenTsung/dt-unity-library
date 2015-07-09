using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.LootSystemEditor {
	public struct DictionaryRenameOperation {
		public string oldKey;
		public string newKey;
	}

	public struct DictionaryRemoveOperation {
		public string key;
	}

	public class LootDatabaseEditor : EditorWindow {
		protected LootDatabase database;

		// Temporary strings for modification (only update value in DB once a valid string is in place)
		protected Dictionary<string, string> temporaryStrings = new Dictionary<string, string>();
		protected PrefabList prefabList;

		protected GUIStyle richTextStyle = new GUIStyle();

		// ListView
		protected Vector2 _scrollPos;
		protected int selectedIndex = -1;

		// Modifying dictionary
		protected DictionaryRenameOperation renameStruct;
		protected DictionaryRemoveOperation removeStruct;
		
		[MenuItem("DarrenTsung/Database/Loot Database Editor %#i")]
		public static void Init() {
			LootDatabaseEditor window = EditorWindow.GetWindow<LootDatabaseEditor>();
			window.minSize = new Vector2(600, 300);
			window.title = "Loot Database";
			window.Show();
		}

		protected void OnEnable() {
			prefabList = new PrefabList();
			database = LootDatabase.Load();

			richTextStyle.richText = true;
		}

		protected void OnGUI() {
			ListView();
			BottomBar();
			EditorUtility.SetDirty(database);
		}

		// list all of the stored rarities in the database
		public void ListView() {
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandHeight(true));

			DisplayLoots();

			EditorGUILayout.EndScrollView();
		}

		protected void DisplayLoots() {
			foreach (string lootId in database.Keys()) {
				Loot currentLoot = database.Get(lootId);
				DisplayLootGUI(lootId, currentLoot);
			}

			ModifyDictionary();
		}

		protected void ModifyDictionary() {
			if (renameStruct.oldKey != null && renameStruct.newKey != null) {
				database.RenameKey(renameStruct.oldKey, renameStruct.newKey);
				renameStruct.oldKey = null;
				renameStruct.newKey = null;
			}

			if (removeStruct.key != null) {
				database.Remove(removeStruct.key);
				removeStruct.key = null;
			}
		}

		protected void DisplayLootGUI(string lootId, Loot l) {
			GUILayout.BeginHorizontal("Box", GUILayout.MinWidth(500));

			LootGUI(lootId, l);
			LootDropTableGUI(lootId, l);

			GUILayout.EndHorizontal();
		}

		protected void LootGUI(string lootId, Loot l) {
			GUILayout.BeginVertical(GUILayout.Width(150));
			GUILayout.Space(1.6f);
			string newLootId = GUILayout.TextField(lootId, GUILayout.Width(150));
			if (!newLootId.Equals(lootId)) {
				renameStruct.oldKey = lootId;
				renameStruct.newKey = newLootId;
			}

			GUILayout.BeginHorizontal(GUILayout.Width(150), GUILayout.Height(25));

			GUILayout.BeginVertical(GUILayout.Width(125));
			GUILayout.BeginHorizontal();
			string errorString, validString;
			if (l.IsValid(out errorString)) {
				validString = "Valid ✓";
			} else {
				validString = "Not Valid ✖";
			}

			GUILayout.Space(3);
			GUILayout.Label(validString, GUILayout.Width(75));
			GUILayout.Space(47);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space(3);
			GUILayout.Label("<color=red>" + errorString + "</color>", richTextStyle, GUILayout.Width (125));
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			if (GUILayout.Button("x", GUILayout.Width(25), GUILayout.Height(25))) {
				if (EditorUtility.DisplayDialog("Delete Loot?", 
				                                "Are you sure that you want to delete " + lootId + " from the database?", 
				                                "Delete", 
				                                "Cancel")) {
					removeStruct.key = lootId;
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();
		}

		protected void LootDropTableGUI(string lootId, Loot l) {
			GUILayout.BeginVertical("Box", GUILayout.Width(400));
			for (int i=0; i<l.dropTable.Count; i++) {
				LootDrop drop = l.dropTable[i];
				LootDropGUI(lootId, l, drop, i);
			}
			if (GUILayout.Button("+")) {
				l.dropTable.Add(new LootDrop());
			}
			GUILayout.EndVertical();
		}

		protected void LootDropGUI(string lootId, Loot l, LootDrop drop, int index) {
			string errorString;

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal("Box");
			// DROP ROW - REMOVE BUTTON
			if (GUILayout.Button("-", GUILayout.Width(15), GUILayout.Height(15))) {
				if (EditorUtility.DisplayDialog("Delete Drop Row", 
				                                "Are you sure?", 
				                                "Delete", 
				                                "Cancel")) {
					l.dropTable.Remove(drop);
				}
			}

			// DROP ROW - PREFAB NAME
			drop.prefabName = GUILayout.TextField(drop.prefabName, GUILayout.Width(150));

			// DROP ROW - QUANTITY
			bool quantityValid = false;
			string quantityKey = lootId + "Quantity" + index;
			if (!temporaryStrings.ContainsKey(quantityKey)) {
				temporaryStrings[quantityKey] = drop.quantity.ToString();
			}

			GUILayout.Label("Quantity: ");
			temporaryStrings[quantityKey] = GUILayout.TextField(temporaryStrings[quantityKey], GUILayout.Width(25));
			int result = 0;
			if (int.TryParse(temporaryStrings[quantityKey], out result)) {
				drop.quantity = (uint)Mathf.Max(0, result);
				quantityValid = true;
			} 

			// DROP ROW - WEIGHT
			bool weightValid = false;
			string weightKey = lootId + "Weight" + index;
			if (!temporaryStrings.ContainsKey(weightKey)) {
				temporaryStrings[weightKey] = drop.weight.ToString();
			}

			GUILayout.Label("Weight: ");
			temporaryStrings[weightKey] = GUILayout.TextField(temporaryStrings[weightKey], GUILayout.Width(40));
			if (int.TryParse(temporaryStrings[weightKey], out result)) {
				drop.weight = (uint)Mathf.Max(0, result);
				weightValid = true;
			} 

			if (weightValid && quantityValid && drop.IsValid(out errorString, prefabList)) {
				GUILayout.Label("✓");
			} else {
				GUILayout.Label("✖");
			}
			GUILayout.EndHorizontal();

			if (errorString != null && !errorString.Trim().Equals("")) {
				GUILayout.Label("<color=red>" + errorString + "</color>", richTextStyle);
			}
			GUILayout.EndVertical();
		}

		protected void BottomBar() {
			GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));

			GUILayout.Label("Loot Ids: " + database.Count);

			if (GUILayout.Button("Refresh Cached PrefabList")) {
				prefabList = new PrefabList();
			}

			if (GUILayout.Button("Add Empty")) {
				int unnamedCount = 1;
				while (database.ContainsKey("Unnamed " + unnamedCount)) {
					unnamedCount++;
				}
				database.Set("Unnamed " + unnamedCount, new Loot());
			}

			GUILayout.EndHorizontal();
		}
	}
}