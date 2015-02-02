using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroHandler : MonoBehaviour {

	public static HeroHandler instance {get; private set;}
	StoppableCoroutine move;
	StoppableCoroutine move2;
	int timesMoved = 0;
	List<float> distances = new List<float>();
	bool canMove = true;
	public bool dirInterrupt = false;
	public GameObject player;

	dir[] options = new dir[4]{dir.up, dir.down, dir.left, dir.right};
	dir currentDir = dir.down;
	public bool needToMove = true;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		PickRandomDir();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(needToMove);
		if(needToMove) {
			timesMoved ++;
			if(timesMoved > 4 || AboutToCollide()) {
				timesMoved = 0;
				PickRandomDir();
			}
			needToMove = false;
			StoppableCoroutine s = new StoppableCoroutine(MovementHandler.instance.MoveDir(currentDir, transform));
			StartCoroutine(s);
		}

		if(MovementHandler.instance.chestMoving) {
			CheckForPlayerMovement();
		}
	}

	void PickRandomDir(){
		currentDir = options[Random.Range(0, 4)];
		Vector2 v = MovementHandler.instance.dirVector[currentDir];
		RaycastHit2D hit = Physics2D.Raycast(transform.position, v);
		Debug.DrawRay(transform.position, v, Color.red);
		if(hit.collider != null){
			float distance = Vector2.Distance(hit.point, transform.position);
			if(distance < 0.30f) {
				PickRandomDir();
			}
		}
	}

	bool AboutToCollide(){
		Vector2 v = MovementHandler.instance.dirVector[currentDir];
		RaycastHit2D hit = Physics2D.Raycast(transform.position, v);
		Debug.DrawRay(transform.position, v, Color.red);
		if(hit.collider != null){
			float distance = Vector2.Distance(hit.point, transform.position);
			if(distance < 0.30f) {
				return true;
			}
		}
		return false;
	}

	void CheckForPlayerMovement(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position, MovementHandler.instance.dirVector[currentDir]);
		if(hit.collider != null){
			Debug.Log("saw " + hit.collider.gameObject.name);
			if(Mathf.Abs(transform.parent.position.x - player.transform.position.x)<1.0f){
				if(currentDir == dir.down){
					if(player.transform.position.y > hit.collider.gameObject.transform.position.y &&
						player.transform.position.y < transform.position.y){
						Debug.Log("spotted by hero!");
					}
				} else {
					if(player.transform.position.y < hit.collider.gameObject.transform.position.y &&
						player.transform.position.y > transform.position.y){
						Debug.Log("spotted by hero!");
					}
				}
			} else if(Mathf.Abs(transform.parent.position.y - player.transform.position.y)<1.0f){
				Debug.Log("check x");
				if(currentDir == dir.right){
					Debug.Log("moving right");
					if(player.transform.position.x < hit.collider.gameObject.transform.position.x &&
						player.transform.position.x > transform.position.x){
						Debug.Log("spotted by hero!");
					}
				} else {
					Debug.Log("moving left");
					if(player.transform.position.x > hit.collider.gameObject.transform.position.x &&
						player.transform.position.x < transform.position.x){
						Debug.Log("spotted by hero!");
					}
				}
			}
		}
	}
}
