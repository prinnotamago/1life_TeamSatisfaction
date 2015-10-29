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
    private float _GROUND_SPEED = 0.2f;
    [SerializeField]
    private float _SKY_SPEED = 0.1f;

    //摩擦力
    [SerializeField]
    private float _FRICTION = 0.1f;

    //空中にいるかどうかのフラグ
    private bool _jump = true;

    //プレイヤーの各状態を表す定数
    private enum PlayerMode
    {
        _WATER = 0,
        _AIR = 1,
        _ICE = 2,
    }
    //プレイヤーの状態
    [SerializeField]
    private PlayerMode _playerMode = PlayerMode._WATER;

    //角度の限界値
    private const float _MAX_ANGLE = 0.5f;

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
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;

        //プレイヤータグの追加
        this.tag = "Player";
    }

    private void Update()
    {
        //スマホの角度
        float angle = Input.gyro.gravity.x;
        //最大値最低値の設定
        if (Input.gyro.gravity.x >= _MAX_ANGLE) angle = _MAX_ANGLE;
        if (Input.gyro.gravity.x <= -_MAX_ANGLE) angle = -_MAX_ANGLE;

        //氷状態の時はジャンプモードをオフに
        if (_playerMode == PlayerMode._ICE) _jump = false;

        //速度の代入
        float speed;
        if (!_jump) speed = _GROUND_SPEED * angle;
        else speed = _SKY_SPEED * angle;

        //水蒸気状態の時は操作が反転する
        if (_playerMode == PlayerMode._AIR) speed = speed * -1.0f;

        //プレイヤーの移動
        transform.Translate(speed, 0, 0);

        //PCでの操作確認用デバックキー
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!_jump) transform.Translate((_GROUND_SPEED * -0.3f), 0, 0);
            else transform.Translate((_SKY_SPEED * -0.3f), 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!_jump) transform.Translate((_GROUND_SPEED * 0.3f), 0, 0);
            else transform.Translate((_SKY_SPEED * 0.3f), 0, 0);
        }
        //再起動デバックキー
        //if (Input.GetMouseButtonDown(0)) Application.LoadLevel("PlayerScene");
    }

    private void OnCollisionStay(Collision collision)
    {
        //Floorタグのものにぶつかっているとき
        if (collision.gameObject.tag == "Floor")
        {
            //地面にいる判定
            _jump = false;

            //地面と同じ角度にする
            this.transform.rotation = collision.transform.rotation;

            //角度の取得
            float floorAngle = -1 * (collision.transform.eulerAngles.z);
            if (floorAngle <= -180) floorAngle = (360 + floorAngle);
            transform.Translate((floorAngle*(_FRICTION/200)), 0, 0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Floorタグのものから離れたら
        if (collision.gameObject.tag == "Floor")
        {
            //ジャンプしてる判定
            _jump = true;
        }
    }
}
