using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestinationScript : MonoBehaviour {
	public GameObject pointer; // image
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void showPointer(bool isVisible) {
		pointer.SetActive(isVisible);
	}
}
