using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;
    Fire _fire;

    //重力を反転させるためのbool
    private bool _gravityMode = false;
    //重力
    [SerializeField]
    private float Down_Gravity = 50.0f;
    [SerializeField]
    private float Fly_Gravity = 1;


    //プレイヤーの各状態を表す変数
    public enum PlayerMode
    {
        WATER = 0,
        AIR = 1,
        ICE = 2,
    }
    public PlayerMode _playerMode = 0;
    public PlayerMode _PLAYER_MODE_WATER { get { return PlayerMode.WATER; } }
    public PlayerMode _PLAYER_MODE_ICE { get { return PlayerMode.ICE; } }
    public PlayerMode _PLAYER_MODE_AIR { get { return PlayerMode.AIR; } }


    //ジャンプ関係の変数
    [SerializeField]
    private float Jump_Power = 50.0f;
    private bool _jump = false;


    //状態変化時に使う変数
    [SerializeField]
    private int _changeTime = 0;


    //ジャンプ
    private void JumpSystem(ref bool _jump)
    {
        if (!_jump && _playerMode == PlayerMode.WATER)
        {
            float _jumpPower = Jump_Power;
            _jump = true;

            _rigidbody.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
        }
    }


    //重力
    private void GravitySystem(ref bool _gravityMode)
    {
        if (_gravityMode)
        {
            Physics.gravity = new Vector3(0, Fly_Gravity, 0);
        }
        else
        {
            Physics.gravity = new Vector3(0, -1 * Down_Gravity, 0);
        }

        switch (_playerMode)
        {
            case PlayerMode.WATER:
                _gravityMode = false;
                break;

            case PlayerMode.AIR:
                _gravityMode = true;
                break;

            case PlayerMode.ICE:
                _gravityMode = false;
                break;
        }
    }


    //状態変化
    private void ChangeState(Collision collision)
    {
        int _changeCount = 0;
        _changeCount++;

        //一定時間たったら
        if (_changeCount > _changeTime)
        {
            //オブジェクトの判別はtagで行っているので、tag追加お願いします
            //
            //プレイヤーが火に触れた時の処理
            if (collision.gameObject.tag == "Fire")
            {
                if (_playerMode == PlayerMode.WATER)
                {
                    _playerMode = PlayerMode.AIR;
                    _changeCount = 0;
                }
                else if (_playerMode == PlayerMode.ICE)
                {
                    _playerMode = PlayerMode.WATER;
                    _fire.Destroy();
                    _changeCount = 0;
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
                        _changeCount = 0;
                    }
                }
                else
                {
                    if (_playerMode == PlayerMode.WATER)
                    {
                        _playerMode = PlayerMode.ICE;
                        _changeCount = 0;
                    }
                }
            }
        }
    }


    private void Start()
    {
        //重力の初期値
        Physics.gravity = new Vector3(0, Down_Gravity, 0);

        _rigidbody = GetComponent<Rigidbody>();
        _fire = FindObjectOfType<Fire>();
    }


    private void Update()
    {
        //Debug.Log(_playerMode);
        //デバック用のキー操作
        //if (!((_playerMode == PlayerMode.ICE) && _jump))
        //{
        //    var r = Input.GetAxis("Horizontal");
        //    transform.Translate(r / 5, 0, 0);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            JumpSystem(ref _jump);
        }
        //if (Input.GetKey(KeyCode.I))
        //{
        //    _gravityMode = true;
        //}
        //else if (Input.GetKey(KeyCode.O))
        //{
        //    _gravityMode = false;
        //}

        //各状態の重力処理
        GravitySystem(ref _gravityMode);
    }


    private void OnCollisionStay(Collision collision)
    {
        ChangeState(collision);

        //プレイヤーが着地している時
        if (collision.gameObject.tag == "Floor")
        {
            _jump = false;
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
