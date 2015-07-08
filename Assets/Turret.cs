using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameEngine.gameOver)
			return;
		Vector3 diff = Roomba.instance.transform.position - GetComponentInParent<Tank>().transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

		if(transform.parent.GetComponentInChildren<Suckable>().beingSucked){
			rot_z = transform.rotation.eulerAngles.z + 1f;
		}

		transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
	}
}
