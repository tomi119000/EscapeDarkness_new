using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player; //ターゲットとなるプレイヤーのオブジェクト

    public float followSpeed = 5.0f; //プライヤーにおいつくSpeed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Find Player game object based on tag "Player".  
        player = GameObject.FindGameObjectWithTag("Player");

        //Camera Position: set the same as Player position. 
        //set z as -10 to have sufficient distanse from the main camera. 
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y,-10);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null) return;

        Vector3 nextPos = new Vector3(player.transform.position.x,
            player.transform.position.y, -10);

        Vector3 nowPos = transform.position;

        transform.position = Vector3.Lerp(nowPos, nextPos, followSpeed * Time.deltaTime);


    }
}
