using UnityEngine;
using UnityEngine.EventSystems;
﻿using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DT.TweakableVariables {
	public class TweakableVariableManager : Singleton<TweakableVariableManager> {
		protected const KeyCode TOGGLE_KEY = KeyCode.Return;
		protected const int MAX_VARIABLES_SHOWN = 8;
		
		protected Transform _canvasTransform;
		protected Transform CanvasTransform {
			get {
				if (_canvasTransform == null) {
					foreach (Transform child in transform) {
						Canvas uiCanvas = child.GetComponent<Canvas>();
						if (uiCanvas != null) {
							_canvasTransform = child;
							break;
						}
					}
					
					if (_canvasTransform == null) {
						Debug.LogError("TweakableVariableManager - No child with UICanvas found!");
					}
				}
				
				return _canvasTransform;
			}
		}
		
		protected InputField _searchField;
		
		protected Dictionary<TweakableVariable<float>, GameObject> _variableUIMap;
		protected Dictionary<TweakableVariable<float>, GameObject> VariableUIMap {
			get { 
				if (_variableUIMap == null) 	{
					_variableUIMap = new Dictionary<TweakableVariable<float>, GameObject>();
				}
				return _variableUIMap;
			}
		}
		
		public void Register(TweakableVariable<float> variable) {
			GameObject variableViewObject = Instantiate(Resources.Load("DT/TweakableVariables/TweakableVariableView")) as GameObject;
			TweakableVariableView view = variableViewObject.GetComponent<TweakableVariableView>();
			view.SetupWithModel(variable);
			
			variableViewObject.transform.SetParent(CanvasTransform, false);
			variableViewObject.SetActive(false);
			
			VariableUIMap.Add(variable, variableViewObject);
		}
		
		protected TweakableVariableManager() {}
		
		protected void Update() {
			// if (Input.GetKeyDown(TOGGLE_KEY)) {
			// 	GameObject canvasObject = CanvasTransform.gameObject as GameObject;
			// 	canvasObject.SetActive(!canvasObject.activeSelf);
			// 	
			// 	EventSystem.current.SetSelectedGameObject(_searchField.gameObject, null);
			// 	_searchField.ActivateInputField();
			// }
		}
		
		protected void Awake() {
			_searchField = (CanvasTransform.Find("SearchField").gameObject as GameObject).GetComponent<InputField>();
			
			OnSearchValueUpdate("");
		}
		
		// PRAGMA MARK - UI FUNCTIONS
		public void OnSearchValueUpdate(string searchString) {
			TweakableVariable<float>[] variables = VariableUIMap.Keys.ToArray();
			Array.Sort(variables, delegate(TweakableVariable<float> var1, TweakableVariable<float> var2) {
					double distance1 = StringMatch.ComputeDistance(searchString, var1.Name);
					double distance2 = StringMatch.ComputeDistance(searchString, var2.Name);
					return distance2.CompareTo(distance1);
				});
				
			int index = 0;
			foreach (TweakableVariable<float> variable in variables) {
				GameObject variableViewObject = VariableUIMap[variable];
				
				bool active = index < MAX_VARIABLES_SHOWN;
				variableViewObject.SetActive(active);
				if (active) {
					TweakableVariableView variableView = variableViewObject.GetComponent<TweakableVariableView>();
					Vector2 offset = new Vector2(0.0f, -index * variableView.Height);
					variableView.SetOffset(offset);
				}
				
				index++;
			}
		}
	}
}
