using UnityEngine;
using System.Collections;

//READ ME//
//Tagを使用しているので、プレイヤーオブジェクトに"Player"Tagを追加してください

//ボックスコライダーの追加
[RequireComponent(typeof(BoxCollider))]

public class DeathZone : MonoBehaviour
{

    private BoxCollider _boxCollider;

   private void Start()
    {
        //IsTriggerをTrueにする
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Playerタグのついたオブジェクトと接触したら
        if (other.tag == "Player")
        {
            //処理を書く
            Debug.Log("ゲームオーバー");
            //プレイヤーの削除
            GameObject player = GameObject.FindWithTag("Player");
            Destroy(player);
        }
    }
}
