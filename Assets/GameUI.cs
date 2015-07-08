using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour {

	public static GameUI instance;


	public Text capacityText;
	public Text scoreText;
	public Image fader;
	public Button restartButton;
	public Text youWinText;
	public Text highScoreText;
	public int currentScore;
	public Text shadowText;


	bool isFading = false;
	float fadeProgress = 0f;
	// Use this for initialization
	void Awake() {
		instance = this;
		currentScore = 0;
		youWinText.enabled = false;
		highScoreText.enabled = false;
		restartButton.GetComponentInChildren<Text>().enabled = false;
		restartButton.GetComponent<Image>().enabled = false;
	}
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(currentScore > GameEngine.instance.score) {
			currentScore = GameEngine.instance.score;
			UpdateUI();
		}
		if(currentScore < GameEngine.instance.score) {
			int diff = GameEngine.instance.score - currentScore;
			if(diff < 250)
				currentScore += diff;
			else if(diff < 1000)
				currentScore += Random.Range(50, 100);
			else if(diff < 5000)
				currentScore += Random.Range (200, 400);
			else if (diff < 10000)
				currentScore += (Random.Range (500, 1000));
			else
				currentScore += Random.Range(8000, 10000);
			UpdateUI();
		}
		if(isFading) {
			fadeProgress += Time.deltaTime;
			if(GameEngine.gameLost) {
				fadeProgress += Time.deltaTime * 2;
			}
			float alpha = fadeProgress;
			if(GameEngine.gameLost)
				alpha = Mathf.Min(3f, alpha);
			fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, alpha / 4f);
			if(fadeProgress >= 3f) {
				highScoreText.enabled = true;
				restartButton.GetComponent<Image>().enabled = true;
				restartButton.GetComponentInChildren<Text>().enabled = true;
				if(GameEngine.gameWon) {
					youWinText.enabled = true;
				}
			}
		}
	}

	public static string numberfy(int number) {
		string txt;
		txt = string.Format("{0:#,###0.#}", number);
		return txt;
	}
	public static void UpdateUI() {

		instance.scoreText.text = numberfy(instance.currentScore);
		instance.shadowText.text = instance.scoreText.text;
		instance.highScoreText.text = "Best Score\n" + numberfy(GameEngine.highScore);
		//instance.capacityText.text = "Capacity " + Roomba.instance.GetComponentInChildren<Vacuum>().fill + "/" + Roomba.instance.GetComponentInChildren<Vacuum>().capacity;
		//instance.healthText.text = "Health " + Roomba.instance.GetComponentInChildren<Hittable>().hitPoints;
	}

	public static void fadeToWhite() {
		instance.isFading = true;
		MCamera.ScreenShake(4f, 3f);
	}

	public static void fadeToRed() {
		instance.isFading = true;
		instance.fader.color = Color.black;
	}
}
