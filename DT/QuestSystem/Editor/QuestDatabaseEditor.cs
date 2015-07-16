using DT.QuestSystem;
﻿using UnityEditor;
using UnityEngine;
using System;		// used for Math
using System.Collections;
using System.Collections.Generic;

namespace DT.QuestSystemEditor {
	public struct DictionaryRenameOperation {
		public int oldKey;
		public int newKey;
	}

	public struct DictionaryRemoveOperation {
		public int key;
	}

	public class QuestDatabaseEditor : EditorWindow {
		protected QuestDatabase database;

		// Temporary strings for modification (only update value in DB once a valid string is in place)
		protected Dictionary<string, string> temporaryStrings = new Dictionary<string, string>();

		protected GUIStyle richTextStyle = new GUIStyle();

		// ListView
		protected Vector2 _scrollPos;
		protected int selectedIndex = -1;

		// Modifying dictionary
		protected DictionaryRenameOperation renameStruct;
		protected DictionaryRemoveOperation removeStruct;
		
		[MenuItem("DarrenTsung/Databases/Quest Database Editor %#q")]
		public static void Init() {
			QuestDatabaseEditor window = EditorWindow.GetWindow<QuestDatabaseEditor>();
			window.minSize = new Vector2(600, 300);
			window.titleContent = new GUIContent("Quest Database");
			window.Show();
		}

		protected void OnEnable() {
			database = QuestDatabase.Load();

			richTextStyle.richText = true;
		}

		protected void OnGUI() {
			ListView();
			BottomBar();
			EditorUtility.SetDirty(database);
		}

		public void ListView() {
			_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandHeight(true));

			DisplayQuests();

			EditorGUILayout.EndScrollView();
		}

		protected void DisplayQuests() {
			foreach (int questId in database.Keys()) {
				Quest currentQuest = database.Get(questId);
				DisplayQuestGUI(questId, currentQuest);
			}

			ModifyDictionary();
		}

		protected void ModifyDictionary() {
			if (renameStruct.oldKey != 0 && renameStruct.newKey != 0) {
				Debug.Log("Old: " + renameStruct.oldKey);
				Debug.Log("New: " + renameStruct.newKey);
				database.RenameKey(renameStruct.oldKey, renameStruct.newKey);
				renameStruct.oldKey = 0;
				renameStruct.newKey = 0;
			}

			if (removeStruct.key != 0) {
				Debug.Log("Removing : " + removeStruct.key);
				database.Remove(removeStruct.key);
				removeStruct.key = 0;
			}
		}

		protected void DisplayQuestGUI(int questId, Quest quest) {
			GUILayout.BeginHorizontal("Box", GUILayout.MinWidth(500));
			GUILayout.Label("Key: " + questId);
			
			// QUEST ID
			string questIdKey = "QuestId::" + quest.QuestId;
			if (!temporaryStrings.ContainsKey(questIdKey)) {
				temporaryStrings[questIdKey] = quest.QuestId.ToString();
			}

			GUILayout.Label("Id: ");
			temporaryStrings[questIdKey] = GUILayout.TextField(temporaryStrings[questIdKey], GUILayout.Width(25));
			int result = 0;
			if (int.TryParse(temporaryStrings[questIdKey], out result)) {
				if (result != quest.QuestId) {
					int oldQuestId = quest.QuestId;
					int newQuestId = Math.Max(1, result); 
					
					// remove old temporary string 
					temporaryStrings.Remove(questIdKey);
					// remove new temporary string (just in case)
					temporaryStrings.Remove("QuestId::" + newQuestId);
					
					renameStruct.oldKey = oldQuestId;
					renameStruct.newKey = newQuestId;
					quest.QuestId = newQuestId;
				}
			} 
			
			// NAME
			quest.Name = GUILayout.TextField(quest.Name);

			if (GUILayout.Button("x", GUILayout.Width(25), GUILayout.Height(25))) {
				if (EditorUtility.DisplayDialog("Delete Quest?", 
				                                "Are you sure that you want to delete this quest from the database?", 
				                                "Delete", 
				                                "Cancel")) {
					removeStruct.key = quest.QuestId;
				}
			}

			GUILayout.EndHorizontal();
		}

		protected void BottomBar() {
			GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));

			GUILayout.Label("Quests: " + database.Count);

			if (GUILayout.Button("Add Empty")) {
				int unusedId = database.NextUnusedId();
				database.Set(unusedId, new Quest(unusedId));
			}

			GUILayout.EndHorizontal();
		}
	}
}