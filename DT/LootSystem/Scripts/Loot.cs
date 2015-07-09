using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.LootSystem {
	[System.Serializable]
	public class LootDrop {
		public const string NONE_PREFAB_NAME = "None";
		public string prefabName;
		public uint quantity;
		public uint weight;

		public LootDrop() {
			prefabName = NONE_PREFAB_NAME;
			quantity = 1;
			weight = 10;
		}

		public bool IsValid(out string error, PrefabList plist) {
			if (prefabName.Trim().Equals("")) {
				error = "Prefab name is empty";
				return false;
			}

			if (!plist.IsValidPrefabName(prefabName) && !prefabName.Equals(NONE_PREFAB_NAME)) {
				error = string.Format("Prefab name: {0} is not a valid prefab name", prefabName);
				return false;
			}

			if (weight == 0) {
				error = "Weight is 0";
				return false;
			}

			error = "";
			return true;
		}
	}

	[System.Serializable]
	public class Loot {
		public List<LootDrop> dropTable;

		public Loot() {
			dropTable = new List<LootDrop>();
		}

		public bool IsValid(out string error) {
			if (dropTable.Count == 0) {
				error = "Drop table is empty";
				return false;
			}

			error = "";
			return true;
		}

		public LootDrop ChooseRandom() {
			uint combinedWeight = 0;
			foreach (LootDrop drop in dropTable) {
				combinedWeight += drop.weight;
			}

			int chosenIndex = Random.Range(0, (int)combinedWeight);

			uint lastIndex = 0;
			foreach (LootDrop drop in dropTable) {
				if (lastIndex <= chosenIndex  && chosenIndex < lastIndex + drop.weight) {
					return drop;
				}
				lastIndex += drop.weight;
			}

			Debug.LogError("LootDrop:ChooseRandom - Drop was not chosen correctly | chosenIndex: " + chosenIndex);
			return null;
		}
	}

	[System.Serializable]
	public class LootIdMap : SerializableDictionary<string, Loot> {}
}