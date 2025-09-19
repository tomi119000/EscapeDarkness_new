using UnityEngine;

public class BillController : MonoBehaviour
{
    public float deleteTime = 2.0f;
    public GameObject barrierPrefab; //自己消滅と引き換えに生成するプレハブ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //deleteTime秒後に「バリア展開して消滅」
        Invoke("FieldExpansion", deleteTime);

    }

    void FieldExpansion()
    {
        //バリア展開と自己消滅のメソッド
        //お札と同じ場所にバリア生成
        Instantiate(barrierPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //敵とぶつかったらバリア発動
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            FieldExpansion();
        }
    }
}
