using UnityEngine;
using System.Collections;

public class PosMarkerAdjust : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D c){
		if(c.gameObject.tag == "PosMarker"){
			//position markers overlap, stop movement
			Transform parent = transform.parent;
			EnemyHandler ehandler = parent.GetComponentInChildren<EnemyHandler>();
			//EnemyHandler e = (EnemyHandler)ehandler[0];
			ehandler.SetStill(true);
			//((EnemyHandler)ehandler[0]).SetStill(true);
		}
	}
}
