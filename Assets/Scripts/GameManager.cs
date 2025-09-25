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
    public static int bill = 0; //お札の数
    public static bool[] itemsPickedState = { false, false, false, false, false };

    static public bool hasSpotLight; //スポットライトを所持しているかどうか

    public static int playerHP =3; //PlayerのHP
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        //まずはゲーム開始状態にする
        gameState = GameState.playing;

        //シーン名の取得
        Scene currentScene = SceneManager.GetActiveScene();
        // シーンの名前を取得
        string sceneName = currentScene.name;

        switch (sceneName)
        {
            case "Title":
                SoundManager.instance.PlayBgm(BGMType.Title);
                break;
            case "Boss":
                SoundManager.instance.PlayBgm(BGMType.InBoss);
                break;
            case "Opening":
            case "Ending": //case構文を2つ上下に並べるとOpeningまたはEndingのとき
                SoundManager.instance.StopBgm();
                break;
            default:
                SoundManager.instance.PlayBgm(BGMType.InGame);
                break;
        }
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
