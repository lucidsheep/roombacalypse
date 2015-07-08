using UnityEngine;
using System.Collections;

public class JunkBullet : MonoBehaviour {

	public Sprite[] junkSprites;

	public int numhits;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		float angle = transform.localRotation.eulerAngles.z;
		angle += 180 * Time.deltaTime;
		transform.localRotation = Quaternion.Euler(new Vector3(0,0,angle));
	}
	public void setSize(int fill){
		Sprite junk;
		if(fill <= 300) { //people / blood
			junk = junkSprites[0];
			numhits = 1;
			GetComponentInParent<CircleCollider2D>().radius = 1f;
		} else if(fill <= 1000) {//small tank
			junk = junkSprites[1];
			numhits = 2;
			GetComponentInParent<CircleCollider2D>().radius = 1.25f;

		}
		else if(fill <= 2000){ //helicopter
			junk = junkSprites[2];
			numhits = 3;
			GetComponentInParent<CircleCollider2D>().radius = 1.5f;

		}
		else { //big tank
			junk = junkSprites[3];
			numhits = 4;
			GetComponentInParent<CircleCollider2D>().radius = 2.5f;

		}
		//junk = junkSprites[3];
		GetComponent<SpriteRenderer>().sprite = junk;
	}	
}
