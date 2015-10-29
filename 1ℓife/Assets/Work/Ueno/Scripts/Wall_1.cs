using UnityEngine;
using System.Collections;

public class Wall_1 : MonoBehaviour 
{
    public GameObject Wall_parts;
    private float x1 = 0.25f;   //0.25f or 0.1f


	void Start () 
    {
	}
	

	void Update () 
    {
	}


    //プレイヤーにぶつかったら壊れる
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);

            Instantiate(Wall_parts, new Vector3
                    (transform.position.x, transform.position.y,
                    transform.position.z), Quaternion.identity);

            Instantiate(Wall_parts, new Vector3
                    (transform.position.x + x1, transform.position.y,
                    transform.position.z), Quaternion.identity);

            Instantiate(Wall_parts, new Vector3
                    (transform.position.x - x1, transform.position.y,
                    transform.position.z), Quaternion.identity);

        }
        //Wall_partsBox縦3*横3=9
        //現在Wall_partsBox3つ      Wall_parts計27個
        //27*5=135  同時破壊までOK 6から若干処理落ち
    }
}
