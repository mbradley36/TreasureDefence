﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroHandler : MonoBehaviour {
	StoppableCoroutine move, move2;
	List<float> distances = new List<float>();

	// Use this for initialization
	void Start () {
		Debug.Log("hero");
		move = new StoppableCoroutine(movePattern());
		StartCoroutine(move);
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public List<dir> FindClearDirections(){
		List<dir> movable = new List<dir>();
		distances = new List<float>();
		if (CheckDir(new Vector2(-1,0))) movable.Add(dir.left);
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
	
	public void MoveHero(dir d, StoppableCoroutine s){
		s = new StoppableCoroutine(MovementHandler.instance.MoveDir(d, this.transform));
		StartCoroutine(s);
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
		yield return new WaitForSeconds(0.01f);
		dir d = DecideDirection(FindClearDirections());
		Debug.Log("should move " + d);
		MoveHero(d, move2);
	}
}
