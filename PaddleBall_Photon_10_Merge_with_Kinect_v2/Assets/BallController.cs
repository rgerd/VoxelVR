using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	
	public float speed = 10f;
	private Vector3 velocity;
	public GameObject burst;
	
	void Start () {
		velocity = new Vector3 (speed, speed, speed);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (velocity * Time.deltaTime);
		
		/*Vector3 pos = transform.position;

		if (pos.z > eastWall.transform.position.z) {
			transform.position =  ( new Vector3 (transform.position.x, transform.position.y, eastWall.transform.position.z));
			velocity.z *= -1;
			Instantiate(burst, transform.position, new Quaternion());
		}
		if (pos.z < westWall.transform.position.z) {
			transform.position = ( new Vector3 (transform.position.x, transform.position.y, westWall.transform.position.z));
			velocity.z *= -1;
			Instantiate(burst, transform.position, new Quaternion());
		}
		if (pos.x < northWall.transform.position.x) {
			transform.position = ( new Vector3 (northWall.transform.position.x, transform.position.y, transform.position.z));
			velocity.x *= -1;
			Instantiate(burst, transform.position, new Quaternion());
		}
		if (pos.x > southWall.transform.position.x) {
			transform.position = ( new Vector3 (southWall.transform.position.x, transform.position.y, transform.position.z));
			velocity.x *= -1;
			Instantiate(burst, transform.position, new Quaternion());
		}
		if (pos.y > ceiling.transform.position.y) {
			transform.position = ( new Vector3 (transform.position.x, ceiling.transform.position.y, transform.position.z));
			velocity.y *= -1;
			Instantiate(burst, transform.position, new Quaternion());
		}
		if (pos.y < floor.transform.position.y) {
			transform.position = ( new Vector3 (transform.position.x, floor.transform.position.y, transform.position.z));
			velocity.y *= -1;
			Instantiate(burst, transform.position, new Quaternion());
		}*/
	}
		
		void OnTriggerEnter(Collider other) {
			
			if (other.gameObject.CompareTag ("East_West Wall")) {
				velocity.z = -velocity.z;
				Instantiate(burst, transform.position, new Quaternion());
			} else if (other.gameObject.CompareTag ("Ceiling_Floor Wall")) {
				velocity.y = -velocity.y;
				Instantiate(burst, transform.position, new Quaternion());
			} else if (other.gameObject.CompareTag ("North_South Wall")) {
				velocity.x = -velocity.x;
				Instantiate(burst, transform.position, new Quaternion());
			}
		}
	}
