using UnityEngine;
using System.Collections;

public class CreateWater : MonoBehaviour {

    [SerializeField]
    GameObject _createObj;

    [SerializeField]
    GameObject _parentObj;

    [SerializeField]
    GameObject _createPositionObj;

    [SerializeField]
    string _createObjName = "Water";

    [SerializeField]
    int _createNum = 0;

    [SerializeField]
    bool _playerHoming = false;

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
        // 何か１つでも欠けていたら(NULL)ならログを返す
        bool createObjCheck = _createObj != null;
        bool parentObjCheck = _parentObj != null;
        bool createObjNameCheck = _createObjName != null;
        bool allCheck = createObjCheck && parentObjCheck && createObjNameCheck;
        if (allCheck)
        {
            GameObject Createobj = Instantiate(_createObj);
            Createobj.transform.SetParent(_parentObj.transform);
            Createobj.name = _createObjName + num + "[" + _createPositionObj.name + "]";
            if (_playerHoming)
            {
                // プレイヤーをホーミングする水を作るなら
                Createobj.transform.position += _createPositionObj.transform.position + new Vector3(Mathf.Cos(angle) * angle / 30, 0.0f, Mathf.Sin(angle) * angle / 30);
                Createobj.GetComponent<WaterMove>().PlayerHomingOn();
            }
            else
            {
                // 回復用の水を作るなら
                Createobj.transform.position += _createPositionObj.transform.position;
            }
        }
        else
        {
            Debug.Log("_createObj、_parentObj、_createObjNameのいずれかが NULL です");
        }
    }
}
