using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TalkController : MonoBehaviour
{
    public MessageData message; //ScritableObjectであるクラス
    bool isPlayerInRange;
    bool isTalk;
    GameObject canvas; //トークUIを含んだCanvasオブジェクト
    GameObject talkPanel; //対象となるトークUIパネル
    TextMeshProUGUI nameText; //トークＵＩパネルの名前
    TextMeshProUGUI messageText; //トークＵＩパネルのメッセージ 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        //canvasの子であるTalkPanelを探す。これはgameObjcet
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
        //talkPanelの子であるNemeText（これはTextMeshProUGUIコンポーネント）
        nameText = talkPanel.transform.Find("NameText").GetComponent< TextMeshProUGUI>();
        messageText = talkPanel.transform.Find("Messagetext").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerInRange && !isTalk && Input.GetKeyDown(KeyCode.E))
            //GetKeyDown : Downを付けることで1回ごとに反応
        {
            StartConversation(); //トーク開始
        }
    }

    void StartConversation()
    {
        isTalk = true;
        GameManager.gameState = GameState.talk;
        talkPanel.SetActive(true);
        Time.timeScale = 0; //ゲーム進行スピードをゼロにする

        //コルーチンの発動. （）内にコルーチン名
        StartCoroutine(TalkProcess()); 
    }

    IEnumerator TalkProcess()
    {
        //対象としたScriptableObject（変数message）が扱っている配列
        for(int i=0; i<message.msgArray.Length; i++)
        {
            nameText.text = message.msgArray[i].name;
            messageText.text = message.msgArray[i].message;

            //E keyが押されてない間(!)、何もしない
            while(!Input.GetKeyDown(KeyCode.E))
            {
                yield return null; //何もしない
            }
        }

        EndConversation(); 

    }

    void EndConversation()
    {
        talkPanel.SetActive(false);
        GameManager.gameState = GameState.playing;
        isTalk = false;
        Time.timeScale = 1.0f; //GameSpeedを1に戻す
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
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
