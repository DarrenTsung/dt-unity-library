using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.LootSystem {
	public class LootController : MonoBehaviour {
		[SerializeField]
		protected List<string> lootIds;

		public void SpawnLoot() {
			LootManager.Instance.SpawnLootAtPoint(lootIds, transform.position);
		}

		public void SpawnLoot(Vector3 position) {
			LootManager.Instance.SpawnLootAtPoint(lootIds, position);
		}
	}
}
