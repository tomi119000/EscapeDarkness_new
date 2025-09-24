using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static int[] doorsPositionNumber = { 0, 0, 0 }; //# of door position
    public static int key1PositionNumber;  //key1 position #
    public static int[] itemsPositionNumber = { 0, 0, 0, 0, 0 }; //item position #

    public GameObject[] items = new GameObject[5]; //Array of Prefabs of items 
    public GameObject room; //Prefab of door

    public MessageData[] messages; //配置したドアに割り振るScritableObject

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
        if (!positioned) //！でfalseの場合
        {
            StartKeysPosition(); //キーを初回配置するメソッド
            StartDoorsPosition();
            StartItemsPosition();
            positioned = true; //初回配置済にする
        }
        else //初期配置済だった場合
        {
            LoadKeysPosition(); //key配置の再現
            LoadItemsPosition(); //アイテム配置の再現
            LoadDoorsPosition(); //ドア配置の再現
        }
    }

    void StartKeysPosition()
    {
        GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");
        //受け皿が配列の場合は、FindGameObjectsWithTag (Objects :"s"が付く）
        int rand = Random.Range(1, (keySpots.Length + 1)); //1~3までの数字をランダム生成
        Debug.Log("keySpots数" + keySpots.Length);

        //全てのkey1のスポットをチェックしに行く
        foreach (GameObject spots in keySpots)
        {
            // Check if the number extracted is the same as randomly generated number(rand)
            if (spots.GetComponent<KeySpot>().spotNum == rand)
            {
                //キー１を生成する
                Instantiate(key, spots.transform.position, Quaternion.identity);
                //どのスポット番号にキーを配置したか記録しておく
                key1PositionNumber = rand;

            }
        }

        //Key2, Key3の対象スポット
        GameObject KeySpot;
        GameObject obj; //生成したKey2, Key3が入る予定

        //Key2スポットの取得
        KeySpot = GameObject.FindGameObjectWithTag("KeySpot2");
        //Keyの生成とobjへの格納
        obj = Instantiate(key, KeySpot.transform.position, Quaternion.identity);
        obj.GetComponent<KeyData>().keyType = KeyType.key2;

        //Key3スポットの取得
        KeySpot = GameObject.FindGameObjectWithTag("KeySpot3");
        //Keyの生成とobjへの格納
        obj = Instantiate(key, KeySpot.transform.position, Quaternion.identity);
        obj.GetComponent<KeyData>().keyType = KeyType.key3;
    }

    void StartItemsPosition()
    {
        GameObject[] itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot");

        for (int i = 0; i < items.Length; i++)
        {
            int rand; //ランダムな数の受け皿
            bool unique; //ユニークかどうかのフラグ

            do //絶対1回目を実施する
            {
                unique = true; //問題なければそのままループを抜ける予定
                rand = Random.Range(1, (itemSpots.Length + 1));

                //既にランダムに取得した番号がどこかのスポットに割り当てられていないか
                //doorsPositionNumber配列の状況を全点検
                foreach (int numbers in itemsPositionNumber)
                {
                    if (numbers == rand)
                    {
                        unique = false;
                        break;
                    }
                }
            } while (!unique); //uniqueな番号でない場合最初から繰り返し(doへ）

            foreach (GameObject spots in itemSpots)
            {
                //スポットの全チェック（ランダム値とスポット番号の一致）
                //一致していれば、そこにアイテムを生成
                if (spots.GetComponent<ItemSpot>().spotNum == rand)
                {
                    GameObject obj = Instantiate(items[i],
                        spots.transform.position,
                        Quaternion.identity);

                    //どのスポット番号がどのアイテムに割り当てられているのかを記録
                    itemsPositionNumber[i] = rand;

                    //生成したアイテムに識別番号を割り振っていく
                    if (obj.CompareTag("Bill"))
                    {
                        obj.GetComponent<BillData>().itemNum = i;
                    }
                    else
                    {
                        obj.GetComponent<DrinkData>().itemNum = i;
                    }
                }
            }
        }
    }

    void StartDoorsPosition()
    {
        //全スポットの取得
        GameObject[] roomSpots = GameObject.FindGameObjectsWithTag("RoomSpot");

        //出入口（鍵１～鍵３の3つの出入口）の分だけ繰り返し
        for (int i = 0; i < doorsPositionNumber.Length; i++)
        {
            int rand; //ランダムな数の受け皿
            bool unique; //ユニークかどうかのフラグ

            do //絶対1回目を実施する
            {
                unique = true; //問題なければそのままループを抜ける予定
                rand = Random.Range(1, (roomSpots.Length + 1));

                //既にランダムに取得した番号がどこかのスポットに割り当てられていないか
                //doorsPositionNumber配列の状況を全点検
                foreach (int numbers in doorsPositionNumber)
                {
                    if (numbers == rand)
                    {
                        unique = false;
                        break;
                    }
                }
            } while (!unique); //uniqueな番号でない場合最初から繰り返し(doへ）

            foreach (GameObject spots in roomSpots)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == rand)
                {
                    GameObject obj = Instantiate(
                        room,
                        spots.transform.position,
                        Quaternion.identity
                        );
                    doorsPositionNumber[i] = rand;

                    //生成したドアのセッティング
                    DoorSetting(
                        obj, "fromRoom" + (i + 1),
                        "Room" + (i + 1),
                        "Main",
                        false,
                        DoorDirection.down,
                        messages[i]
                        );
                }

            }
        }
        //ダミードア扉の生成
        foreach (GameObject spots in roomSpots)
        {
            //既に配置済かどうか
            bool match = false;

            foreach (int doorNum in doorsPositionNumber)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == doorNum)
                {
                    match = true;
                    break;
                }
            }

            if (!match) Instantiate(
                dummyDoor, spots.transform.position, Quaternion.identity);
        }

    }
    void DoorSetting(GameObject obj, string roomName, string nextRoomName, string sceneName, bool openedDoor, DoorDirection direction, MessageData messaage)
    {
        RoomData roomData = obj.GetComponent<RoomData>();
        //第一引数で指定したobjectのRoomDataスクリプトの各変数に
        //第二引数で指定した値を代入
        roomData.roomName = roomName;
        roomData.nextRoomName = nextRoomName;
        roomData.openedDoor = openedDoor;
        roomData.nextScene = sceneName;
        roomData.direction = direction;
        roomData.message = messaage;

        //ドアの開閉状況フラグをみてドアを表示/非表示するメソッド
        roomData.DoorOpenCheck();
    }

    void LoadKeysPosition()
    {
        //Keyが未取得だったら
        if (!GameManager.KeysPickedState[0])
        {
            GameObject[] keySpots = GameObject.FindGameObjectsWithTag("KeySpot");

            foreach (GameObject spots in keySpots)
            {
                //key1 positionの取得
                if (spots.GetComponent<KeySpot>().spotNum == key1PositionNumber)
                {
                    Instantiate(
                        key,
                        spots.transform.position,
                        Quaternion.identity);
                }
            }
        }

        //Key2が未取得だったら
        if (!GameManager.KeysPickedState[1])
        {
            //key1 positionの取得
            GameObject keySpot2 = GameObject.FindGameObjectWithTag("KeySpot2");
            GameObject obj = Instantiate(
                key,
                keySpot2.transform.position,
                Quaternion.identity);

            obj.GetComponent<KeyData>().keyType = KeyType.key2;
        }

        if (!GameManager.KeysPickedState[2])
        {
            //key1 positionの取得
            GameObject keySpot3 = GameObject.FindGameObjectWithTag("KeySpot3");
            GameObject obj = Instantiate(
                key,
                keySpot3.transform.position,
                Quaternion.identity);

            obj.GetComponent<KeyData>().keyType = KeyType.key3;
        }
    }
    void LoadItemsPosition()
    {
        GameObject[] itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot");

        for (int i = 0; i < items.Length; i++)
        {
            if (!GameManager.itemsPickedState[i])
            {
                foreach (GameObject spots in itemSpots)
                {
                    //スポットの全チェック（ランダム値とスポット番号の一致）
                    //一致していれば、そこにアイテムを生成
                    if (spots.GetComponent<ItemSpot>().spotNum == itemsPositionNumber[i])
                    {
                        GameObject obj = Instantiate(
                                items[i],
                                spots.transform.position,
                                Quaternion.identity);

                        //生成したアイテムに識別番号を割り振っていく
                        if (obj.CompareTag("Bill"))
                        {
                            obj.GetComponent<BillData>().itemNum = i;
                        }
                        else
                        {
                            obj.GetComponent<DrinkData>().itemNum = i;
                        }
                    }
                }
            }
        }
    }

    void LoadDoorsPosition()
    {
        //全スポットの取得
        GameObject[] roomSpots = GameObject.FindGameObjectsWithTag("RoomSpot");

        for (int i = 0; i < doorsPositionNumber.Length; i++)
        {

            foreach (GameObject spots in roomSpots)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == doorsPositionNumber[i])
                {
                    GameObject obj = Instantiate(
                            room,
                            spots.transform.position,
                            Quaternion.identity
                            );

                    //生成したドアのセッティング
                    DoorSetting(
                        obj, "fromRoom" + (i + 1),
                        "Room" + (i + 1),
                        "Main",
                        GameManager.doorsOpenedState[i], //i番目のドア開錠情報
                        DoorDirection.down,
                        messages[i]
                        );
                }

            }
        }
        //ダミードア扉の生成
        foreach (GameObject spots in roomSpots)
        {
            //既に配置済かどうか
            bool match = false;

            foreach (int doorNum in doorsPositionNumber)
            {
                if (spots.GetComponent<RoomSpot>().spotNum == doorNum)
                {
                    match = true;
                    break;
                }
            }

            if (!match) Instantiate(
                dummyDoor, spots.transform.position, Quaternion.identity);
        }
    }
}

