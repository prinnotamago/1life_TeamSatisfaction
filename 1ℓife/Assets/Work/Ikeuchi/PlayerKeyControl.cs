using UnityEngine;
using System.Collections;

public class PlayerKeyControl : MonoBehaviour {

    [SerializeField]
    float _moveValue = 0.05f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0.0f, 0.0f, _moveValue);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0.0f, 0.0f, -_moveValue);

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-_moveValue, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(_moveValue, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(0.0f, 200.0f, 0.0f);
        }
    }
}
