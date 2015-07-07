using UnityEngine;
﻿using UnityEngine.UI;
using System.Collections;

namespace DT.TweakableVariables {
	public class TweakableVariableView : MonoBehaviour {
		protected Slider slider;
		protected Text text;
		protected GameObject offsetObject;
		
		protected float _height;
		public float Height {
			get { return _height; }
		}
		
		TweakableVariable<float>model;
		
		public void SetupWithModel(TweakableVariable<float> model) {
			this.model = model;
			
			slider.minValue = model.Min;
			slider.maxValue = model.Max;
			slider.value = model.Value;
			
			RefreshText();
		}
		
		public void SetOffset(Vector2 offset) {
			RectTransform offsetTransform = offsetObject.GetComponent<RectTransform>();
			offsetTransform.anchoredPosition = offset;
		}
		
		// PRAGMA MARK - SLIDER DELEGATE
		public void OnValueUpdate(float newValue) {
			model.Value = newValue;
			
			RefreshText();
		}
		
		// PRAGMA MARK - PROTECTED FUNCTIONS 
		protected void RefreshText() {
			text.text = model.Name + ": " + model.Value;
		}
		
		protected void Awake() {
			offsetObject = transform.Find("Offset").gameObject as GameObject;
			slider = offsetObject.transform.Find("Slider").GetComponent<Slider>();
			text = offsetObject.transform.Find("Text").GetComponent<Text>();
			
			_height = GetComponent<RectTransform>().rect.height;
		}
	}
}
