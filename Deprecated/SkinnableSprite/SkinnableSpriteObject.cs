using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.SkinnableSprites {
	public class SkinnableSpriteObject : MonoBehaviour {
		[SerializeField]
		protected string startingPrefix; 
		
		protected string _prefix;
		public string Prefix {
			get { return _prefix; }
			set {
				if (_prefix == value) {
					return;
				}
				
				_prefix = value;
				foreach (SkinnableSprite child in children) {
					child.UpdateSpriteWithPrefix(_prefix);
				}
			}
		}
		
		protected List<SkinnableSprite> children;
		
		public void Register(SkinnableSprite child) {
			children.Add(child);
			child.UpdateSpriteWithPrefix(_prefix);
		}
		
		protected void Awake() {
			children = new List<SkinnableSprite>();
			Prefix = startingPrefix;
		}
	}
}
