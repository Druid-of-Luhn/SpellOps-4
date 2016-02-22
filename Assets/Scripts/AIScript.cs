using UnityEngine;
using System.Collections;

public class AIScript : MonoBehaviour {
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

	private bool attackStart = false;
	private bool attackEnd = false;

	private float delay = 1f;
	private float current = 0f;

	private GameObject wiz;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		wiz = GameObject.Find ("Wizard");
	}

	// Update is called once per frame
	void Update () {


//		attackStart = true;
//
//		if (attackStart && !fireballGrowing) {
//			attackStart = false;
//			fireballGrowing = true;
//			growingFire = Instantiate(fireballGrowObj, transform.position, Quaternion.identity) as Transform;
//			fireballStartScale = growingFire.localScale;
//		}
//
//		attackEnd = true;
//
//		if(attackEnd){
//			attackEnd = false;
//			Destroy(growingFire.gameObject);
//			animator.SetTrigger("CastTrigger");
//
//			Vector3 wizPos = wiz.transform.position;
//			wizPos = wizPos - transform.position;
//			float angle = Mathf.Atan2(wizPos.y, wizPos.x) * Mathf.Rad2Deg;
//			Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
//
//			Transform fire =  Instantiate(fireball,transform.position, rot) as Transform;
//			fire.SendMessage("setSize", fireballSize);
//			fireballSize = 0;
//			fireballGrowing = false;
//			Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), GetComponent<Collider2D>());
//		}

		if (!grounded && rb2d.velocity.y == 0) {
			grounded = true;
			jump = true;
		}
	}

	void FixedUpdate() {
		current += Time.deltaTime;
		if (current >= delay) {
			current = 0;
			Vector3 target = wiz.transform.position - transform.position;
			float angle = Mathf.Atan2 (target.y, target.x) * Mathf.Rad2Deg;
			Transform fire = Instantiate (fireball, transform.position, Quaternion.Euler(0, 0, angle)) as Transform;
			fire.SendMessage ("setSize", maxFireballSize);
			Physics2D.IgnoreCollision(fire.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
		float translation = 0f;
		transform.Translate(Vector2.right * translation);

		if (jump && grounded) {
			rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
			grounded = false;
			jump = false;
		}

		if(translation != 0) {
			animator.SetTrigger("RunTrigger");
			if (direction == Direction.RIGHT && translation < 0) {
				Flip ();
			} else if (direction == Direction.LEFT && translation > 0) {
				Flip ();
			}
		}

//		if (fireballGrowing && !attackEnd) {
//			growingFire.position = transform.position + fireballOffset;
//
//			if(fireballSize == 0){
//				fireballSize = minFireballSize;
//			}
//			if(fireballSize < maxFireballSize){
//				fireballSize += fireballGrowthRate;
//				if(fireballSize > maxFireballSize){
//					fireballSize = maxFireballSize;
//				}
//				growingFire.localScale = fireballSize*fireballStartScale;
//			}
//
//		}
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
