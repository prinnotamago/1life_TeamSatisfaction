using UnityEngine;
using System.Collections;

public class TextureOffsetMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = GetComponent<Renderer>().sharedMaterial.mainTextureOffset;
        offset.x = Mathf.Sin(Time.time * 2) * 0.01f;
        offset.y = Mathf.Repeat(offset.y - 0.001f, 1.0f);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

}
