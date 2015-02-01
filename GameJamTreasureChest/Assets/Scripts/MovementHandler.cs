using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum dir{ left, right, up, down }

public class MovementHandler : MonoBehaviour {

	public static MovementHandler instance { get; private set; }
	public bool coroutineRunning = false;
	public bool chestMoving = false;
	public Dictionary<dir, Vector2> dirVector = new Dictionary<dir, Vector2>();
	float minDist = 0.4f;
		
	void Awake() {
		instance = this;
		dirVector.Add (dir.left, new Vector2(-1,0));
		dirVector.Add (dir.right, new Vector2(1,0));
		dirVector.Add (dir.up, new Vector2(0,1));
		dirVector.Add (dir.down, new Vector2(0,-1));
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
					if(CheckDir(t, dirVector[dir.left]) > minDist) {
						t.position = new Vector2(t.position.x - 0.15f, t.position.y);
						yield return new WaitForSeconds(0.1f);
					} else break;
				}
				break;
			case dir.right:
				yield return new WaitForSeconds(0.25f);
				float movedPos2 = t.position.x + 0.5f;
				while(t.position.x < movedPos2){
					if(CheckDir(t, dirVector[dir.right]) > minDist) {
						t.position = new Vector2(t.position.x + 0.15f, t.position.y);
						yield return new WaitForSeconds(0.1f);
					} else break;
				}
				break;
			case dir.up:
				yield return new WaitForSeconds(0.25f);
				float movedPos3 = t.position.y + 0.5f;
				while(t.position.y < movedPos3){
					if(CheckDir(t, dirVector[dir.up]) > minDist) {
						t.position = new Vector2(t.position.x, t.position.y + 0.15f);
						yield return new WaitForSeconds(0.1f);
					} else break;
				}
				break;
			case dir.down:
				yield return new WaitForSeconds(0.25f);
				float movedPos4 = t.position.y - 0.5f;
				while(t.position.y > movedPos4){
					if(CheckDir(t, dirVector[dir.left]) > minDist) {
						t.position = new Vector2(t.position.x, t.position.y - 0.15f);
						yield return new WaitForSeconds(0.1f);
					} else break;
				}
				break;
		}
		coroutineRunning = false;
	}
	
	public float CheckDir(Transform t, Vector2 v){
		RaycastHit2D hit = Physics2D.Raycast(t.position, v);
		if(hit.collider != null){
			float distance = Vector2.Distance(hit.point, t.position);
			Debug.Log(distance);
			return distance;
		}
		return 0f;
	}
}
