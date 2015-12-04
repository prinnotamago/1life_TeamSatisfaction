using UnityEngine;
using System.Collections;

public class PlayerWaterDestroy : MonoBehaviour {

    [SerializeField]
    GameObject _playerWatersObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WaterDestroy(1);
            Debug.Log("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WaterDestroy(2);
            Debug.Log("2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WaterDestroy(3);
            Debug.Log("3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            WaterDestroy(4);
            Debug.Log("4");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            WaterDestroy(5);
            Debug.Log("5");
        }
    }

    public void WaterDestroy(int destroyNum)
    {
        GameObject parentObj = GameObject.Find(_playerWatersObj.name);
        var waters = parentObj.GetComponentsInChildren<WaterMove>();
        if(destroyNum > waters.Length) { destroyNum = waters.Length; }
        for (int i = 0; i < destroyNum; ++i)
        {         
            Destroy(waters[i].gameObject);
        }
    
        // こっちだとうまく動作しなかった
        // 上記だと処理が遅いので改善の余地あり
        //for (int i = 0; i < destroyNum; ++i)
        //{
        //    var water = parentObj.GetComponentInChildren<WaterMove>();
        //    Destroy(water.gameObject);
        //}
    }
}
