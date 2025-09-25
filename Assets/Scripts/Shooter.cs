using JetBrains.Annotations;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    PlayerController playerCnt;
    public GameObject billPrefab; //Instantiateを生成する対象オブジェクト
    
    public float shootSpeed;
    public float shootDelay;
    bool inAttack; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerのPlayerController Component取得
        playerCnt = GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) Shoot();
        
    }

    public void Shoot()
    {
        if (inAttack || GameManager.bill <= 0) return;

        SoundManager.instance.SEPlay(SEType.Shoot); //お札を投げる音

        GameManager.bill--; //お札の数を減らす
        inAttack = true; //攻撃中

        //プレイヤーの角度を入手
        float angleZ = playerCnt.angleZ;
        //Rotationが扱っているQuaterion型として準備
        Quaternion q = Quaternion.Euler(0, 0, angleZ);
        
        //生成
        GameObject obj = Instantiate(billPrefab, transform.position, q);

        //生成したオブジェクトのRigidBody2D情報を取得
        Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();

        //生成したオブジェクトが向くべき方角を入手
        //Mathf関数では引数にラジアン（円周値）を与える必要がある
        float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); //x軸座標
        float y = Mathf.Sin(angleZ * Mathf.Deg2Rad); //y軸座標
        Vector2 v = (new Vector2(x, y)).normalized * shootSpeed;

        //AddForceで指定した方角に飛ばす
        rbody.AddForce(v, ForceMode2D.Impulse);

        //時間差(shootDelay秒分）で攻撃中フラグを解除
        Invoke("StopAttack", shootDelay); 
    }

    void StopAttack()
    {
        inAttack = false; 
    }
}
