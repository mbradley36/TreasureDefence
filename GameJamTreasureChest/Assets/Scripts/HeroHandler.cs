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
			PickRandomDir();
			needToMove = false;
			StoppableCoroutine s = new StoppableCoroutine(MovementHandler.instance.MoveDir(currentDir, transform));
			StartCoroutine(s);
		}
	}

	void PickRandomDir(){
		currentDir = options[Random.Range(0, 4)];
		Vector2 v = MovementHandler.instance.dirVector[currentDir];
		RaycastHit2D hit = Physics2D.Raycast(transform.position, v);
		Debug.DrawRay(transform.position, v, Color.red);
		if(hit.collider != null){
			float distance = Vector2.Distance(hit.point, transform.position);
			if(distance < 0.25f) {
				PickRandomDir();
			}
		}
	}
}
