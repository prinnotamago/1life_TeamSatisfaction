using UnityEngine;
using System.Collections;

public class WaterMove : MonoBehaviour {

    [SerializeField]
    GameObject _playerObj;

    [SerializeField]
    float velocityPower = 3.0f;

    bool _isRendered = false;
    public bool _IS_RENDERED { get { return _isRendered; } }

    public Vector3 _GET_POSITION { get { return transform.position; } }

    // Use this for initialization
    void Start () {
        _playerObj = GameObject.Find(_playerObj.name);
	}
	
	// Update is called once per frame
	void Update () {
        Move();

        OnCameraWhether();
        //Debug.Log(_isRendered);
        //_isRendered = false;
        //Debug.Log(_isRendered);
    }

    void Move()
    {
        if (_playerObj != null)
        {
            Vector3 targetPos = _playerObj.transform.position;
            Vector3 myPos = transform.position;
            Vector3 targetDirection = targetPos - myPos;
            //transform.position += targetDirection / 50;
            //GetComponent<Rigidbody>().AddForce(targetDirection * 5);
            float value = 
                targetDirection.x * targetDirection.x +
                targetDirection.y * targetDirection.y +
                targetDirection.z * targetDirection.z;
            //if(value < 0.3) { power = 3.0f; }

            // 1.0f * 1.0f = 1.0f
            if (value > 1.0f)
            {
                //power = 5.0f;
                GetComponent<Rigidbody>().velocity = targetDirection * value * velocityPower;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = targetDirection.normalized * velocityPower;
            }
        }
        else
        {
            Debug.Log("_playerObj が　NULL です");
        }
    }

    void OnCameraWhether()
    {
        Camera cameraObj = _playerObj.GetComponent<CameraReference>()._CAMERA_OBJ;
        Vector3 screenPoint = cameraObj.WorldToScreenPoint(transform.position);

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.gameObject.name);

            _isRendered = hit.transform.name == transform.name;
            //Debug.Log(_isRendered);
        }
    }

    void OnWillRenderObject()
    {
        //_isRendered = true;
        //Debug.Log(_isRendered);
    }

    //private void OnBecameVisible() { _isRendered = true; }
    //private void OnBecameInvisible() { _isRendered = false; }
}
