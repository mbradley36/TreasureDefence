using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum dir{ left, right, up, down }

public class MovementHandler : MonoBehaviour {

	public static MovementHandler instance { get; private set; }
	public bool coroutineRunning = false;
	public bool chestMoving = false;
		
	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public IEnumerator MoveDir(dir direction, Transform t) {
		coroutineRunning = true;
		switch(direction){
			case dir.left:
				yield return new WaitForSeconds(0.25f);
				float movedPos = t.position.x - 0.5f;
				while(t.position.x > movedPos){
					t.position = new Vector2(t.position.x - 0.15f, t.position.y);
					yield return new WaitForSeconds(0.1f);
				}
				break;
			case dir.right:
				yield return new WaitForSeconds(0.25f);
				float movedPos2 = t.position.x + 0.5f;
				while(t.position.x < movedPos2){
					t.position = new Vector2(t.position.x + 0.15f, t.position.y);
					yield return new WaitForSeconds(0.1f);
				}
				break;
			case dir.up:
				yield return new WaitForSeconds(0.25f);
				float movedPos3 = t.position.y + 0.5f;
				while(t.position.y < movedPos3){
					t.position = new Vector2(t.position.x, t.position.y + 0.15f);
					yield return new WaitForSeconds(0.1f);
				}
				break;
			case dir.down:
				yield return new WaitForSeconds(0.25f);
				float movedPos4 = t.position.y - 0.5f;
				while(t.position.y > movedPos4){
					t.position = new Vector2(t.position.x, t.position.y - 0.15f);
					yield return new WaitForSeconds(0.1f);
				}
				break;
		}
		coroutineRunning = false;
	}
}
