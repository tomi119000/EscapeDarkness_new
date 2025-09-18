using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーのステータス")]
    public float playerSpeed = 3.0f;

    float axisH; // Horizontal axis value
    float axisV; // Vertical axis value

    [Header("プレイヤーの角度計算用")]
    public float angleZ = -90f; // Rotation angle around the Z-axis

    [Header("オン/オフの対象スポットライト")]
    public GameObject spotLight; //対象のスポットライト

    bool inDamage;  //ダメージ中かどうか

    //Components
    Rigidbody2D rbody;
    Animator anime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Componentの取得
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        //スポットライトを所持していればスポットライトを表示
        if (GameManager.hasSpotLight)
        {
            spotLight.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;

        Move();
        angleZ = GetAngle();
        Animation();
    }

    private void FixedUpdate()
    {
        if(GameManager.gameState == GameState.gameover)
        {
            GameOver(); 
        }
        if(inDamage) //inDamage flagがtrueの場合、何もしない
        {
            //SInメソッドの角度情報にゲーム開始からの経過時間(Time.timeメソッド)を与える
            //timeを50倍する
            float val = Mathf.Sin(Time.time * 50); 
            
            if(val > 0)
            {
                //Player gameobjectのSpriteRenderer componentをオン/オフ
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

                //入力によるVelocityが入らないようにreturn処理
            return; 
        }
        //入力状況に応じてプレイヤーを動かす
        // .normalizedで正規化（斜め方向も１）している
        rbody.linearVelocity = (new Vector2(axisH, axisV)).normalized * playerSpeed;
    }

    public void Move()
    {
        //axisHとaxisVにそれぞれHorizontalとVerticalの入力値を代入
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");
    }

    //プレイヤーの角度を取得するメソッド
    public float GetAngle()
    {
        //現在座標の取得（Vector2型）
        Vector2 fromPos = transform.position;
        //その瞬間のキー入力値(axisH, axisV）に応じた予測座標の取得
        Vector2 toPos = new Vector2(fromPos.x + axisH, fromPos.y + axisV);
        
        float angle; //returnする値の準備

        //もしも何かしらの入力があれば新たに角度算出
        if (axisH != 0 || axisV != 0)
        {
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;

            //第一引数に高さy、第二引数に底辺xを与えると角度をラジアン形式で算出
            //　ラジアン＝円周の長さ、で表現
            float rad = Mathf.Atan2(dirY, dirX);

            //ラジアン値をオイラー値（Degree）に変換
            angle = rad * Mathf.Rad2Deg;
        }
        //何も入力されていなければ、前フレームの角度情報を据え置き
        else
        {
            angle = angleZ;
        }
        return angle;
    }
    void Animation()
    {
        //何らかの入力がある場合
        if (axisH != 0 || axisV != 0)
        {
            //ひとまずRunアニメを走らせる
            anime.SetBool("run", true);

            //angleZを利用して方角を決める パラメータdirection int型
            //int型のdirection 下：0　上：1　右：2　左：それ以外
            if (angleZ > -135f && angleZ < -45f) //下方向
            {
                anime.SetInteger("direction", 0);
            }
            else if (angleZ >= -45f && angleZ <= 45f) //右方向
            {
                anime.SetInteger("direction", 2);
                transform.localScale = new Vector2(1, 1);
            }
            else if (angleZ > 45f && angleZ < 135f) //上方向
            {
                anime.SetInteger("direction", 1);
            }
            else //左方向
            {
                anime.SetInteger("direction", 3);
                transform.localScale = new Vector2(-1, 1);
            }
        }
        else //何も入力が無い場合
        {
            anime.SetBool("run", false); //走るフラグをOff
        }
    }

    //ぶつかったときにCollision（反発）
    private void OnCollisionEnter2D(Collision2D collision)
    {
       //ぶつかったオブジェクトがEnemyだった場合
       if(collision.gameObject.CompareTag("Enemy"))
        {
            GetDamage(collision.gameObject);  //damage処理
        }
    }

    void GetDamage(GameObject enemy)
    {
        if (GameManager.gameState != GameState.playing) return;

        GameManager.playerHP--; //playerHPを１減らす

        if(GameManager.playerHP > 0)
        {
            //そこまでのプライヤーの動きをいったんストップ
            rbody.linearVelocity = Vector2.zero; //new Vector2(0,0)と同じ

            //playerと敵との差を取得し、方向を決める
            Vector3 v = (transform.position - enemy.transform.position).normalized;
            //決まった方向に押される
            rbody.AddForce(v*4, ForceMode2D.Impulse);

            //点滅するためのフラグ
            inDamage = true;

            //時間差で0.25秒後に点滅フラグ解除（DamageEndメソッドの発動遅らせる）
            Invoke("DamageEnd", 0.25f);
        }
        else
        {
            //残HPが残っていなければゲームオーバー
            GameOver();
        }
    }

    void DamageEnd()
    {
        inDamage = false; //点滅ダメージフラグを解除
        //プレイヤーを確実に表示
        gameObject.GetComponent<SpriteRenderer>().enabled = true; 
    }

    void GameOver()
    {
        //Gameの状態を変える. static変数なのでGameState型を明示
        GameManager.gameState = GameState.gameover;

        //GameOver演出. "this.gameObject"は省略可能
        //当たり判定無効化
        GetComponent<CircleCollider2D>().enabled = false;
        //動きをとめる
        rbody.linearVelocity = Vector2.zero;
        rbody.gravityScale = 1.0f;
        anime.SetTrigger("dead"); //死亡アニメクリップの発動
        //y軸に5跳ね上げる
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        Destroy(gameObject, 1.0f); //Playerの存在を1秒後に消去
    }
}
