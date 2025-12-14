using UnityEngine;
using System.Collections;

public class GerakEnemy : MonoBehaviour
{
    public float kecepatanGerak = 5f;
    public float batasKiri = -15f;

    // ROTASI
    public float kecepatanRotasiX = 0f;
    public float kecepatanRotasiY = 0f;
    public float kecepatanRotasiZ = 100f;
    private float sudutX, sudutY, sudutZ;

    [Header("Efek")]
    public GameObject prefabLedakan;
    public AudioClip suaraPecah;

    [Header("Warna Kena Tembak")]
    public Renderer rendererEnemy;
    public Color warnaKena = new Color(0.9f, 0.9f, 0.9f, 1f);
    public float durasiWarna = 0.15f;

    private Material matEnemy;
    private bool sudahKena = false;

    void Start()
    {
        // ROTASI AWAL
        Vector3 rotasiAwal = transform.eulerAngles;
        sudutX = rotasiAwal.x;
        sudutY = rotasiAwal.y;
        sudutZ = Random.Range(0, 360);

        // AMBIL MATERIAL (TIDAK MENGUBAH WARNA AWAL)
        if (rendererEnemy == null)
            rendererEnemy = GetComponentInChildren<Renderer>();

        if (rendererEnemy != null)
            matEnemy = rendererEnemy.material; // INSTANCE material
    }

    void Update()
    {
        // GERAK
        transform.position += Vector3.left * kecepatanGerak * Time.deltaTime;

        // ROTASI
        sudutX += kecepatanRotasiX * Time.deltaTime;
        sudutY += kecepatanRotasiY * Time.deltaTime;
        sudutZ += kecepatanRotasiZ * Time.deltaTime;
        if (sudutZ >= 360f) sudutZ -= 360f;

        transform.eulerAngles = new Vector3(sudutX, sudutY, sudutZ);

        // KELUAR LAYAR
        if (transform.position.x < batasKiri)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (sudahKena) return;

        if (other.CompareTag("Peluru"))
        {
            sudahKena = true;

            Destroy(other.gameObject);

            // TAMBAH SKOR
            ScoreManager score = FindFirstObjectByType<ScoreManager>();
            if (score != null)
                score.TambahSkor(10);

            StartCoroutine(AbuHancur());
        }
        else if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator AbuHancur()
    {
        // UBAH WARNA KE ABU (TANPA BALIK)
        if (matEnemy != null)
            matEnemy.color = warnaKena;

        yield return new WaitForSeconds(durasiWarna);

        if (prefabLedakan != null)
            Instantiate(prefabLedakan, transform.position, Quaternion.identity);

        if (suaraPecah != null)
            AudioSource.PlayClipAtPoint(suaraPecah, transform.position);

        Destroy(gameObject);
    }
}
