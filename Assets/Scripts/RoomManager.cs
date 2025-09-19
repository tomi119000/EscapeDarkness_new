using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static int[] doorsPositionNumber = {0,0,0}; //# of door position
    public static int key1PositionNumber;  //key1 position #
    public static int[] itemsPositionNumber = { 0, 0, 0, 0, 0 }; //item position #

    public GameObject[] items = new GameObject[5]; //Array of Prefabs of items 
    public GameObject room; //Prefab of room
    public GameObject dummyDoor; //Prefab of Dummy door
    public GameObject key;  //Prefab of key

    public static bool positioned; //Initial positioning true/false
    public static string toRoomNumber = "fromRoom1"; //Position which Player should be assigned

    GameObject player; //Player information 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() //Startよりも早いタイミングで処理する
    {
        player = GameObject.FindGameObjectWithTag("Player"); 

        //キーの配置を確認する
        if(!positioned) //！でfalseの場合
        {
            StartKeysPosition(); //キーを初回配置するメソッド
            positioned = true; //初回配置済にする
        }
    }

    void StartKeysPosition()
    {
        GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");
        //受け皿が配列の場合は、FindGameObjectsWithTag (Objects :"s"が付く）
        int rand = Random.Range(1, keySpots.Length+1); //1~3までの数字をランダム生成

        //全スポットをチェックしに行く
        foreach(GameObject spots in keySpots)
        {
            // Check if the number extracted is the same as randomly generated number(rand)
            if(spots.GetComponent<KeySpot>().spotNum == rand)
            {
                //キー１を生成する
                Instantiate(key, spots.transform.position, Quaternion.identity);
                //どのスポット番号にキーを配置したか記録しておく
                key1PositionNumber = rand; 

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
