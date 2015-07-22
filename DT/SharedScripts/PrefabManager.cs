using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT {
	public class PrefabList {
		protected Dictionary<string, GameObject> prefabMap;

		public PrefabList() {
			prefabMap = new Dictionary<string, GameObject>();

			Object[] loadedPrefabs = Resources.LoadAll("Prefabs", typeof(GameObject));
			foreach (Object o in loadedPrefabs) {
				GameObject go = o as GameObject;
				prefabMap.Add(go.name, go);
			}
		}

		public bool IsValidPrefabName(string name) {
			return prefabMap.ContainsKey(name);
		}

		public GameObject PrefabForName(string name) {
			return prefabMap[name];
		}
	}

	public class PrefabManager : Singleton<PrefabManager> {
		protected PrefabManager () {}

		protected PrefabList prefabList;

		protected void Awake() {
			prefabList = new PrefabList();
		}

		public GameObject SpawnPrefab(string prefabName, Vector3 position) {
			if (prefabList.IsValidPrefabName(prefabName)) {
				GameObject prefab = prefabList.PrefabForName(prefabName);
				return Instantiate(prefab, position, Quaternion.identity) as GameObject;
			} else {
				Locator.Logger.LogWarning("SpawnPrefab - invalid prefab name: " + prefabName);
			}
			return null;
		}

		public List<GameObject> SpawnPrefabs(string prefabName, uint quantity, Vector3 position) {
			List<GameObject> prefabs = new List<GameObject>();

			for (int i=0; i<quantity; i++) {
				prefabs.Add(SpawnPrefab(prefabName, position));
			}

			return prefabs;
		}

	}
}