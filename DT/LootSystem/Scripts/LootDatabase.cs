using UnityEngine;
using System.Linq;		// used for ElementAt
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT.LootSystem {
	public class LootDatabase : ScriptableObject {
		const string DATABASE_FOLDER_NAME = @"Database";
		const string DATABASE_FILE_NAME = @"LootDatabase.asset";
		const string DATABASE_FULL_PATH = "Assets/Resources/" + DATABASE_FOLDER_NAME + "/" + DATABASE_FILE_NAME;


		public static LootDatabase Load() {
			LootDatabase database = null;
	#if UNITY_EDITOR
			database = AssetDatabase.LoadAssetAtPath(DATABASE_FULL_PATH, typeof(LootDatabase)) as LootDatabase;

			if (database == null) {
				Debug.Log ("No database found: " + database + " remaking..");
				if (!AssetDatabase.IsValidFolder("Assets/Resources/" + DATABASE_FOLDER_NAME)) {
					AssetDatabase.CreateFolder("Assets/Resources", DATABASE_FOLDER_NAME);
				}
				database = ScriptableObject.CreateInstance<LootDatabase>();
				AssetDatabase.CreateAsset(database, DATABASE_FULL_PATH);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
	#else
			database = Resources.Load("Database/LootDatabase") as LootDatabase;
	#endif
			return database;
		}

		// LootIdMap == SerializableDictionary<string, Loot>
		[SerializeField]
		protected LootIdMap database = new LootIdMap();

		public int Count {
			get { return database.Count; }
		}

		public Loot Get(string key) {
			return database[key];
		}

		public void Set(string key, Loot value) {
			database[key] = value;
			DirtyEditorIfPossible();
		}

		public bool ContainsKey(string key) {
			return database.ContainsKey(key);
		}

		public void RenameKey(string oldKey, string newKey) {
			Loot value = database[oldKey];
			database.Remove(oldKey);
			database[newKey] = value;
			DirtyEditorIfPossible();
		}

		public void Remove(string key) {
			database.Remove(key);
			DirtyEditorIfPossible();
		}

		public Dictionary<string, Loot>.KeyCollection Keys() {
			return database.Keys;
		}

		protected void DirtyEditorIfPossible() {
	#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
	#endif
		}
	}
}
