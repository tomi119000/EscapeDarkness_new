using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float deleteTime = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.instance.SEPlay(SEType.Barrier); //バリア発生の音

        Destroy(gameObject, deleteTime);
    }

}
