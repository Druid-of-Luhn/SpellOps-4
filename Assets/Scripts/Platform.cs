using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	private static readonly int COL_COUNT = 30;
	public GameObject columnObj;

	public float delay;
	private float currentDelay = 0f;
	private Stack cols = new Stack ();

	void Start () {
		Vector3 position = transform.position;
		AddColumn (position);
		for (int i = 0; i < COL_COUNT / 2; ++i) {
			position.x += 1.25f;
			position = AddColumn (position);
			position = AddColumn (position);
		}
	}

	void Update () {
	
	}

	void FixedUpdate(){
		currentDelay += Time.deltaTime;
		if (currentDelay >= delay && cols.Count > 0) {
			(cols.Pop () as GameObject).gameObject.GetComponent<Block> ().fall ();
			currentDelay = 0;
		}
	}

	private Vector3 AddColumn(Vector3 position) {
		cols.Push (Instantiate (columnObj, position, Quaternion.identity) as GameObject);
		position.x = -position.x;
		return position;
	}
}
