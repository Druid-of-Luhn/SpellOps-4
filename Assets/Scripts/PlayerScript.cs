using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public float speed = 5;
	public Transform fireball;
	public Transform fireballGrowObj;

	public float minFireballSize;
	public float maxFireballSize;
	public float fireballGrowthRate;

	public float moveForce;
	public float maxSpeed;
	public float jumpForce;

	private Animator animator;
	private Direction direction = Direction.RIGHT;
	private bool jump = false;
	private bool grounded = false;

	enum Direction { LEFT, RIGHT };

	private bool fireballGrowing = false;
	private float fireballSize = 0;

	private Rigidbody2D rb2d;

	private Fireball fire;

	private Transform growingFire;
	private Vector3 fireballStartScale;
	private Vector3 fireballOffset = new Vector3(0.6f, -0.3f, 0f);

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0) && fireballGrowing == false){
			fireballGrowing = true;
			growingFire = Instantiate(fireballGrowObj, transform.position, Quaternion.identity) as Transform;
			fireballStartScale = growingFire.localScale;
		}

		if(Input.GetMouseButtonUp(0)){
			Destroy(growingFire.gameObject);
			animator.SetTrigger("PlayerFire");

			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
			Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
			lookPos = lookPos - transform.position;
			float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
			Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

			Transform fire =  Instantiate(fireball,transform.position, rot) as Transform;
			fire.SendMessage("setSize", fireballSize);
			fireballSize = 0;
			fireballGrowing = false;
			Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}

		if (!grounded && rb2d.velocity.y == 0) {
			grounded = true;
		}
		if (Input.GetKeyDown(KeyCode.W) && rb2d.velocity.y <= 0) {
			jump = true;
		}
	}

	void FixedUpdate() {
		float translation = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		transform.Translate(Vector2.right * translation);

		if (jump && grounded) {
			rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			grounded = false;
			jump = false;
		}

		if(translation != 0) {
			animator.SetTrigger("PlayerRun");
			if (direction == Direction.RIGHT && translation < 0) {
				Flip ();
			} else if (direction == Direction.LEFT && translation > 0) {
				Flip ();
			}
		} else {
			animator.SetTrigger("PlayerStopRun");
		}

		if (fireballGrowing && Input.GetMouseButtonUp(0) == false) {
			growingFire.position = transform.position + fireballOffset;

			if(fireballSize == 0){
				fireballSize = minFireballSize;
			}
			if(fireballSize < maxFireballSize){
				fireballSize += fireballGrowthRate;
				if(fireballSize > maxFireballSize){
					fireballSize = maxFireballSize;
				}
				growingFire.localScale = fireballSize*fireballStartScale;
			}

		}
	}

	void Flip() {
		direction = direction == Direction.RIGHT ? Direction.LEFT : Direction.RIGHT;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		fireballOffset.x *= -1;
	}

	void hit(Vector3 force) {
		rb2d.AddForce(new Vector2(force.x, force.y), ForceMode2D.Impulse);
	}
}
