using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

	public float[] defconProgressTime;
	public float[] civilianSpawnTime;
	public float[] smallTankSpawnTime;
	public float[] helicopterSpawnTime;
	public float[] tankSpawnTime;
	private int Score;
	public int score {
		get {
			return this.Score;
		} set {
			if(GameEngine.gameOver == false){
				this.Score = value;
				if(GameEngine.highScore < this.Score) {
					GameEngine.highScore = this.Score;
				}
			}
		}
	}
	public static int highScore = 0;
	public static int DEFCON;
	public static float defconTime;
	public static bool gameWon;
	public static bool gameLost;
	public static bool gameOver;
	public GameObject civilianTemplate;
	public GameObject tankTemplate;
	public GameObject smallTankTemplate;
	public GameObject helicopterTemplate;
	public AudioClip  gameOverExplosion;
	float civilianCooldown;
	float tankCooldown;
	float smallTankCooldown;
	float helicopterCooldown;
	float defconPoints = 0f;
	public static GameEngine instance;

	void Awake() {
		instance = this;
		Cursor.visible = false;
	}
	// Use this for initialization
	void Start () {
		civilianCooldown = civilianSpawnTime[0];
		tankCooldown = tankSpawnTime[0];
		smallTankCooldown = smallTankSpawnTime[0];
		helicopterCooldown = helicopterSpawnTime[0];
		score = 0;
		DEFCON = 5;
		gameWon = gameLost = gameOver = false;
		defconTime = defconProgressTime[0];
		for(int i = 0; i < 40; i++) {
			spawnObject(civilianTemplate, 5f);
		}
		DefconAnim.instance.startAnim();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameOver)
			return;
		civilianCooldown -= Time.deltaTime;
		tankCooldown -= Time.deltaTime;
		smallTankCooldown -= Time.deltaTime;
		helicopterCooldown -= Time.deltaTime;
		if(civilianCooldown <= 0f) {
			civilianCooldown = civilianSpawnTime[5 - DEFCON];
			if(GameObject.FindGameObjectsWithTag("Civilian").Length < 50)
				spawnObject(civilianTemplate);
		}
		if(tankCooldown <= 0f) {
			tankCooldown = tankSpawnTime[5 - DEFCON];
			spawnObject(tankTemplate);
		}
		if(smallTankCooldown <= 0f) {
			smallTankCooldown = smallTankSpawnTime[5 - DEFCON];
			spawnObject(smallTankTemplate);
		}
		if(helicopterCooldown <= 0f) {
			helicopterCooldown = helicopterSpawnTime[5 - DEFCON];
			spawnObject(helicopterTemplate);
		}
	}

	void spawnObject(GameObject template, float range = 25f) {
		Vector3 spawnPos;
		int tries = 100;
		spawnPos.z = 0f;
		do {
			tries--;
			spawnPos.x = Random.Range(-20f, 20f);
			spawnPos.y = Random.Range (-25f, 25f);
		} while(Vector2.Distance(spawnPos, Roomba.instance.transform.position) <= range && tries > 0);

		Instantiate(template, spawnPos, Quaternion.identity);
	}

	public void restartGame() {
		Application.LoadLevel(Application.loadedLevelName);
	}

	public void Roombacalypse() {
		if(!gameOver) {
			score += 10000000;
			gameOver = gameWon = true;
			if(highScore < score) {
				highScore = score;
			}
			GameUI.UpdateUI();
			GameUI.fadeToWhite();
			GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().clip = gameOverExplosion;
			GetComponent<AudioSource>().loop = false;
			GetComponent<AudioSource>().Play();
		}
	}

	public void playerDefeated() {
		if(!gameOver) {
			gameOver = gameLost = true;
			if(highScore < score) {
				highScore = score;
			}
			GameUI.UpdateUI();
			GameUI.fadeToRed();
			GetComponent<AudioSource>().Stop();
		}
	}

	public void getDefconPoints(float amount) {
		defconPoints += amount;
		if(defconPoints >= defconProgressTime[5 - DEFCON]){
			DEFCON--;
			if(DEFCON == 4)
				spawnObject(smallTankTemplate);
			if(DEFCON == 3)
				spawnObject(helicopterTemplate);
			if(DEFCON == 2)
				spawnObject(tankTemplate);
			if(DEFCON == 0) {
				GameEngine.instance.Roombacalypse();
			} else {
				DefconAnim.instance.startAnim();
			}
			defconTime += defconProgressTime[5 - DEFCON];
			civilianCooldown = civilianSpawnTime[5 - DEFCON];
			tankCooldown = tankSpawnTime[5 - DEFCON];
			smallTankCooldown = smallTankSpawnTime[5 - DEFCON];
			helicopterCooldown = helicopterSpawnTime[5 - DEFCON];
		}
	}
}
