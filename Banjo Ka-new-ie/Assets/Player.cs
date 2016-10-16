using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private float speed = 5.0f;
	private Vector3 mouse_pos;
	public bool firstPersonView;
    private GameObject tree;
	
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

		if (!firstPersonView && tree == null) {
			//rotate the player
			mouse_pos = Input.mousePosition;
			var screenPoint = Camera.main.WorldToScreenPoint (transform.localPosition);
			var offset = new Vector3 (mouse_pos.x - screenPoint.x, 0, mouse_pos.y - screenPoint.y);
			var angle = Mathf.Atan2 (offset.z, offset.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0, -angle + 90, 0);
			
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				//move up
				transform.Translate (Vector3.forward * Time.deltaTime * speed);
			}
			if (Input.GetKey (KeyCode.Space)) {
				//move up
				transform.GetComponent<Rigidbody>().velocity = new Vector3 (0, 7, 0);
			}
			/*if (transform.GetComponent<Rigidbody>().velocity.y >= 0) {
				speed = 5f;
			} else {
				speed = 0.5f;
			}*/
		} else if (tree != null) {
            if (Input.GetKey(KeyCode.D)){
                transform.RotateAround(tree.transform.position, Vector3.up, -315f * Time.deltaTime);   
            }
            if (Input.GetKey(KeyCode.A)){
                transform.RotateAround(tree.transform.position, Vector3.up, 315f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.W)){
                transform.Translate(Vector3.up * Time.deltaTime * 5f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.up * Time.deltaTime * -5f);
            }
            if (Input.GetKeyDown(KeyCode.Space)){
                tree = null;
                GetComponent<Rigidbody>().useGravity = true;
                transform.GetComponent<Rigidbody>().velocity = new Vector3(-5, 3, 0);
            }
        }
        else {

		}
	}

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Tree"){
            GetComponent<Rigidbody>().useGravity = false;
            tree = col.gameObject;
            transform.LookAt(new Vector3(col.gameObject.transform.position.x, transform.position.y, col.gameObject.transform.position.z));
        }
    }

    void OnCollisionExit(Collision col){
        if (col.gameObject.tag == "Tree"){
            tree = null;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}