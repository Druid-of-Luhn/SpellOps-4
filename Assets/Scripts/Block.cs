using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	private static readonly int COL_HEIGHT = 6;

	public GameObject tile1;
	public GameObject tile2;
	public GameObject tile3;
	public GameObject tile4;
	public GameObject tile5;

	public float delay;
	private float currentDelay = 0f;

	private bool falling = false;
	private Stack blocks = new Stack();

	void Start () {
		Vector3 position = transform.position;
		for (int i = 0; i < COL_HEIGHT; ++i) {
			int random = Random.Range (1, COL_HEIGHT);
			GameObject tile;
			switch (random) {
			case 1:
				tile = tile1;
				break;
			case 2:
				tile = tile2;
				break;
			case 3:
				tile = tile3;
				break;
			case 4:
				tile = tile4;
				break;
			case 5:
				tile = tile5;
				break;
			default:
				return;
			}
			GameObject block = Instantiate (tile, position, Quaternion.identity) as GameObject;
			blocks.Push(block);
			position.y -= tile1.transform.localScale.y;
		}
	}

	void FixedUpdate() {
		currentDelay += Time.deltaTime;
		// Remove the collision edge last
		if (blocks.Count == 0 && transform.childCount > 0) {
			transform.GetChild (0).GetComponent<Rigidbody2D> ().isKinematic = false;
			transform.DetachChildren ();
		}
		// Remove the bottom-most block
		if (falling && currentDelay >= delay && blocks.Count > 0) {
			ToggleKinematic(blocks.Pop() as GameObject);
			currentDelay = 0;
		}
	}

	public void fall(){
		falling = true;
	}

	private void ToggleKinematic(GameObject gameObject) {
		Rigidbody2D body = gameObject.GetComponent<Rigidbody2D> ();
		body.isKinematic = !body.isKinematic;
	}
}
