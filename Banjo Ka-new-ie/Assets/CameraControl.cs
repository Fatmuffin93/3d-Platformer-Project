using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject player;
	public Vector3 myPos;
	private Quaternion rotation = new Quaternion(0.4f,0f,0f,0.9f);
	private bool rotationPositive = false;
	private bool rotationNegative = false;
	private int rotationCountdown = 45;
	private bool firstPersonView = false;
	private bool changeCamera = false;
	private Vector3 mouse_pos;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!firstPersonView && !changeCamera) {
			//third-person camera
			rotation = transform.rotation;
			if (rotationPositive && rotationCountdown > 0) {
				myPos = RotateAroundPoint (myPos, Vector3.zero, Quaternion.Euler (0, 2f, 0));
				transform.RotateAround (player.transform.position, Vector3.up, 2f);
				rotationCountdown--;
			}

			if (rotationNegative && rotationCountdown > 0) {
				myPos = RotateAroundPoint (myPos, Vector3.zero, Quaternion.Euler (0, -2f, 0));
				transform.RotateAround (player.transform.position, Vector3.up, -2f);
				rotationCountdown--;
			}

			if (rotationCountdown <= 0) {
				rotationNegative = false;
				rotationPositive = false;
				rotationCountdown = 45;
			}
			transform.position = player.transform.position + myPos;

			if (Input.GetKeyDown (KeyCode.E) && !rotationNegative) {
				rotationPositive = true;
			}

			if (Input.GetKeyDown (KeyCode.Q) && !rotationPositive) {
				rotationNegative = true;
			}

		} else if (firstPersonView && !changeCamera) {

			//first-person camera
			mouse_pos = Input.mousePosition;
			mouse_pos.x -= Screen.width/2;
			mouse_pos.y -= Screen.height/2;
			/*var screenPoint = Camera.main.WorldToScreenPoint (transform.localPosition);
			var offset = new Vector3 (mouse_pos.x - screenPoint.x, 0, mouse_pos.y - screenPoint.y);
			Debug.Log (offset);
			var angle = Mathf.Atan2 (offset.z, offset.x) * Mathf.Rad2Deg;*/

			transform.rotation = Quaternion.Euler (-mouse_pos.y * Time.deltaTime * 5, mouse_pos.x * Time.deltaTime * 5, 0);
            transform.position = player.transform.position;
            /*Vector3 MousePosition = Input.mousePosition;
			MousePosition.x = (Screen.height/2) - Input.mousePosition.y;
			MousePosition.y = -(Screen.width/2) + Input.mousePosition.x;
			transform.Rotate(MousePosition * Time.deltaTime * 0.2f, Space.Self);
			transform.position = player.transform.position;*/

        } else if (changeCamera && !firstPersonView) {
			//change to first-person view
			if(!player.GetComponent<Player>().firstPersonView){
				player.GetComponent<Player>().firstPersonView = true;
			}
			if(transform.position != player.transform.position){
				transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 20 * Time.deltaTime);
				transform.rotation = rotation;
			} else{
				firstPersonView = true;
				changeCamera = false;
			}

		} else {

			//change to third-person view
			if(transform.position != player.transform.position + myPos){
				transform.position = Vector3.MoveTowards(transform.position, player.transform.position + myPos, 20 * Time.deltaTime);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 3);
			} else{
				firstPersonView = false;
				changeCamera = false;
				player.GetComponent<Player>().firstPersonView = false;
			}

		}
		if (Input.GetKeyDown (KeyCode.F) && !changeCamera) {
			changeCamera = true;
		}
	}

	Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle){
		return angle * ( point - pivot) + pivot;
	}

}