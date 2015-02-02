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

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		move2 = new StoppableCoroutine(MovementHandler.instance.MoveDir(dir.up, this.transform));
		//Debug.Log("hero");
		move = new StoppableCoroutine(movePattern());
		StartCoroutine(move);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public List<dir> FindClearDirections(){
		List<dir> movable = new List<dir>();
		distances = new List<float>();
		if(CheckDir(new Vector2(-1,0))) movable.Add(dir.left);
		if(CheckDir(new Vector2(1,0))) movable.Add(dir.right);
		if(CheckDir(new Vector2(0,1))) movable.Add(dir.up);
		if(CheckDir(new Vector2(0,-1))) movable.Add(dir.down);
		return movable;
	}
	
	public bool CheckDir(Vector2 v){
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, v);
		if(hit.collider != null){
			Debug.Log(hit.collider.gameObject.name);
			float distance = Vector2.Distance(hit.point, this.transform.position);
			//Debug.Log(distance + " dist " + hit.collider.gameObject.name);
			if(hit.collider.gameObject.name == "otherChest" || hit.collider.gameObject.tag =="Player"){
				distances.Add(99999);
				dirInterrupt = true;
				StopCoroutine(move2);
				Debug.Log("chest seen!");
				return true;
			}
			if(hit.distance > 2f) {
				distances.Add(distance);
				return true;
			} else return false;
		} else {
			return false;
		}
	}
	
	public void MoveHero(dir d){
		//StopCoroutine(move2);
		Debug.Log("trying to move");
		move2 = new StoppableCoroutine(MovementHandler.instance.MoveDir(d, this.transform));
		StartCoroutine(move2);
		RecheckDist(MovementHandler.instance.dirVector[d], d);
	}
	
	public void RecheckDist(Vector2 v, dir d){
		if(dirInterrupt) {
			//dirInterrupt = false;
			return;
		}

		Debug.Log("check");
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, v);
		if(hit.collider != null){
			float distance = Vector2.Distance(hit.point, this.transform.position);
			if(distance > 2f) {
				timesMoved++;
				if(timesMoved > 4) {
					timesMoved = 0;
					move = new StoppableCoroutine(moveSwitch(d));
					StartCoroutine(move);
				} else {
					StartCoroutine(movePattern(d));	
				}
			} else {
				move = new StoppableCoroutine(moveSwitch(d));
				StartCoroutine(move);
			}
		}
	}
	
	public dir DecideDirection(List<dir> d){
		if(d.Count == 0 || distances.Count == 0) return dir.left;
		float largest = distances[0];
		int index = 0;
		for(int i = 0; i < distances.Count; i++){
			if(distances[i] > largest){
				largest = distances[i];
				index = i;
			}
		}
		return d[index];
	}
	
	IEnumerator movePattern(){
		yield return new WaitForSeconds(1f);
		dir d = DecideDirection(FindClearDirections());
		//Debug.Log("should move " + d);
		MoveHero(d);
	}
	
	IEnumerator movePattern(dir d){
		if(!CheckAllDirForChest()) {
			yield return new WaitForSeconds(1f);
			MoveHero(d);
		} else yield return 0;
	}

	IEnumerator moveToChest(dir d){
		yield return new WaitForSeconds(1f);
		MoveHero(d);
	}
	
	IEnumerator moveSwitch(dir d){
		//if we've moved l/r, try up/down (to keep char from going back and forth)
		//if there isn't an up/down path available, continue as normal
		bool switchedPath = false;
		Vector2 v = MovementHandler.instance.dirVector[d];
		distances = new List<float>();
		List<dir> movable = new List<dir>();
		
		if(v.x == 0) {
			Debug.Log("move switch x");
			if(CheckDir(new Vector2(1, 0))) movable.Add (dir.left);
			if(CheckDir(new Vector2(-1, 0))) movable.Add (dir.right);

			if(CheckForChest(new Vector2(0, 1))) movable.Add(dir.up);
			if(CheckForChest(new Vector2(0, -1))) movable.Add(dir.down);
		} else if (v.y == 0){
			Debug.Log("move switch y");
			if(CheckDir(new Vector2(0, 1))) movable.Add (dir.up);
			if(CheckDir(new Vector2(0, -1))) movable.Add (dir.down);

			if(CheckForChest(new Vector2(1, 0))) movable.Add (dir.left);
			if(CheckForChest(new Vector2(-1, 0))) movable.Add (dir.right);
		}
		
		if(movable.Count > 0) switchedPath = true;
		
		if(!switchedPath) {
			yield return 0;
			StartCoroutine(movePattern());
		} else {
			yield return new WaitForSeconds(1f);
			dir newd = DecideDirection(movable);
			//Debug.Log("should move " + newd);
			MoveHero(newd);
		}
	}

	bool CheckAllDirForChest(){
		bool foundChest = false;
		dir d = dir.up;
		Debug.Log("checking all dirs for chest");
		if(CheckForChest(new Vector2(1, 0))) {
			foundChest = true;			
			d=dir.left;
		}
		if(CheckForChest(new Vector2(-1, 0))) {
			foundChest = true;
			d=dir.right;
		}
		if(CheckForChest(new Vector2(0, 1))) {
			foundChest = true;
			d=dir.up;
		}
		if(CheckForChest(new Vector2(0,-1))) {
			foundChest = true;
			d=dir.down;
		}
		if(foundChest) StartCoroutine(moveToChest(d));
		return foundChest;
	}

	bool CheckForChest(Vector2 v){
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, v);
		if(hit.collider != null){
			float distance = Vector2.Distance(hit.point, this.transform.position);
			//Debug.Log(distance + " dist " + hit.collider.gameObject.name);
			Debug.Log(hit.collider.gameObject.name);
			if(hit.collider.gameObject.name == "otherChest" || hit.collider.gameObject.tag =="Player"){
				distances.Add(99999);
				StopCoroutine(move2);
				dirInterrupt = true;
				Debug.Log("chest seen, movin!");
				return true;
			}
		}
		return false;
	}
	
	void OnCollisionEnter2D(Collision2D c){
		Debug.Log("stopped movement " + c.gameObject.name);
		StopCoroutine(move2);
		StopCoroutine(move);
		move = new StoppableCoroutine(movePattern());
		StartCoroutine(move);
	}
}
