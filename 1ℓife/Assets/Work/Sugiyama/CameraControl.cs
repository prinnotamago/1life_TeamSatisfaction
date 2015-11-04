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

    [SerializeField]
    private float _airPositionY = -0.5f;

    private Player _fujiwaraPlayer;

    void Start()
    {
        _fujiwaraPlayer = _Player.GetComponent<Player>();
    }

    void Update()
    {

        if (_Player != null)
        {
            //水蒸気じゃなければ
           if(_fujiwaraPlayer._playerMode != Player.PlayerMode.AIR)
                this.transform.position = _Player.transform.position + new Vector3(0, _positionY, _positionZ);
           //水蒸気だったら
           else this.transform.position = _Player.transform.position + new Vector3(0, _airPositionY, _positionZ);
        }
        else Debug.Log("プレイヤーオブジェクトをドラッグ＆ドロップしてください");
    }
}
