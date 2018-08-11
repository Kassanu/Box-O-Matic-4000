using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBoxFromConveyor : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Box") {
            Box box = collision.gameObject.GetComponent<Box>();
            if (box.OnConveyor) {
                box.OnConveyor = false;
                box.transform.SetParent(null);
                box.Throw(Random.Range(0f, 3f));
            }
        }
    }
}
