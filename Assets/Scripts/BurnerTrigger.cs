using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnerTrigger : MonoBehaviour {

    [SerializeField]
    Burner burner;

    private void OnTriggerEnter2D(Collider2D other) {
        burner.triggerFlameBurst();
        if (other.tag == "Player") {
            other.GetComponent<Player>().Kill("burner");
        } else {
            Destroy(other.gameObject);
        }
        print("Trigger Entered");
    }

}