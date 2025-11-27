using UnityEngine;

public class HancurkanOtomatis : MonoBehaviour
{
    // Waktu tunggu sebelum hancur (bisa diatur di Unity nanti)
    public float waktuTunggu = 1.0f;

    void Start()
    {
        // Hancurkan objek ini setelah sekian detik
        Destroy(gameObject, waktuTunggu);
    }
}