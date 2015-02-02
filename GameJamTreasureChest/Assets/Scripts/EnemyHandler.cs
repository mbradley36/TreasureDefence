using UnityEngine;
using System.Collections;

public class EnemyHandler : MonoBehaviour {
	public Animator animator;
	public Transform pos1, pos2;
	public float pauseTime;
	public int stepsTaken;
	private float lastPause;
	private bool moving = false;
	private bool flipped = false;
	public StoppableCoroutine s;
	public bool still = false;

	// Use this for initialization
	void Start () {
		lastPause = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - lastPause > pauseTime && !moving){
			StartCoroutine(EnemyMovement());
		}
		
		if(MovementHandler.instance.chestMoving) {
			CheckForPlayerMovement();
		}
	}
	
	public void SetStill(bool _still){
		still = _still;
	}
	
	public void ShiftPositionMarkers(Transform shiftPos){
		Vector2 offset = new Vector2(pos1.position.x - pos2.position.x, pos1.position.y - pos2.position.y);
		offset.Normalize();
		pos2.position = shiftPos.position + new Vector3(offset.x, offset.y, 0);
	}
	
	void OnTriggerEnter2D(Collider2D c){
		//Debug.Log("entered collision");
		if(c.gameObject == pos2.gameObject){
			s.Stop();
			flipped = true;
			Transform posHolder = pos1;
			pos1 = pos2;
			pos2 = posHolder;
		}
	}
	
	IEnumerator EnemyMovement(){
		moving = true;
		int steps = 0;
		dir direction = dir.up;
		while(steps < stepsTaken) {
			animator.SetBool("isWalking", true);
			if(pos1.position.x - pos2.position.x > 0) {
				direction = dir.left;
			} else if (pos1.position.x - pos2.position.x < 0){
				direction = dir.right;
			} else if (pos1.position.y - pos2.position.y < 0){
				direction = dir.up;
			} else {
				direction = dir.down;
			}
			
			s = new StoppableCoroutine(MovementHandler.instance.MoveDir(direction, transform));
			if(!still)yield return StartCoroutine(s);
			
			if(flipped) {
				transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
				flipped = false;
			}
			steps++;
			animator.SetBool("isWalking", false);
		}
		
		lastPause = Time.time;
		moving = false;
		yield return new WaitForSeconds(0.001f);
	}
	
	void CheckForPlayerMovement(){
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(pos2.position.x - pos1.position.x, pos2.position.y - pos1.position.y));
		if(hit.collider != null){
			if(hit.collider.gameObject.tag == "Player"){
				Debug.Log("spotted!");
				RelocateGoalPos();
			}
		}
	}
	
	void RelocateGoalPos(){
		//find good place for pos2
	}

	/*void OnCollisionEnter2D(Collision2D c){
		Debug.Log("stopped movement " + c.gameObject.name);
		StopCoroutine(s);
		s = new StoppableCoroutine(MovementHandler.instance.MoveDir(direction, transform));
		StartCoroutine(s);
	}*/
}
