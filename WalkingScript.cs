
public class WalkingScript : MonoBehaviour {
	public GameObject body;
	public GameObject leftFoot;
	public GameObject rightFoot;

	private float normalDistanceX;
	private float normalDistanceZ;

	private float normalPositionX;
	private float normalPositionZ;

	void Start () {
		Vector3 leftFootPosition = leftFoot.transform.position;
		Vector3 rightFootPosition = rightFoot.transform.position;
		normalPositionX = (leftFootPosition.x + rightFootPosition.x) / 2;
		normalPositionZ = (leftFootPosition.z + rightFootPosition.z) / 2;
		normalDistanceX = Mathf.Abs(leftFootPosition.x - rightFootPosition.x);
		normalDistanceZ = Mathf.Abs(leftFootPosition.x - rightFootPosition.x);
	}

	void Update () {
		Vector3 leftFootPosition = leftFoot.transform.position;
		Vector3 rightFootPosition = rightFoot.transform.position;

		float dx = Mathf.Abs(leftFootPosition.x - rightFootPosition.x);
		float dz = Mathf.Abs(leftFootPosition.z - rightFootPosition.z);

		if (dx <= normalDistanceX) {
			normalDistanceX = (normalDistanceX + dx) / 2;
			normalPositionX = (normalPositionX + (leftFootPosition.x + rightFootPosition.x) / 2) / 2;
		} else {
			float ldx = leftFootPosition.x - normalPositionX;
			float rdx = rightFootPosition.x - normalPositionX;

			if(Mathf.Abs (ldx) > Mathf.Abs (rdx)) {
				// Move left
				body.transform.position.x += 10 * Time.deltaTime;
			} else {
				// Move right
				body.transform.position.x -= 10 * Time.deltaTime;
			}
		}

		if (dz <= normalDistanceZ) {
			normalDistanceZ = (normalDistanceZ + dz) / 2;
			normalPositionZ = (normalPositionZ + (leftFootPosition.z + rightFootPosition.z) / 2) / 2;
		} else {
			float ldz = leftFootPosition.z - normalPositionZ;
			float rdz = rightFootPosition.z - normalPositionZ;

			if(Mathf.Abs (ldz) > Mathf.Abs (rdz)) {
				if(ldz < 0) {
					// Move forward
					body.transform.position.z += 10 * Time.deltaTime;
				} else {
					// Move backward
					body.transform.position.z -= 10 * Time.deltaTime;
				}
			} else {
				if(rdz < 0) {
					// Move forward
					body.transform.position.z += 10 * Time.deltaTime;
				} else {
					// Move backward
					body.transform.position.z -= 10 * Time.deltaTime;
				}
			}
		}
	}
}
