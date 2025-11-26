using UnityEngine;

public class GerakEnemy : MonoBehaviour
{
    // --- SETTINGAN GERAK ---
    public float kecepatanGerak = 5f;
    public float batasKiri = -15f;

    // --- SETTINGAN ROTASI ---
    public float kecepatanRotasiX = 0f;
    public float kecepatanRotasiY = 0f;
    public float kecepatanRotasiZ = 100f; 

    // Variabel penampung sudut 
    private float sudutX = 0;
    private float sudutY = 0;
    private float sudutZ = 0;

    void Start()
    {
        // 1. Ambil rotasi awal 
        Vector3 rotasiAwal = transform.eulerAngles;
        sudutX = rotasiAwal.x;
        sudutY = rotasiAwal.y;
        sudutZ = rotasiAwal.z;

        // 2. Acak sedikit rotasinya
        sudutZ = Random.Range(0, 360);
    }

    void Update()
    {
        // -------------------------------------------------------
        // 1. LOGIKA JALAN KE KIRI 
        // -------------------------------------------------------
        float xBaru = transform.position.x - (kecepatanGerak * Time.deltaTime);
        transform.position = new Vector3(xBaru, transform.position.y, transform.position.z);


        // -------------------------------------------------------
        // 2. LOGIKA ROTASI/ROLLING 
        // -------------------------------------------------------
        sudutX += kecepatanRotasiX * Time.deltaTime;
        sudutY += kecepatanRotasiY * Time.deltaTime;
        sudutZ += kecepatanRotasiZ * Time.deltaTime; 

        // Reset sudut kalau kelebihan 
        if (sudutZ >= 360f) sudutZ -= 360f;

        // Terapkan ke Transform
        transform.eulerAngles = new Vector3(sudutX, sudutY, sudutZ);


        // -------------------------------------------------------
        // 3. HAPUS KALAU KELUAR LAYAR
        // -------------------------------------------------------
        if (transform.position.x < batasKiri)
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // 4. LOGIKA KENA PELURU
    // -------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peluru"))
        {
            // Hancurkan Peluru
            Destroy(other.gameObject);

            // Hancurkan Musuh
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            // Kalau nabrak Pesawat Game Over 
            Debug.Log("DITABRAK MUSUH!");
            Destroy(gameObject); // Musuh hancur
        }
    }
}