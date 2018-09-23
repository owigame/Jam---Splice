using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSelected : MonoBehaviour {
	public bool selected = false;

	public void Selected () {
		selected = !selected;
		if (selected){
			transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		} else {
			transform.localScale = Vector3.one;
		}
	}
	
	void Update () {
		
	}
}
