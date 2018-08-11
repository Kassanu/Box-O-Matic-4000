using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

    public float beltSpeed = 1f;

	void Start () {
		
	}
	
	void FixedUpdate () {
        foreach (Transform child in transform) {
            if (child.tag == "Box") {
                Box box = child.gameObject.GetComponent<Box>();
                box.Throw(0, new Vector2(this.beltSpeed, box.GetVelocity().y));
            }
        }
    }
}
