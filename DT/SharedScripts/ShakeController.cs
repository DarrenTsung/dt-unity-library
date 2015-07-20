using UnityEngine;
using System.Collections;

public class ShakeController : MonoBehaviour {
	protected float _shakeRadius; 
	protected float _shakeDuration;
	protected float _baseTimeBetweenShakes;
	protected float _timeBetweenShakesTimer;
	protected Vector3 _shakeOffset;
	
	protected Vector3 _basePosition;
	
	protected bool _isShaking;
	public bool Shaking {
		get { return _isShaking; }
	}
	
	public void Shake(float radius, float duration, float timeBetweenShakes) {
		_shakeRadius = radius;
		_shakeDuration = duration;
		_baseTimeBetweenShakes = timeBetweenShakes;
		_timeBetweenShakesTimer = 0.0f;
		
		// if we weren't previously shaking, store the current position as the base position
		if (!Shaking) {
			_basePosition = transform.position;
			_isShaking = true;
		}
	}

	protected void Update () {
		if (_shakeDuration > 0.0f) {
			_timeBetweenShakesTimer -= Time.deltaTime;
			if (_timeBetweenShakesTimer < 0.0f) {
				_timeBetweenShakesTimer += _baseTimeBetweenShakes;
				
				float xShake = Random.Range(-_shakeRadius, _shakeRadius);
				float yShake = Random.Range(-_shakeRadius, _shakeRadius);
				
				_shakeOffset = new Vector3(xShake, yShake, 0.0f);
				transform.position = _basePosition + _shakeOffset;
			}
			_shakeDuration -= Time.deltaTime;
		} else {
			transform.position = _basePosition;
		}
	}
}
