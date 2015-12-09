using UnityEngine;
using System.Collections;

public class Lighter : MonoBehaviour 
{
    public GameObject Zippo;
    Quaternion quat = Quaternion.Euler(0, 0, 0);

    private float destroyTime = 0.05f;

    //プレイヤーがぶつかったら倒れる ぶつかったらオブジェクトを差し替え
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(Zippo, new Vector3
                    (transform.position.x, transform.position.y,
                    transform.position.z), quat);

            Destroy(this.gameObject, destroyTime);
        }
    }
}
