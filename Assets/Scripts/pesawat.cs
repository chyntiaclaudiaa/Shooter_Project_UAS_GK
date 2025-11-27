using UnityEngine;
using UnityEngine.InputSystem;

public class GerakPesawat : MonoBehaviour
{
    // --- SETTINGAN GERAK ---
    public float kecepatan = 5f;
    public float batasAtas = 6.85f;
    public float batasBawah = -5.25f;
    public float kekuatanRotasi = 5f;

    public Transform modelPesawat;

    // --- PELURU ---
    public GameObject prefabPeluru;
    public Transform titikTembak;

    // --- NYAWA & EFEK KENA HIT (BARU) ---
    [Header("Settingan Nyawa")]
    public int nyawa = 3;             // Jumlah nyawa awal
    public float faktorMengecil = 0.9f; // Ukuran jadi 70% setiap kena hit

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
        // Kalau game over (waktu berhenti), hentikan input
        if (Time.timeScale == 0) return;

        // 1. INPUT
        float inputY = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputY = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputY = -1f;
        }

        // 2. GERAK POSISI 
        float yBaru = transform.position.y + (inputY * kecepatan * Time.deltaTime);

        // Batasi posisi (Clamping)
        if (yBaru > batasAtas) yBaru = batasAtas;
        else if (yBaru < batasBawah) yBaru = batasBawah;

        transform.position = new Vector3(transform.position.x, yBaru, transform.position.z);

        // Reset rotasi global biar gak aneh
        transform.rotation = Quaternion.identity;

        // 3. GERAK ROTASI MODEL
        if (modelPesawat != null)
        {
            float rotasiZBaru = rotasiModelAsliZ + (inputY * kekuatanRotasi);
            modelPesawat.localEulerAngles = new Vector3(rotasiModelAsliX, rotasiModelAsliY, rotasiZBaru);
        }

        // 4. NEMBAK
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

    // --- FUNGSI TABRAKAN (BARU) ---
    // Pastikan Player punya Rigidbody (uncheck Use Gravity) dan Box Collider (Check IsTrigger)
    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang ditabrak adalah Musuh?
        // Pastikan Prefab Balok/Bola/Musuh punya Tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            KenaHit();
            Destroy(other.gameObject); // Hancurkan musuh yang nabrak
        }
    }

    void KenaHit()
    {
        nyawa--; // Kurangi nyawa
        Debug.Log("Pesawat Kena! Sisa Nyawa: " + nyawa);

        // Efek Mengecil
        transform.localScale = transform.localScale * faktorMengecil;

        // Cek Game Over
        if (nyawa <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER!");
        // Menghentikan waktu permainan
        Time.timeScale = 0;

        // Di sini nanti bisa panggil UI Game Over
    }
}