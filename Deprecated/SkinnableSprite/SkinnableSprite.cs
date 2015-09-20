using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.SkinnableSprites {
	[RequireComponent (typeof (SpriteRenderer))]
	public class SkinnableSprite : MonoBehaviour {
		[SerializeField]
		protected Sprite sprite;
		
		protected string _storedSpriteName;
		protected string _storedPrefix;
		protected SpriteRenderer _spriteRenderer;
		
		public void UpdateSpriteWithPrefix(string prefix) {
			_storedPrefix = prefix;
		}
		
		protected void RefreshSprite() {
			Sprite newSprite = SkinnableSpriteManager.Instance.GetSprite(_storedPrefix, _storedSpriteName);
			
			if (newSprite != null) {
				_spriteRenderer.sprite = newSprite;
			} else {
				Debug.LogError(string.Format("UpdateSpriteWithPrefix - no sprite found for prefix: {0} and spriteName: {1}", _storedPrefix, _storedSpriteName));
			}
		}
		
		protected void OnDidApplyAnimationProperties() {
			string spriteName = SkinnableSpriteManager.RemoveSkinnedPrefixFromSpriteName(sprite.name);
			if (_storedSpriteName == spriteName) {
				return;
			}
			
			_storedSpriteName = spriteName;
			RefreshSprite();
		}
		
		protected void Awake() {
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}
		
		protected void Start() {
			RegisterWithSpriteObject();
		}
		
		protected void RegisterWithSpriteObject() {
			SkinnableSpriteObject spriteObject = GetComponentInParent<SkinnableSpriteObject>();
			spriteObject.Register(this);
		}
	}
}
