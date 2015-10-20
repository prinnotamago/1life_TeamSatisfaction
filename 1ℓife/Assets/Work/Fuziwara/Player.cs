using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;

    //重力を反転させるためのbool
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


    [SerializeField]
    private float Jump_Power = 7.0f;

    private bool _jump = false;

    //ジャンプ
    private void JumpSystem(Rigidbody _rigidbody, PlayerMode _playerMode, ref bool _jump, float Jump_Power)
    {
        if (!_jump && !(_playerMode == PlayerMode.AIR))
        {
            float _jumpPower = Jump_Power;
            _jump = true;

            _rigidbody.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
        }
    }


    private void Start()
    {
        //重力の初期値
        Physics.gravity = new Vector3(_gravityX, _gravityY, 0);
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        Debug.Log(_playerMode);
        //デバック用のキー操作
        if (!((_playerMode == PlayerMode.ICE) && _jump))
        {
            var r = Input.GetAxis("Horizontal");
            transform.Translate(r / 5, 0, 0);
        }

        if (Input.GetKey(KeyCode.J))
        {
            JumpSystem(_rigidbody, _playerMode, ref _jump, Jump_Power);
        }
        if (Input.GetKey(KeyCode.I))
        {
            _gravityMode = true;
        }
        else if (Input.GetKey(KeyCode.O))
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


    private void OnCollisionEnter(Collision collision)
    {
        //オブジェクトの判別はtagで行っているので、tag追加お願いします
        //
        //プレイヤーが着地している時
        if (collision.gameObject.tag == "Floor")
        {
            _jump = false;
        }

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
