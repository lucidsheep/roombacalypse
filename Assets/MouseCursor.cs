using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour {

	public Sprite suckSprite;
	public Sprite blowSprite;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(GameEngine.gameOver) {
			pos = new Vector3(-1000f, -1000f, 0f);
			Cursor.visible = true;
		}
		pos.z = 0;
		transform.position = pos;
		if(GameEngine.gameOver)
			return;
		if(Roomba.instance.GetComponentInChildren<Vacuum>().fill >= Roomba.instance.GetComponentInChildren<Vacuum>().capacity)
			GetComponent<SpriteRenderer>().sprite = blowSprite;
		else
			GetComponent<SpriteRenderer>().sprite = suckSprite;
	}
}
