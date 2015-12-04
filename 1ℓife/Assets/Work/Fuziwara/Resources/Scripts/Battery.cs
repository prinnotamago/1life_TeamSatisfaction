using UnityEngine;
using System.Collections;

public class Battery : MonoBehaviour
{
    private Player _player;

    private bool _battery = false;
    public bool _Battery { get { return _battery; } }


    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_player._playerMode == _player._PLAYER_MODE_WATER)
            {
                _battery = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _battery = false;
        }
    }
}
