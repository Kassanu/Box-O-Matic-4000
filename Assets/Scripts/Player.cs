using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 10f;
    public float speed = 10f;
    public float jumpForce = 10f;
    public float pickUpRadius = 1f;
    public float throwForce = 10f;
    public bool facingRight = false;
    public bool FacingRight {
        get { return this.facingRight; }
        set {
            this.facingRight = value;
            this.UpdateHoldPosition();
        }
    }

    private Rigidbody2D rb2d;
    [SerializeField]
    private float rayLength = 1f;

    public Box heldBox = null;
    private Box closestBox = null;

    [SerializeField]
    private Transform holdPoint;

    void Start() {
        this.FacingRight = false;
        this.rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Debug.DrawRay(transform.position, Vector2.down * this.rayLength, Color.green);
        
        if (Input.GetKeyDown(KeyCode.E)) {
            this.Interact();
        }

    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetButton("Jump")) {
            this.Jump();
        }
        //this.rb2d.AddForce(Vector2.right * moveHorizontal * this.speed);
        this.rb2d.velocity = new Vector2(moveHorizontal * this.maxSpeed, this.rb2d.velocity.y);
        this.clampHorizontalSpeed();
        this.Flip(moveHorizontal);

        if (this.heldBox == null) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, this.pickUpRadius, 1 << LayerMask.NameToLayer("Entities"));
            List<Box> boxesInRange = new List<Box>();
            foreach (Collider2D collider in colliders) {
                if (collider.gameObject.tag == "Box") {
                    boxesInRange.Add(collider.gameObject.GetComponent<Box>());
                }
            }

            boxesInRange = boxesInRange.OrderBy(x => x, new BoxOrientationComparer(this)).ThenBy(x => x, new BoxDistanceComparer(this)).ToList();

            if (boxesInRange.Count > 0) {
                this.setClosestBox(boxesInRange[0]);
            } else {
                this.setClosestBox(null);
            }
        }
    }

    void Interact() {
        if (this.heldBox == null && this.closestBox != null) {
            this.PickUpBox();
        }
        else if (this.heldBox != null) {
            this.DropBox();
        }
    }

    private void DropBox() {
        Debug.Log("Dropping " + this.heldBox.gameObject.name);
        this.heldBox.transform.SetParent(null);
        this.heldBox.IsHeld = false;
        this.heldBox.Throw((this.FacingRight?this.throwForce:-this.throwForce), new Vector2(this.rb2d.velocity.x, this.rb2d.velocity.y));
        this.heldBox = null;
    }

    void PickUpBox() {
        Debug.Log("Picking up " + this.closestBox.gameObject.name);
        this.heldBox = this.closestBox;
        this.setClosestBox(null);
        this.heldBox.transform.SetParent(transform);
        this.heldBox.transform.position = this.holdPoint.position;
        this.heldBox.IsHeld = true;
    }

    void Jump() {
        if (this.isGrounded()) {
            this.rb2d.velocity = new Vector2(this.rb2d.velocity.y, 0);
            this.rb2d.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);
        }
    }

    void Flip(float moveHorizontal) {
        if (moveHorizontal > 0 && !this.FacingRight || moveHorizontal < 0 && this.FacingRight) {
            this.FacingRight = !this.FacingRight;
        }
    }

    private void UpdateHoldPosition() {
        this.holdPoint.transform.localPosition = new Vector3(( this.facingRight ? 1 : -1 ), 0, 0);
        if (this.heldBox != null) {
            this.heldBox.transform.position = this.holdPoint.position;
        }
    }

    void clampHorizontalSpeed() {
        var v = this.rb2d.velocity;
        var yd = v.y;
        v.y = 0f;
        v = Vector3.ClampMagnitude(v, this.maxSpeed);
        v.y = yd;
        this.rb2d.velocity = v;
    }

    private bool isGrounded() {
        return Physics2D.Raycast(transform.position, Vector2.down, this.rayLength);
        
    }

    void setClosestBox(Box b) {
        if (this.closestBox != null) {
            this.closestBox.Highlight = false;
        }
        this.closestBox = b;
        if (this.closestBox != null) {
            this.closestBox.Highlight = true;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, this.pickUpRadius);
    }

}
