using UnityEngine;
using System.Linq;		// used for ElementAt
using System.Collections;
using System.Collections.Generic;

namespace DT.LootSystem {
	public class LootDatabase : Database<string, Loot> {
		public static LootDatabase Load() {
			return Load<LootDatabase>("LootDatabase");
		}
	}
}
