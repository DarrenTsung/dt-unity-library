using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.LootSystem {
	public class LootManager : Singleton<LootManager> {
		protected const float SPAWN_FORCE = 200.0f;

		[SerializeField]
		protected LootDatabase database;

		protected LootManager () {}

		protected void Awake() {
			LoadDatabase();
		}

		public void LoadDatabase() {
			database = LootDatabase.Load();
		}

		public void SpawnLootAtPoint(List<string> lootIds, Vector3 point) {
			foreach (string lootId in lootIds) {
				if (!database.ContainsKey(lootId)) {
					Locator.Logger.LogError("LootDatabase does not contain key: " + lootId);
				}
				Loot loot = database.Get(lootId);

				LootDrop chosen = loot.ChooseRandom();
				//Debug.Log ("Loot roll: " + lootId + "... chose: " + chosen.prefabName);
				if (chosen.prefabName.Equals(LootDrop.NONE_PREFAB_NAME)) {
					continue;
				}

				List<GameObject> prefabs = PrefabManager.Instance.SpawnPrefabs(chosen.prefabName, chosen.quantity, point);

				foreach (GameObject instance in prefabs) {
					// TODO: parent should be room
					// set self as parent for now
					instance.transform.parent = transform;

					Rigidbody2D rigidbody = instance.GetComponent<Rigidbody2D>();
					if (rigidbody != null) {
						Vector2 spawnForce = new Vector2(Random.Range(-SPAWN_FORCE / 2.0f, SPAWN_FORCE / 2.0f), 
						                                 Random.Range(SPAWN_FORCE / 2.0f, SPAWN_FORCE));
						rigidbody.AddForce(spawnForce);
					}
				}
			}
		}
	}
}
