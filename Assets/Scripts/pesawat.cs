using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GerakPesawat : MonoBehaviour
{
    [Header("Settingan Gerak")]
    public float kecepatan = 5f;
    public float batasAtas = 6.85f;
    public float batasBawah = -5.25f;
    public float kekuatanRotasi = 3f;
    public Transform modelPesawat;

    [Header("Peluru")]
    public GameObject prefabPeluru;
    public Transform titikTembak;

    [Header("Settingan Nyawa")]
    public int nyawa = 3;
    public float faktorMengecil = 0.9f;

    [Header("UI")]
    public GameObject panelGameOver;
    public GameObject panelWin; // <-- BARU: Panel untuk menang

    [Header("Audio")]
    public AudioClip suaraTembak;
    public AudioClip suaraGameOver;
    private AudioSource audioSource;

    private float rotasiModelAsliX;
    private float rotasiModelAsliY;
    private float rotasiModelAsliZ;

    [Header("Shader Warna")]
    public Renderer rendererPesawat;
    public Color warnaNormal = new Color(0.29f, 0.29f, 0.29f, 1f);
    public Color warnaTembak = new Color(0.75f, 0.75f, 0.75f, 1f);
    public float durasiWarnaTembak = 0.4f;

    private Material matPesawat;


    void Start()
    {
        // Cari komponen AudioSource di benda ini
        audioSource = GetComponent<AudioSource>();
        rendererPesawat = GetComponentInChildren<Renderer>();
        if (rendererPesawat != null)
        {
            matPesawat = rendererPesawat.material; // INSTANCE material
            matPesawat.SetColor("_Color", warnaNormal);
        }

        if (modelPesawat != null)
        {
            rotasiModelAsliX = modelPesawat.localEulerAngles.x;
            rotasiModelAsliY = modelPesawat.localEulerAngles.y;
            rotasiModelAsliZ = modelPesawat.localEulerAngles.z;
        }

        // Pastikan panel nonaktif di awal
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelWin != null) panelWin.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        // --- INPUT GERAK ---
        float inputY = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputY = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputY = -1f;
        }
        float yBaru = transform.position.y + (inputY * kecepatan * Time.deltaTime);
        if (yBaru > batasAtas) yBaru = batasAtas;
        else if (yBaru < batasBawah) yBaru = batasBawah;
        transform.position = new Vector3(transform.position.x, yBaru, transform.position.z);
        transform.rotation = Quaternion.identity;

        // Rotasi Model
        if (modelPesawat != null)
        {
            float rotasiZBaru = rotasiModelAsliZ + (inputY * kekuatanRotasi);
            modelPesawat.localEulerAngles = new Vector3(rotasiModelAsliX, rotasiModelAsliY, rotasiZBaru);
        }

        // --- NEMBAK ---
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

            // BUNYIKAN SUARA TEMBAK
            if (audioSource != null && suaraTembak != null)
            {
                audioSource.PlayOneShot(suaraTembak);
            }
            if (matPesawat != null)
            {
                StopAllCoroutines();
                StartCoroutine(FlashWarna());
            }
        }
    }

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
        Debug.Log("Sisa Nyawa: " + nyawa);
        transform.localScale = transform.localScale * faktorMengecil;

        if (nyawa <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // BUNYIKAN SUARA GAME OVER
        if (audioSource != null && suaraGameOver != null)
        {
            audioSource.PlayOneShot(suaraGameOver);
        }

        Time.timeScale = 0;
        if (panelGameOver != null) panelGameOver.SetActive(true);
    }

    // ========== FUNGSI BARU: UNTUK MENANG ==========
    public void WinGame()
    {
        Debug.Log("WIN GAME! Score mencapai 100!");

        // HENTIKAN GAME (sama seperti GameOver)
        Time.timeScale = 0;

        // TAMPILKAN PANEL WIN (bukan panel GameOver)
        if (panelWin != null)
        {
            panelWin.SetActive(true);
            Debug.Log("Panel Win diaktifkan!");
        }
        else
        {
            Debug.LogError("Panel Win belum diassign di Inspector!");

            // Fallback ke panel GameOver
            if (panelGameOver != null)
            {
                panelGameOver.SetActive(true);
            }
        }

        // CATATAN: Tidak ada pemanggilan KenaHit() di sini
        // Jadi pesawat TIDAK AKAN mengecila
    }

    IEnumerator FlashWarna()
    {
        matPesawat.SetColor("_Color", warnaTembak);
        yield return new WaitForSeconds(durasiWarnaTembak);
        matPesawat.SetColor("_Color", warnaNormal);
    }
}