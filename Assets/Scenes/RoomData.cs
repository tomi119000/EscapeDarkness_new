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
    public string roomName;
    public string nextRoomName;
    public string nextScene;
    public bool openedDoor;
    public DoorDirection direction;
    public MessageData message;
    public GameObject door;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
