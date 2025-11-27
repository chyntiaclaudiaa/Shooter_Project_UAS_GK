using UnityEngine;
using UnityEngine.InputSystem;

public class GerakPesawat : MonoBehaviour
{
    // --- SETTINGAN GERAK ---
    public float kecepatan = 5f;
    public float batasAtas = 6.85f;
    public float batasBawah = -5.25f;
    public float kekuatanRotasi = 3f;

    public Transform modelPesawat;

    // --- PELURU ---
    public GameObject prefabPeluru;
    public Transform titikTembak;

    // --- NYAWA & EFEK ---
    [Header("Settingan Nyawa")]
    public int nyawa = 3;
    public float faktorMengecil = 0.9f;

    // --- UI GAME OVER (INI YANG BARU) ---
    [Header("UI Game Over")]
    public GameObject panelGameOver; // <--- Variabel baru buat panel

    // Variabel Simpanan Rotasi
    private float rotasiModelAsliX;
    private float rotasiModelAsliY;
    private float rotasiModelAsliZ;

    void Start()
    {
        if (modelPesawat != null)
        {
            rotasiModelAsliX = modelPesawat.localEulerAngles.x;
            rotasiModelAsliY = modelPesawat.localEulerAngles.y;
            rotasiModelAsliZ = modelPesawat.localEulerAngles.z;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // 1. INPUT GERAK
        float inputY = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputY = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputY = -1f;
        }

        // 2. LOGIKA POSISI
        float yBaru = transform.position.y + (inputY * kecepatan * Time.deltaTime);

        if (yBaru > batasAtas) yBaru = batasAtas;
        else if (yBaru < batasBawah) yBaru = batasBawah;

        transform.position = new Vector3(transform.position.x, yBaru, transform.position.z);
        transform.rotation = Quaternion.identity;

        // 3. LOGIKA ROTASI MODEL
        if (modelPesawat != null)
        {
            float rotasiZBaru = rotasiModelAsliZ + (inputY * kekuatanRotasi);
            modelPesawat.localEulerAngles = new Vector3(rotasiModelAsliX, rotasiModelAsliY, rotasiZBaru);
        }

        // 4. LOGIKA NEMBAK
        bool tekanTembak = false;
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame) tekanTembak = true;
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) tekanTembak = true;

        if (tekanTembak) Nembak();
    }

    void Nembak()
    {
        if (prefabPeluru != null && titikTembak != null)
        {
            Instantiate(prefabPeluru, titikTembak.position, prefabPeluru.transform.rotation);
        }
    }

    // --- FUNGSI TABRAKAN ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            KenaHit();
            Destroy(other.gameObject);
        }
    }

    public void KenaHit()
    {
        nyawa--;
        Debug.Log("Pesawat Kena! Sisa Nyawa: " + nyawa);

        // Efek Mengecil
        transform.localScale = transform.localScale * faktorMengecil;

        if (nyawa <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER!");

        Time.timeScale = 0;

        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }
    }
}