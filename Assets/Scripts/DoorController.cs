using System.Collections;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public RoomData roomData;
    //親Objectのroomdata
    MessageData message; //ScritableObjectであるクラス

    bool isPlayerInRange;
    bool isTalk;
    GameObject canvas; //トークUIを含んだCanvasオブジェクト
    GameObject talkPanel; //対象となるトークUIパネル
    TextMeshProUGUI nameText; //トークＵＩパネルの名前
    TextMeshProUGUI messageText; //トークＵＩパネルのメッセージ 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        message = roomData.message;
        //トークデータは親オブジェクトのスクリプトの変数を参照
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        //canvasの子であるTalkPanelを探す。これはgameObjcet
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
        //talkPanelの子であるNemeText（これはTextMeshProUGUIコンポーネント）
        nameText = talkPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerInRange && !isTalk && Input.GetKeyDown(KeyCode.E))
        {
            StartConversation();
        }
    }

    void StartConversation()
    {
        isTalk = true; //talk中フラグON
        GameManager.gameState = GameState.talk; //gameState is talk
        talkPanel.SetActive(true);
        nameText.text = message.msgArray[0].name;
        messageText.text = message.msgArray[0].message;

        Time.timeScale = 0; //ゲーム進行をストップ
        StartCoroutine(TalkProcess()); //Talk Processコルーチン発動
    }

    // TalkProcessコルーチンの設計
    IEnumerator TalkProcess()
    {
        SoundManager.instance.SEPlay(SEType.Door); //ドアの音

        //フラッシュ入力阻止のため一瞬止める
        yield return new WaitForSecondsRealtime(0.1f); 

        while(!Input.GetKeyDown(KeyCode.E))
        {
            yield return null; //Eキーが押されるまで何もしない
        }

        bool nextTalk = false; //トークをさらに展開するかどうか

        switch(roomData.roomName)
        {
            case "fromRoom1":
                if(GameManager.key1 > 0)
                {
                    GameManager.key1--; //鍵の数を減らす
                    nextTalk = true;
                    GameManager.doorsOpenedState[0] = true; 
                }
                break;

            case "fromRoom2":
                if (GameManager.key2 > 0)
                {
                    GameManager.key2--; //鍵の数を減らす
                    nextTalk = true;
                    GameManager.doorsOpenedState[1] = true;
                }
                break;

            case "fromRoom3":
                if (GameManager.key3 > 0)
                {
                    GameManager.key3--; //鍵の数を減らす
                    nextTalk = true;
                    GameManager.doorsOpenedState[2] = true;
                }
                break;
        }

        if(nextTalk)
        {
            SoundManager.instance.SEPlay(SEType.DoorOpen); //ドア開ける音

            // 開錠したというメッセージを表示
            nameText.text = message.msgArray[1].name;
            messageText.text = message.msgArray[1].message;

            yield return new WaitForSecondsRealtime(0.1f); 

            while(!Input.GetKeyDown(KeyCode.E))
            {
                yield return null;
            }

            roomData.openedDoor = true;
            roomData.DoorOpenCheck(); 
        }

        EndConversation(); 
    }

    void EndConversation()
    {
        talkPanel.SetActive(false); 
        GameManager.gameState = GameState.playing;
        isTalk = false;
        Time.timeScale = 1.0f; //ゲーム進行を戻す
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
