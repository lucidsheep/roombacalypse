using UnityEngine;
using System.Collections;

public class DefconAnim : MonoBehaviour {

	public TextMesh text;

	float animProgress = 0f;
	float animLength = 3f;
	bool animInProgress = false;

	float stage1 = .5f;
	float stage2 = 2f;
	float stage3 = .5f;
	public static DefconAnim instance;
	// Use this for initialization
	void Awake() {
		instance = this;
	}
	void Start () {
		text.GetComponent<MeshRenderer>().sortingLayerName = "UI";

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 origin = Camera.main.transform.position;
		origin.z = 0f;
		origin.y += 7.5f;
		origin.x -= 60f;
		if(animInProgress) {
			animProgress += Time.deltaTime;

			if(animProgress < stage1) {
				origin.x += (57.5f * (animProgress / stage1)); 
			} else {
				origin.x += 57.5f;
			}

			if(animProgress > stage1 && animProgress < stage1 + stage2) {
				origin.x += (5f * ((animProgress - .5f) / stage2));
			} else if(animProgress > stage1){
				origin.x += 5f;
			}
			if(animProgress >= stage1 + stage2) {
				origin.x += (55f * ((animProgress - 2.5f) / stage3));
			}
			//origin.x += (120f * (animProgress / animLength));
			if(animProgress >= animLength) {
				animInProgress = false;
				GetComponent<AudioSource>().Stop ();
			}
		}
		transform.position = origin;
	}

	public void startAnim() {
		if(!animInProgress) {
			animInProgress = true;
			GetComponentInChildren<TextMesh>().text = GameEngine.DEFCON.ToString();
			GetComponent<AudioSource>().Play();
			animProgress = 0f;
		}
	}
}
