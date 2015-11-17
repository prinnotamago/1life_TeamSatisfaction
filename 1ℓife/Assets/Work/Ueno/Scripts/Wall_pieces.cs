using UnityEngine;
using System.Collections;

public class Wall_pieces : MonoBehaviour 
{

    private float destroyTime = 10.0f;


	void Start () 
    {
        Destroy(this.gameObject, destroyTime);
	}
	

	void Update () 
    {
	
	}
}
