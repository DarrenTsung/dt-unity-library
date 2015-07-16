using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.QuestSystem {
	public class QuestDatabase : Database<int, Quest> {
		public static QuestDatabase Load() {
			return Load<QuestDatabase>("QuestDatabase");
		}
		
		public int NextUnusedId() {
			int id;
			for (id = 1; this.ContainsKey(id); id++);
			return id;
		}
	}
}
