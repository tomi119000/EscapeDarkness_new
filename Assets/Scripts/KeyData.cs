using UnityEngine;

public enum KeyType
{
    key1,
    key2,
    key3
}
public class KeyData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public KeyType keyType = KeyType.key1;  //識別タイプ
    Rigidbody2D rbody; 

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            switch(keyType)
            {
                case KeyType.key1:
                    GameManager.key1++;
                    GameManager.KeysPickedState[0] = true;
                    break; 

                case KeyType.key2:
                    GameManager.key2++;
                    GameManager.KeysPickedState[1] = true;
                    break;

                case KeyType.key3:
                    GameManager.key3++;
                    GameManager.KeysPickedState[2] = true;
                    break; 
            }

            GetComponent<CircleCollider2D>().enabled = false; 
            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);

        }
    }
}
