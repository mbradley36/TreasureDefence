using UnityEngine;
using System.Collections;

public class Reload : MonoBehaviour {
	// Use this for initialization
	void Start () {
		transform.position = new Vector2(Screen.width/2, Screen.height/4);
	}
	
	public void OnClick(){
		Application.LoadLevel(0);
	}
}
