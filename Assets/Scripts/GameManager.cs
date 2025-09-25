using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static bool[] doorsOpenedState = { false, false, false };
    public static int key1; //key1の持ち数
    public static int key2;
    public static int key3;
    public static bool[] KeysPickedState = { false, false, false };
    public static int bill = 10;
    public static bool[] itemsPickedState = { false, false, false, false, false };

    static public bool hasSpotLight; //スポットライトを所持しているかどうか

    public static int playerHP =3; //PlayerのHP
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        //まずはゲーム開始状態にする
        gameState = GameState.playing;
    }

    private void Update()
    {
        // gameState がgameoverになったらTitleシーンへ
        if(gameState == GameState.gameover)
        {
            StartCoroutine(TitleBack());
        }
    }
    IEnumerator TitleBack()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Title");
    }
}
