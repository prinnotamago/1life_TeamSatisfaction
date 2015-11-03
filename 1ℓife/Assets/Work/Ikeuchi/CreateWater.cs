using UnityEngine;
using System.Collections;

public class CreateWater : MonoBehaviour {

    [SerializeField]
    GameObject _createObj;

    [SerializeField]
    GameObject _parentObj;

    [SerializeField]
    string _createObjName = "Water";

    [SerializeField]
    int _createNum = 0;

	// Use this for initialization
    void Awake()
    {
        for (int i = 0; i < _createNum; i++)
        {
            Create(i, i);
        }
    }

    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Create(float angle, int num)
    {
        bool createObjCheck = _createObj != null;
        bool parentObjCheck = _parentObj != null;
        bool createObjNameCheck = _createObjName != null;
        bool allCheck = createObjCheck && parentObjCheck && createObjNameCheck;
        if (allCheck) {
            GameObject obj = Instantiate(_createObj);
            obj.transform.SetParent(_parentObj.transform);
            obj.name = _createObjName + num;
            obj.transform.position += new Vector3(Mathf.Cos(angle) * angle / 30, 0.0f, Mathf.Sin(angle) * angle / 30);
        }
        else
        {
            Debug.Log("_createObj、_parentObj、_createObjNameのいずれかが NULL です");
        }
    }
}
