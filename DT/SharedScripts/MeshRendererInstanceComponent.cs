using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT {
	public class MeshRendererInstanceComponent : MonoBehaviour {
		// PRAGMA MARK - INTERFACE
		public Texture2D SpriteTexture {
			get { return _texture; }
			set { 
				_texture = value;
				this.MaterialInstance.SetTexture("_MainTex", _texture); 
			}
		}
		
		// PRAGMA MARK - INTERNAL
		protected MeshRenderer Renderer {
			get { 
				if (_renderer == null) {
					_renderer = this.GetComponent<MeshRenderer>(); 
				}
				return _renderer;
			}
		}
		
		protected Material MaterialInstance {
			get {
				if (_material == null) {
					_material = new Material(Shader.Find("Sprites/Default-HorizontallyScrolling"));
					this.Renderer.sharedMaterial = _material;
				}
				return _material;
			}
		}
		
		[SerializeField]
		protected Material _material;
		[SerializeField]
		protected MeshRenderer _renderer;
		[SerializeField]
		protected Texture2D _texture;
	}
}