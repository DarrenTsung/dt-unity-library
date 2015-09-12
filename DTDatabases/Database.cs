using UnityEngine;
using System.Linq;		// used for ElementAt
using System.Collections;
using System.Collections.Generic;
using Vexe.Runtime.Types;   // used for BetterScriptableObject

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DT {
  public class Database<K, V> : BetterScriptableObject {
		protected const string DATABASE_FOLDER_PREFIX_PATH = @"Assets/Resources";
		protected const string DATABASE_FOLDER_NAME = @"DTDatabases";
		protected const string DATABASE_FILE_EXTENSION = @".asset";

		public static T Load<T>(string fileName) where T : Database<K, V> {
			T database = null;
	#if UNITY_EDITOR
      string databaseFullPath = DATABASE_FOLDER_PREFIX_PATH + "/" + DATABASE_FOLDER_NAME + "/" + fileName + DATABASE_FILE_EXTENSION;
			database = AssetDatabase.LoadAssetAtPath(databaseFullPath, typeof(T)) as T;

			if (database == null) {
				if (!AssetDatabase.IsValidFolder(DATABASE_FOLDER_PREFIX_PATH + "/" + DATABASE_FOLDER_NAME)) {
					AssetDatabase.CreateFolder(DATABASE_FOLDER_PREFIX_PATH, DATABASE_FOLDER_NAME);
				}
        Locator.Logger.Log("Creating new database!");
				database = ScriptableObject.CreateInstance<T>();
				AssetDatabase.CreateAsset(database, databaseFullPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
	#else
			database = Resources.Load(DATABASE_FOLDER_NAME + "/" + fileName) as T;
	#endif
			return database;
		}
    
    protected void DirtyEditorIfPossible() {
	#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
	#endif
		}

		public Dictionary<K, V> database = new Dictionary<K, V>();

		public int Count {
			get { return database.Count; }
		}

		public V Get(K key) {
			return database[key];
		}

		public void Set(K key, V newValue) {
			database[key] = newValue;
			DirtyEditorIfPossible();
		}

		public bool ContainsKey(K key) {
			return database.ContainsKey(key);
		}

		public void RenameKey(K oldKey, K newKey) {
			V oldValue = database[oldKey];
			database.Remove(oldKey);
			database[newKey] = oldValue;
			DirtyEditorIfPossible();
		}

		public void Remove(K key) {
			database.Remove(key);
			DirtyEditorIfPossible();
		}

		public Dictionary<K, V>.KeyCollection Keys() {
			return database.Keys;
		}
	}
}