using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//READ ME//
//Tagを使用しているので、床となるオブジェクトに"Floor"Tagを追加してください

//Rigidbodyの追加
[RequireComponent(typeof(Rigidbody))]

public class PlayerManagement : MonoBehaviour
{

    //速度
    [SerializeField]
    private float _groundSpeed = 0.2f;
    [SerializeField]
    private float _skySpeed = 0.1f;

    //空中にいるかどうかのフラグ
    private bool _jump = true;

    //プレイヤーの各状態を表す変数
    private enum PlayerMode
    {
        WATER = 0,
        AIR = 1,
        ICE = 2,
    }
    [SerializeField]
    private PlayerMode _playerMode = PlayerMode.WATER;

    //角度の限界値
    private const float _maxAngle = 0.5f;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        //ジャイロセンサーを有効化
        Input.gyro.enabled = true;
    }

    private void Start()
    {
        //コンポーネントを代入
        _rigidBody = GetComponent<Rigidbody>();
        //XY軸の回転を無効化
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY;
    }

    private void Update()
    {
        //スマホの角度
        float angle = Input.gyro.gravity.x;
        //最大値最低値の設定
        if (Input.gyro.gravity.x >= _maxAngle) angle = _maxAngle;
        if (Input.gyro.gravity.x <= -_maxAngle) angle = -_maxAngle;

        //氷状態の時はジャンプモードをオフに
        if (_playerMode == PlayerMode.ICE) _jump = false;

        //速度の代入
        float speed;
        if (!_jump) speed = _groundSpeed * angle;
        else speed = _skySpeed * angle;

        //水蒸気状態の時は操作が反転する
        if (_playerMode == PlayerMode.AIR) speed = speed * -1.0f;

        //プレイヤーの移動
        transform.Translate(speed, 0, 0);

        //PCでの操作確認用デバックキー
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!_jump) transform.Translate((_groundSpeed * -0.3f), 0, 0);
            else transform.Translate((_skySpeed * -0.3f), 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!_jump) transform.Translate((_groundSpeed * 0.3f), 0, 0);
            else transform.Translate((_skySpeed * 0.3f), 0, 0);
        }
        //再起動デバックキー
        //if (Input.GetMouseButtonDown(0)) Application.LoadLevel("PlayerScene");
    }

    private void OnCollisionStay(Collision collision)
    {
        //Floorタグのものにぶつかっているとき
        if (collision.gameObject.tag == "Floor")
            _jump = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        //Floorタグのものから離れたら
        if (collision.gameObject.tag == "Floor")
            _jump = true;
    }
}
