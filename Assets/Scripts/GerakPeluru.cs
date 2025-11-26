using UnityEngine;

public class GerakPeluru : MonoBehaviour
{
    public float kecepatan = 15f;
    public float batasKanan = 20f;

    void Update()
    {
        // Gerak ke kanan (Sumbu X)
        float xBaru = transform.position.x + (kecepatan * Time.deltaTime);
        transform.position = new Vector3(xBaru, transform.position.y, transform.position.z);

        // Hancur kalau lewat batas
        if (transform.position.x > batasKanan) Destroy(gameObject);
    }
}