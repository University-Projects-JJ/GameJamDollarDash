using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public readonly int STEAL_TIME_PENALTY = 3, STEAL_MULTIPLIER = 2, NEW_PACKAGE_VALUE = 50;
	public int time, newPackageValue = 50, stage = 1;
	public bool isRunning = false;

	public GameObject player;
	public Transform packageLocations, destinationLocations;


	void Awake() {
		Screen.SetResolution(1280, 720, false);

		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
	// Start is called before the first frame update
	void Start() {
		isRunning = false;
		// startGame();
	}

	public void startGame() {
		isRunning = true;
		time = 30;
		newPackageValue = NEW_PACKAGE_VALUE;
		spawnNextPackage();
		StartCoroutine(gameTimer());
	}

	public void endGame() {
		isRunning = false;
	}

	public void goalWasReached() {
		time += 20 * stage++;
		UIManager.instance.updateTimerText(time);
		newPackageValue = Mathf.CeilToInt(newPackageValue * 1.5f);
	}

	// spawns 2-3 packages randomly
	public void spawnNextPackage() {

		// get random package and destination
		int randomPackage = 0;
		int randomDestination = 1;

		// offset by 1 since randomDestination starts from 1
		while (randomPackage + 1 == randomDestination) {
			randomPackage = Random.Range(0, packageLocations.childCount);
			randomDestination = Random.Range(1, destinationLocations.childCount);
		}

		GameObject package = packageLocations.GetChild(randomPackage).gameObject;
		GameObject destination = destinationLocations.GetChild(randomDestination).gameObject;

		PackageScript packageScript = package.GetComponent<PackageScript>();

		// update package value
		packageScript.value = newPackageValue;
		newPackageValue += NEW_PACKAGE_VALUE;
		packageScript.updateValueText();

		// show package
		package.SetActive(true);

		// assign destination
		packageScript.destination = destination;

		// show destinaton pointer
		// packageScript.destination.GetComponent<DestinationScript>().showPointer(true);

	}

	public void toggleAudio() {
		AudioSource audio = GetComponent<AudioSource>();

		if (audio.isPlaying)
			audio.Stop();
		else
			audio.Play();
	}


	IEnumerator gameTimer() {
		while (time > 0) {
			if (isRunning)
				UIManager.instance.updateTimerText(time--);
			yield return new WaitForSeconds(1.0f);
		}
		if (time <= 0) {
			UIManager.instance.updateTimerText(0);
			UIManager.instance.showEndMenu();
			endGame();
		}
	}

	public void timePenalty(bool isStolen) {
		time = isStolen ? time - STEAL_TIME_PENALTY * 2 : time + STEAL_TIME_PENALTY;
		UIManager.instance.updateTimerText(time);
	}
}
