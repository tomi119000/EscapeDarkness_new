using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DoorDirection //ここではup ,downに絞る
{
    up,
    down
}

public class RoomData : MonoBehaviour
{
    public string roomName; //出入口識別名
    public string nextRoomName; //シーン切り替え先での行先
    public string nextScene; //切り替え先シーン
    public bool openedDoor; //ドア開閉状況フラグ
    public DoorDirection direction; //プレイヤーの配置位置
    public MessageData message; //トークデータ
    public GameObject door; //表示非表示対象のドア情報

    public bool isSavePoint; //セーブポイントに使われるスクリプトにするかどうか

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isSavePoint)
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        //このRoomに触れたらどこに行くのかを変数nextRoomNameで決めておく
        RoomManager.toRoomNumber = nextRoomName;
        SceneManager.LoadScene(nextScene);
    }

    public void DoorOpenCheck()
    {
        //if door had been opened already,set the door unseen 
        if (openedDoor) door.SetActive(false);
    }
}
