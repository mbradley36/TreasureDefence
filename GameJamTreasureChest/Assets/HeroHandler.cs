using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroHandler : MonoBehaviour {
	StoppableCoroutine move;
	StoppableCoroutine move2;
	List<float> distances = new List<float>();
	Dictionary<dir, Vector2> dirVector = new Dictionary<dir, Vector2>();
	bool canMove = true;

	// Use this for initialization
	void Start () {
		move2 = new StoppableCoroutine(MovementHandler.instance.MoveDir(dir.up, this.transform));
		Debug.Log("hero");
		move = new StoppableCoroutine(movePattern());
		StartCoroutine(move);
		dirVector.Add (dir.left, new Vector2(-1,0));
		dirVector.Add (dir.right, new Vector2(1,0));
		dirVector.Add (dir.up, new Vector2(0,1));
		dirVector.Add (dir.down, new Vector2(0,-1));
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
			float distance = Vector2.Distance(hit.point, this.transform.position);
			Debug.Log(distance + " dist " + hit.collider.gameObject.name);
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
		move2 = new StoppableCoroutine(MovementHandler.instance.MoveDir(d, this.transform));
		StartCoroutine(move2);
		RecheckDist(dirVector[d], d);
	}
	
	public void RecheckDist(Vector2 v, dir d){
		Debug.Log("to check: " + v);
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, v);
		if(hit.collider != null){
			float distance = Vector2.Distance(hit.point, this.transform.position);
			if(distance > 2f) StartCoroutine(movePattern(d));
			else {
				move = new StoppableCoroutine(moveSwitch(d));
				StartCoroutine(move);
			}
		}
	}
	
	public dir DecideDirection(List<dir> d){
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
		Debug.Log("should move " + d);
		MoveHero(d);
	}
	
	IEnumerator movePattern(dir d){
		yield return new WaitForSeconds(1f);
		MoveHero(d);
	}
	
	IEnumerator moveSwitch(dir d){
		//if we've moved l/r, try up/down (to keep char from going back and forth)
		//if there isn't an up/down path available, continue as normal
		bool switchedPath = false;
		Vector2 v = dirVector[d];
		distances = new List<float>();
		List<dir> movable = new List<dir>();
		
		if(v.x == 0) {
			if(CheckDir(new Vector2(1, 0))) movable.Add (dir.left);
			if(CheckDir(new Vector2(-1, 0))) movable.Add (dir.right);
		} else if (v.y == 0){
			if(CheckDir(new Vector2(0, 1))) movable.Add (dir.up);
			if(CheckDir(new Vector2(0, -1))) movable.Add (dir.down);
		}
		
		if(movable.Count > 0) switchedPath = true;
		
		if(!switchedPath) {
			yield return 0;
			StartCoroutine(movePattern());
		} else {
			yield return new WaitForSeconds(1f);
			dir newd = DecideDirection(movable);
			Debug.Log("should move " + newd);
			MoveHero(newd);
		}
	}
}
