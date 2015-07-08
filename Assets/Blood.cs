using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

	public Sprite[] spriteList;
	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sprite = spriteList[Random.Range(0, spriteList.Length)];
		transform.rotation = Quaternion.Euler(0f,0f,Random.Range(0f,360f));
		float scale = Random.Range (0.8f, 1.2f);
		transform.localScale = new Vector3(scale, scale, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate() {
		if(GetComponent<Suckable>().beingSucked) {
			Vector2 moveVector = Vector2.MoveTowards(transform.position, Roomba.instance.transform.position, 10 * Time.deltaTime);
			transform.position = moveVector;
		}
	}
}
