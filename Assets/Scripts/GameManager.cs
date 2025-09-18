using UnityEngine;

//ゲーム状態を管理する列挙型
public enum GameState
{
    playing,
    talk,
    gameover,
    gameclear,
    ending
}
public class GameManager : MonoBehaviour
{
    public static GameState gameState;

    public static bool[] doorsOpenedState;
    public static int key1; //key1の持ち数
    public static int key2;
    public static int key3;
    public static bool[] KeysPickedState;
    public static int bill = 10;
    public static bool[] itemsPickedState;

    static public bool hasSpotLight; //スポットライトを所持しているかどうか

    public static int playerHP =100; //PlayerのHP
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        //まずはゲーム開始状態にする
        gameState = GameState.playing;
    }
}
