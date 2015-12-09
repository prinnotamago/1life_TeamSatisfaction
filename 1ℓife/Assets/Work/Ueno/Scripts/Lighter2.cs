using UnityEngine;
using System.Collections;

public class Lighter2 : MonoBehaviour
{
    public GameObject Fire;
    public GameObject Zippo;
    Quaternion quat = Quaternion.Euler(0, 0, -90);

    float Time = 0;
    private float destroyTime = 0.05f;

    void Start()
    {

    }

    void Update()
    {
        Time++;

        //倒れ終わったら火
        if (Time == 50.0)
        {
            Instantiate(Fire, new Vector3
            (transform.position.x + 1.0f, transform.position.y,
            transform.position.z), quat);

            //倒れたらオブジェクトを差し替え
            Instantiate(Zippo, new Vector3
            (transform.position.x, transform.position.y,
            transform.position.z), quat);

            Destroy(this.gameObject, destroyTime);
        }
    }
}
