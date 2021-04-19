using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {
	public static UIManager instance;
	public Text txtCash, txtCashEnd, txtTimer;
	public Image imgGoalFG;
	public GameObject mainMenu, pauseMenu, endMenu;
	// Start is called before the first frame update

	void Awake() {
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	void Start() {
	}

	// Update is called once per frame
	void Update() {

	}



	public void onClickPlayGame() {
		showMainMenu(false);
		GameManager.instance.startGame();
	}

	public void onClickResumeGame() {
		showPauseMenu(false);
		GameManager.instance.isRunning = true;
	}

	public void onClickRestartGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void onClickPauseGame() {
		GameManager.instance.isRunning = false;
		showPauseMenu(true);
	}

	public void onClickExit() {
		GameManager.instance.endGame();
		Application.Quit();
	}

	public void onClickAudio() {
		GameManager.instance.toggleAudio();
	}

	public void showMainMenu(bool isVisible = true) {
		mainMenu.SetActive(isVisible);
	}
	public void showPauseMenu(bool isVisible = true) {
		pauseMenu.SetActive(isVisible);
	}

	public void showEndMenu() {
		endMenu.SetActive(true);
		txtCashEnd.text = GameManager.instance.player.GetComponent<PlayerScript>().cash.ToString();
	}


	public void updateTimerText(int time) {
		txtTimer.text = time + "s";
	}

	public void updateCash(int current, int next) {
		txtCash.text = current.ToString();
		imgGoalFG.fillAmount = ((float)current) / next;

	}
}
