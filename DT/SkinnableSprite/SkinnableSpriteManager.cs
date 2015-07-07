using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.SkinnableSprites {
	public class SkinnableSpriteManager : Singleton<SkinnableSpriteManager> {
		public const char SKINNABLE_SPRITE_DELIMINATOR = '@';
		protected Dictionary<string, Sprite> spriteMap;
		
		public static string RemoveSkinnedPrefixFromSpriteName(string spriteFullname) {
			string[] parts = spriteFullname.Split(SKINNABLE_SPRITE_DELIMINATOR);
			return parts[parts.Length - 1];
		}
		
		public string ConstructFilename(string prefix, string spriteName) {
			return prefix + SKINNABLE_SPRITE_DELIMINATOR + spriteName;
		}
		
		public Sprite GetSprite(string prefix, string spriteName) {
			string filename = ConstructFilename(prefix, spriteName);
			if (spriteMap.ContainsKey(filename)) {
				return spriteMap[filename];
			}
			return null;
		}
		
		protected SkinnableSpriteManager() {}
		
		protected void Awake() {
			spriteMap = new Dictionary<string, Sprite>();
			
			Sprite[] skinnableSprites = Resources.LoadAll<Sprite>("SkinnableSprites");
			foreach(Sprite sprite in skinnableSprites) {
				spriteMap.Add(sprite.name, sprite);
			}
		}
	}
}