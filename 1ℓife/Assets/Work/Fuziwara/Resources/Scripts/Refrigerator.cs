using UnityEngine;
using System.Collections;
using System;

public class Refrigerator : MonoBehaviour
{
    private Player _player;
    private Battery _battery;

    [SerializeField]
    private GameObject _refrigeratorCover;
    [SerializeField]
    private ParticleSystem _snowEffect;
    public ParticleSystem SnowEffect
    {
        get
        {
            if (_snowEffect != null) { return _snowEffect; }
            _snowEffect = Resources.Load<ParticleSystem>("Material/SnowEffect");
            return _snowEffect;
        }
    }
    [SerializeField]
    private GameObject _changeAria;
    public GameObject ChangeAria
    {
        get
        {
            if (_changeAria != null) { return _changeAria; }
            _changeAria = Resources.Load<GameObject>("Material/ChangeAriaIce");
            return _changeAria;
        }
    }

    //二つのオブジェクトに接しているか確かめる
    private bool _refrigerator = false;

    //蓋が空くまでのカウント
    private int _chargeCount = 0;
    [SerializeField]
    private int _chargeLimit = 0;

    //冷蔵庫の蓋を開ける
    private bool _openCover = false;
    [SerializeField]
    private int _openAngle = 1;
    [SerializeField, Range(0, 1)]
    private float _openAngleLimit = 0;

    //冷気が出る時間
    [SerializeField]
    private int _snowLimitCount = 10;
    private int _snowCount = 0;

    //冷気エフェクト生成
    private ParticleSystem Spon_SnowEffect()
    {
        ParticleSystem snow = Instantiate(_snowEffect);
        snow.transform.Translate(new Vector3(0, 0, 0));
        return snow;
    }

    //状態変化させる透明の箱を生成
    private GameObject Spone_ChangeAria()
    {
        GameObject aria = Instantiate(_changeAria.gameObject);
        aria.transform.Translate(new Vector3(0, 0, 0));
        return aria;
    }

    private void Open_Refrigirator()
    {
        //一定時間留まったら蓋が開いた
        _chargeCount += (_refrigerator && _battery._Battery &&
                         _openCover == false && _snowEffect.isStopped) ? 1 : 0;
        if (_chargeCount >= _chargeLimit * 60)
        {
            _chargeCount = 0;
            _openCover = true;
        }
        if (_openCover)
        {
            _refrigeratorCover.transform.Rotate(new Vector3(0, -1 * _openAngle, 0));

            if (_refrigeratorCover.transform.localRotation.y <= -1 * _openAngleLimit)
            {
                _snowEffect = Spon_SnowEffect();
                if (_snowEffect.isStopped)
                {
                    _snowEffect.Play();
                }
                _changeAria = Spone_ChangeAria();
                _openCover = false;
            }
        }

        //蓋が開いたあとに状態変化のボックスを作る
        _snowCount += (_snowEffect.isPlaying) ? 1 : 0;
        if (_snowCount > _snowLimitCount * 60)
        {
            _snowCount = 0;
            _snowEffect.Stop();

            if (_player._playerMode == _player._PLAYER_MODE_ICE && _snowEffect.isPlaying)
            {
                Destroy(_changeAria);
            }
        }
    }


    void Start()
    {
        _player = FindObjectOfType<Player>();
        _battery = FindObjectOfType<Battery>();
    }

    void Update()
    {
        Open_Refrigirator();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_player._playerMode == _player._PLAYER_MODE_WATER)
            {
                _refrigerator = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _refrigerator = false;
        }
    }
}
