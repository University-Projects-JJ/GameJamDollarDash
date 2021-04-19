using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public float moveSpeed = 20f;
	Rigidbody2D rb;
	Animator animator;
	Vector2 movement;

	GameObject currentPackage;

	[HideInInspector]
	public int cash = 0, goal = 2000;
	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		cash = 0;
		goal = 2000;
		UIManager.instance.updateCash(cash, goal);

	}

	// Update is called once per frame
	void Update() {
		setPlayerMovement();
	}

	void FixedUpdate() {
		rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		GameObject target = collider.gameObject;

		// if no current package => pickup package
		if (currentPackage == null && target.tag == "Package") {
			pickupPackage(target);
		}

		if (currentPackage != null && target.tag == "Destination") {
			deliverPackage(target);
		}
	}

	void pickupPackage(GameObject target) {
		// pickup package
		currentPackage = target;

		// hide package
		target.SetActive(false);

		// activate destination pointer
		PackageScript packageScript = currentPackage.GetComponent<PackageScript>();
		packageScript.destination.GetComponent<DestinationScript>().showPointer(true);

	}

	void deliverPackage(GameObject target) {
		PackageScript packageScript = currentPackage.GetComponent<PackageScript>();

		bool isStolen = target.name == "home";

		if (target == packageScript.destination || isStolen) {

			int packageValue = packageScript.value;

			// add cash
			cash += isStolen ? packageValue * GameManager.instance.STEAL_MULTIPLIER : packageValue;

			// if goal is reached => update goal & new packageValue*1.5 + new time
			if (cash >= goal) {
				goal *= 4;
				GameManager.instance.goalWasReached();
			}

			// update score
			UIManager.instance.updateCash(cash, goal);


			// if isStolen => time penalty
			GameManager.instance.timePenalty(isStolen);

			// play audio
			gameObject.GetComponent<AudioSource>().Play();

			// hide destination pointer
			packageScript.destination.GetComponent<DestinationScript>().showPointer(false);

			// release current package;
			currentPackage = null;

			// spawn new package
			GameManager.instance.spawnNextPackage();
		}
	}


	void setPlayerMovement() {
		if (GameManager.instance.isRunning) {
			movement.x = movement.y = 0;

			if (Input.GetButton("Horizontal")) {
				movement.x = Input.GetAxisRaw("Horizontal");
				movement.y = 0;
			}

			if (Input.GetButton("Vertical")) {
				movement.x = 0;
				movement.y = Input.GetAxisRaw("Vertical");
			}

			animator.SetFloat("horizontal", movement.x);
			animator.SetFloat("vertical", movement.y);
			animator.SetFloat("speed", movement.sqrMagnitude);
		}
	}
}