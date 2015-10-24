using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

}
