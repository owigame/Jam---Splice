using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public List<Transform> camTransform = new List<Transform> ();

	// How long the object should shake for.
	public float shakeDuration = 0f;
	float _duration;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	float _shake;
	public float decreaseFactor = 1.0f;

	List<Vector3> originalPos = new List<Vector3> ();

	public static CameraShake _CameraShake;

	void Awake () {
		_CameraShake = this;
		if (camTransform.Count == 0) {
			camTransform.Add (transform);
		}
		foreach (Transform t in camTransform) {
			originalPos.Add (t.localPosition);
		}
	}

	public void DoCameraShake (float duration, float multiplier) {
		_shake = multiplier * shakeAmount;
		if (duration == 0) duration = shakeDuration;
		_duration = duration;
	}

	void Update () {
		if (_duration > 0) {
			Vector3 newShake = (new Vector3 (0, Random.Range (-1f, 1f) * shakeAmount, Random.Range (2f, 5f) * _shake) /* Random.insideUnitSphere */ );
			// Debug.Log ("SHAKE: " + newShake);
			foreach (Transform t in camTransform) {
				t.localPosition = originalPos[camTransform.IndexOf (t)] + newShake;
			}

			_duration -= Time.deltaTime * decreaseFactor;
		} else {
			_duration = 0f;
			foreach (Transform t in camTransform) {
				t.localPosition = originalPos[camTransform.IndexOf (t)];
			}
		}
	}
}