using UnityEngine;
using System.Collections;

public class PondWater : MonoBehaviour {

    Vector3 _targetWaterPos;
    public Vector3 _TargetWaterPos {
        get { return _targetWaterPos; }
        set { _targetWaterPos = value; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    void Move()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_TargetWaterPos - transform.position), 0.07f);
        GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.x, transform.forward.y, 0.0f) * 1.1f;
        //transform.position += new Vector3(transform.forward.x, transform.forward.y, 0.0f) * 0.05f;
    }
}
