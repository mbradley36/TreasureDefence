using UnityEngine;
using System.Collections;

public class ChestHandler : MonoBehaviour {
	public Animator animator;
	public float coolDownMax;
	private float coolDown;
	private ArrayList capturedItems = new ArrayList();
	//private StoppableCoroutine s;

	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate(){
		coolDown -= 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		dir direction = dir.up;
		animator.SetBool("jump", true);	
		
		if(Input.GetKeyDown(KeyCode.LeftArrow) && coolDown < 0){
			direction = dir.left;
			StoppableCoroutine s = new StoppableCoroutine(MovementHandler.instance.MoveDir(direction, transform));
			StartCoroutine(s);
		} else if(Input.GetKeyDown(KeyCode.RightArrow) && coolDown < 0){
			direction = dir.right;
			StoppableCoroutine s = new StoppableCoroutine(MovementHandler.instance.MoveDir(direction, transform));
			StartCoroutine(s);
		} else if(Input.GetKeyDown(KeyCode.UpArrow) && coolDown < 0){
			direction = dir.up;
			StoppableCoroutine s = new StoppableCoroutine(MovementHandler.instance.MoveDir(direction, transform));
			StartCoroutine(s);
		} else if(Input.GetKeyDown(KeyCode.DownArrow) && coolDown < 0){
			direction = dir.down;
			StoppableCoroutine s = new StoppableCoroutine(MovementHandler.instance.MoveDir(direction, transform));
			StartCoroutine(s);
		} else {
			MovementHandler.instance.chestMoving = false;
			animator.SetBool("jump", false);	
		}
		
		if(animator.GetCurrentAnimatorStateInfo(0).IsName("chestJump")){
			coolDown = coolDownMax;
			MovementHandler.instance.chestMoving = true;
		}
		
		if(Input.GetKeyDown(KeyCode.Space)){
			animator.SetBool("open", !animator.GetBool("open"));
		}
		
	}
	
	void OnCollisionEnter2D(Collision2D c){
		Debug.Log("collided with " + c.gameObject.name);
		if(!MovementHandler.instance.chestMoving && c.gameObject.GetComponent<EnemyHandler>()){
			EnemyHandler eH = c.gameObject.GetComponent<EnemyHandler>();
			eH.ShiftPositionMarkers(transform);
			eH.s.Stop();
		}else if(c.gameObject.tag == "Capturable"){
			StartCoroutine(Capture(c.gameObject));
		}
	}
	
	IEnumerator Capture(GameObject g){
		capturedItems.Add(g);
		animator.SetBool("jump", true);
		yield return new WaitForSeconds(0.25f);
		g.transform.renderer.enabled = false;
	}
}
