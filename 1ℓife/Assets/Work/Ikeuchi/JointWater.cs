using UnityEngine;
using System.Collections;

public class JointWater : MonoBehaviour {

    [SerializeField]
    GameObject _jointObj;

	// Use this for initialization
	void Start () {
        if (!_jointObj)
        {
            _jointObj = GameObject.Find(_jointObj.name);
            GetComponent<SpringJoint>().connectedBody = _jointObj.GetComponent<Rigidbody>();
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
