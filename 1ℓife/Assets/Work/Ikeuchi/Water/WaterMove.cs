using UnityEngine;
using System.Collections;

public class WaterMove : MonoBehaviour {

    [SerializeField]
    GameObject _playerObj;

    [SerializeField]
    float _velocityPower = 3.0f;

    [SerializeField]
    bool _playerHoming = false;

    bool _isRendered = false;
    public bool _IS_RENDERED { get { return _isRendered; } }

    public Vector3 _GET_POSITION { get { return transform.position - new Vector3(0.0f, transform.localScale.y / 2, 0.0f); } }

    // Use this for initialization
    void Start () {
        _playerObj = GameObject.Find(_playerObj.name);
	}
	
	// Update is called once per frame
	void Update () {
        if (_playerHoming)
        {
            Move();          
        }
        else
        {
            PlayerOnHit();
        }

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
                GetComponent<Rigidbody>().velocity = targetDirection * value * _velocityPower;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = targetDirection.normalized * _velocityPower;
            }
        }
        else
        {
            Debug.Log("_playerObj が　NULL です");
        }
    }

    // その水(中心)がカメラに映ってるかどうか調べる
    void OnCameraWhether()
    {
        Camera cameraObj = _playerObj.GetComponent<CameraReference>()._CAMERA_OBJ;
        Vector3 screenPoint = cameraObj.WorldToScreenPoint(transform.position);

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.transform.gameObject.name);

            _isRendered = hit.transform == transform;
       
            //Debug.Log(_isRendered);
        }
    }

    public void PlayerHomingOn()
    {
        _playerHoming = true;
    }

    void PlayerOnHit()
    {
        Vector3 length = transform.position - _playerObj.transform.position;
        float x = length.x * length.x;
        float y = length.y * length.y;
        float z = length.z * length.z;

        float r = transform.localScale.x / 3 + _playerObj.transform.localScale.x / 3;
        if(x + y + z < r * r * r)
        {
            GameObject parentObj = GameObject.Find("PlayerWaters");
            if (parentObj != null)
            {
                transform.parent = parentObj.transform;
                PlayerHomingOn();
            }
            else
            {
                Debug.Log("PlayerWaters が見つかりません");
            }
        }
    }

    //void OnWillRenderObject()
    //{
    //_isRendered = true;
    //Debug.Log(_isRendered);
    //}

    //private void OnBecameVisible() { _isRendered = true; }
    //private void OnBecameInvisible() { _isRendered = false; }
}
