using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	public GameObject collisionParticle;
	public float speed;
	public float force;

	private float size;

	void Start () {
		// Set the initial fireball velocity
		GetComponent<Rigidbody2D>().velocity = transform.right * speed;
	}
	
	void Update () {
		
	}

	void setSize(float size) {
		this.size = size;
		transform.localScale *= size;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.SendMessage("hit", transform.rotation * Vector3.right * size * force);
		}
		Instantiate(collisionParticle, transform.position, Quaternion.identity);
		// Remove the fireball
		Destroy(gameObject);
	}
}
