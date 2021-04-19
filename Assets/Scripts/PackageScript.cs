using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageScript : MonoBehaviour {
	public int value;
	public Text txtValue;
	public GameObject destination;
	// Start is called before the first frame update

	public void Start() {
		updateValueText();
	}

	public void updateValueText() {
		txtValue.text = value.ToString();
	}
}
