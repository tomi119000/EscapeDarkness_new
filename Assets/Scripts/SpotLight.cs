using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public PlayerController playerCnt;
    public float rotationSpeed = 20.0f; //回転速度

    // Update is called once per frame
    void LateUpdate() 
    //遅れてのUpdateメソッド. ここではPlayerの角度が決まってからUpdateした方がよい
    {
        //寸前までのスポットライトの回転値（ｚ軸のみ取得）
        //float currentAngle = transform.eulerAngles.z;

        //プレイヤーの角度
        float targetAngle = playerCnt.angleZ; 

        //ターゲットとなる角度を調整
        Quaternion targetRotation = Quaternion.Euler(0,0,(targetAngle-90));

        //現在の回転を（寸前の回転）→（ターゲットの回転）になるように
        //なめらかに補完するQuaternion.Slerpメソッド
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
