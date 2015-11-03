using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    private GameObject _Player;

    [SerializeField]
    private float _positionY = 0.5f;
    [SerializeField]
    private float _positionZ = -1.0f;

    // Update is called once per frame
    void Update()
    {
        if(_Player != null)
        this.transform.position = _Player.transform.position + new Vector3(0, _positionY, _positionZ);
    }
}
