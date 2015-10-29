using UnityEngine;
using System.Collections;

public class Wall_parts : MonoBehaviour 
{
    public GameObject Wall_Pieces;


	void Start () 
    {
	}
	

	void Update () 
    {
	}

    //床にぶつかったら当たり判定ない物に変更
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Instantiate(Wall_Pieces, new Vector3
                    (transform.position.x, transform.position.y,
                    transform.position.z), Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}
