using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Center : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector2(Screen.width/2, Screen.height/2);
		int i = PlayerPrefs.GetInt("chestsSaved");
		if(i == 3){
			transform.gameObject.GetComponent<Text>().text = "You win!";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
