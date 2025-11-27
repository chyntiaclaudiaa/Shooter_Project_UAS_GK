using UnityEngine;

public class GerakEnemy : MonoBehaviour
{
    public float kecepatanGerak = 5f;
    public float batasKiri = -15f;

    // ROTASI
    public float kecepatanRotasiX = 0f;
    public float kecepatanRotasiY = 0f;
    public float kecepatanRotasiZ = 100f;
    private float sudutX = 0; private float sudutY = 0; private float sudutZ = 0;

    [Header("Efek")]
    public GameObject prefabLedakan;
    public AudioClip suaraPecah;

    void Start()
    {
        Vector3 rotasiAwal = transform.eulerAngles;
        sudutX = rotasiAwal.x; sudutY = rotasiAwal.y; sudutZ = rotasiAwal.z;
        sudutZ = Random.Range(0, 360);
    }

    void Update()
    {
        // Gerak
        float xBaru = transform.position.x - (kecepatanGerak * Time.deltaTime);
        transform.position = new Vector3(xBaru, transform.position.y, transform.position.z);

        // Rotasi
        sudutX += kecepatanRotasiX * Time.deltaTime;
        sudutY += kecepatanRotasiY * Time.deltaTime;
        sudutZ += kecepatanRotasiZ * Time.deltaTime;
        if (sudutZ >= 360f) sudutZ -= 360f;
        transform.eulerAngles = new Vector3(sudutX, sudutY, sudutZ);

        if (transform.position.x < batasKiri) Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peluru"))
        {
            // --- BAGIAN SKOR (INI BARU) ---
            // Cari script ScoreManager di dalam scene, lalu tambah 10
            ScoreManager penambahSkor = FindFirstObjectByType<ScoreManager>();
            if (penambahSkor != null)
            {
                penambahSkor.TambahSkor(10);
            }
            // ------------------------------

            // EFEK VISUAL
            if (prefabLedakan != null) Instantiate(prefabLedakan, transform.position, Quaternion.identity);

            // EFEK SUARA
            if (suaraPecah != null) AudioSource.PlayClipAtPoint(suaraPecah, transform.position);

            // HANCURKAN
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}