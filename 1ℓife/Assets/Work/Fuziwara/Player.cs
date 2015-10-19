using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private bool _gravityMode = false;

    [SerializeField]
    private float _gravityX = 0.0f;
    [SerializeField]
    private float _gravityY = -5.0f;


    //プレイヤーの各状態を表す変数
    private enum PlayerMode
    {
        WATER = 0,
        AIR = 1,
        ICE = 2,
    }
    private PlayerMode _playerMode = 0;

    
    private void Start()
    {
        //重力の初期値
        Physics.gravity = new Vector3(_gravityX, _gravityY, 0);
    }


    private void Update()
    {
        Debug.Log(_playerMode);
        //デバック用のキー操作
        var r = Input.GetAxis("Horizontal");
        transform.Translate(r / 5, 0, 0);
        if(Input.GetKey(KeyCode.I))
        {
            _gravityMode = true;
        }
        else if(Input.GetKey(KeyCode.O))
        {
            _gravityMode = false;
        }

        //プレイヤーの各状態の処理
        switch (_playerMode)
        {
            case PlayerMode.WATER:

                break;

            case PlayerMode.AIR:

                break;
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        //オブジェクトの判別はtagで行っているので、tag追加お願いします
        //
        //プレイヤーが火に触れた時の処理
        if (collision.gameObject.tag == "Fire")
        {
            if (!(_playerMode == PlayerMode.AIR))
            {
                _playerMode = PlayerMode.AIR;
            }
        }

        //プレイヤーが氷に触れた時の処理
        if (collision.gameObject.tag == "Ice")
        {
            if (_gravityMode)
            {
                if (_playerMode == PlayerMode.AIR)
                {
                    _playerMode = PlayerMode.WATER;
                }
            }
            else
            {
                if (_playerMode == PlayerMode.WATER)
                {
                    _playerMode = PlayerMode.ICE;
                }
            }
        }


        //プレイヤーが壊せる壁に触れた時の処理
        if (collision.gameObject.tag == "BreakWall")
        {
            if (_playerMode == PlayerMode.ICE)
            {
                _playerMode = PlayerMode.WATER;
            }
        }
    }
}
