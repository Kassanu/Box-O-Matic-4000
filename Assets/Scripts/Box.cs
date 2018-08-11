using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

    public Player player;
    private bool isHeld = false;
    public bool IsHeld {
        get { return this.isHeld; }
        set {
            this.isHeld = value;
            this.rb2d.simulated = !value;
        }
    }
    private bool highlight = false;
    public bool Highlight {
        get { return this.highlight; }
        set {
            this.highlight = value;
            if (this.highlight) {
                GetComponent<Renderer>().material = this.highlightMaterial;
            } else {
                GetComponent<Renderer>().material = this.normalMaterial;
            }
        }
    }

    private bool onConveyor = true;
    public bool OnConveyor {
        get { return this.onConveyor; }
        set { this.onConveyor = value; }
    }


    [SerializeField]
    private Material normalMaterial;
    [SerializeField]
    private Material highlightMaterial;
    private Rigidbody2D rb2d;

    void Start() {
        this.rb2d = GetComponent<Rigidbody2D>();
        this.Highlight = false;
    }

    void Update() {
        //Debug.Log(this.gameObject.name + ": Left or Right = " + this.LeftOrRight(player.transform.position, transform.position));
       // Debug.Log(this.gameObject.name + ": Distance = " + Vector2.Distance(player.transform.position, transform.position));
    }

    public float LeftOrRight(Vector2 A, Vector2 B) {
        return A.x * -B.y + A.y * B.x;
    }

    public void Throw(float throwForce, Vector2 velocity) {
        this.rb2d.velocity = velocity;
        this.rb2d.AddForce(new Vector2(throwForce,0), ForceMode2D.Impulse);
    }

    internal void Throw(float throwForce) {
        this.rb2d.AddForce(new Vector2(throwForce, 0), ForceMode2D.Impulse);
    }

    public Vector2 GetVelocity() {
        return this.rb2d.velocity;
    }
}
