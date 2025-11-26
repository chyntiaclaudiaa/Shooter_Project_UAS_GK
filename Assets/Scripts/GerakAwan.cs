using UnityEngine;

public class GerakAwan : MonoBehaviour
{
    public float kecepatanMin = 2f;
    public float kecepatanMax = 4f;

    public float batasKiri = -45f;
    public float posisiMuncul = 40f;

    public float tinggiMin = -4f;
    public float tinggiMax = -4f;

    private float kecepatanAktual;
    private float targetY;

    void Start()
    {
        ResetAwan();
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }

    void LateUpdate()
    {
        float xBaru = transform.position.x - (kecepatanAktual * Time.deltaTime);

        if (xBaru < batasKiri)
        {
            xBaru = posisiMuncul;
            ResetAwan();
        }

        transform.position = new Vector3(xBaru, targetY, transform.position.z);
    }

    void ResetAwan()
    {
        kecepatanAktual = Random.Range(kecepatanMin, kecepatanMax);
        targetY = Random.Range(tinggiMin, tinggiMax);
    }
}