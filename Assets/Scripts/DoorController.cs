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
