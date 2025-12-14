using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI textSkor;

    [Header("Game Settings")]
    [SerializeField] private int skorAwal = 0;
    [SerializeField] private int skorMenang = 100;

    [Header("Audio")]
    [SerializeField] private AudioClip victorySound; // Bisa diassign manual ATAU diambil dari Resources

    [Header("UI Panel")]
    [SerializeField] private GameObject panelWin; // Drag PanelWin ke sini di Inspector!

    private int skorSaatIni;
    private AudioSource audioSource;
    private bool gameSelesai = false;

    void Start()
    {
        skorSaatIni = skorAwal;
        UpdateTampilanSkor();

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Coba load victory sound dari Resources jika belum diassign
        if (victorySound == null)
        {
            LoadVictorySoundFromResources();
        }

        // Jika panelWin tidak diassign, cari otomatis
        if (panelWin == null)
        {
            CariPanelWin();
        }
        else
        {
            // Pastikan panelWin nonaktif di awal
            panelWin.SetActive(false);
        }
    }

    void LoadVictorySoundFromResources()
    {
        // Cari di folder Resources/Sounds/
        victorySound = Resources.Load<AudioClip>("Sounds/victory");

        if (victorySound != null)
        {
            Debug.Log("Victory sound berhasil diload dari Resources: " + victorySound.name);
        }
        else
        {
            Debug.LogWarning("Victory sound tidak ditemukan di Resources/Sounds/victory");
            Debug.Log("Pastikan:");
            Debug.Log("1. File victory.wav ada di folder Assets/Resources/Sounds/");
            Debug.Log("2. Nama file tepat: 'victory' (tanpa ekstensi .wav)");
        }
    }

    void CariPanelWin()
    {
        // Cara 1: Cari langsung dengan nama
        panelWin = GameObject.Find("PanelWin");

        if (panelWin == null)
        {
            // Cara 2: Cari di dalam Canvas
            Canvas[] canvases = FindObjectsOfType<Canvas>(true);
            foreach (Canvas canvas in canvases)
            {
                Transform panel = canvas.transform.Find("PanelWin");
                if (panel != null)
                {
                    panelWin = panel.gameObject;
                    break;
                }
            }
        }

        if (panelWin != null)
        {
            Debug.Log("PanelWin ditemukan: " + panelWin.name);
            panelWin.SetActive(false); // Nonaktifkan di awal
        }
        else
        {
            Debug.LogWarning("PanelWin tidak ditemukan! Pastikan ada GameObject bernama 'PanelWin'");
        }
    }

    public void TambahSkor(int nilai)
    {
        if (gameSelesai) return; // Jangan tambah skor jika game sudah selesai

        if (nilai <= 0)
        {
            Debug.LogWarning("Nilai skor harus positif!");
            return;
        }

        skorSaatIni += nilai;
        Debug.Log("Score: " + skorSaatIni + " / " + skorMenang);
        UpdateTampilanSkor();

        if (skorSaatIni >= skorMenang)
        {
            Debug.Log("SKOR 100 TERCAPAI!");
            Menang();
        }
    }

    void UpdateTampilanSkor()
    {
        if (textSkor != null)
        {
            textSkor.text = skorSaatIni.ToString();
        }
    }

    void Menang()
    {
        gameSelesai = true;

        // Mainkan suara kemenangan
        MainkanSuaraKemenangan();

        // Tampilkan panel kemenangan
        if (panelWin != null)
        {
            panelWin.SetActive(true);
            Debug.Log("PanelWin diaktifkan!");
        }
        else
        {
            Debug.LogError("PanelWin tidak ditemukan!");
            // Coba cari lagi
            CariPanelWin();
            if (panelWin != null)
            {
                panelWin.SetActive(true);
            }
        }

        // Hentikan waktu game
        Time.timeScale = 0f;
        Debug.Log("Game dihentikan - Player Menang!");
    }

    void MainkanSuaraKemenangan()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource tidak ditemukan!");
            return;
        }

        // Coba lagi load dari Resources jika masih null
        if (victorySound == null)
        {
            LoadVictorySoundFromResources();
        }

        if (victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
            Debug.Log("Memainkan victory sound: " + victorySound.name);
        }
        else
        {
            Debug.LogWarning("Tidak bisa memainkan victory sound, clip tidak ditemukan!");
        }
    }

    // Method untuk memainkan sound lain dari Resources
    public void MainkanSoundDariResources(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
            Debug.Log("Memainkan sound: " + path);
        }
        else
        {
            Debug.LogWarning("Sound tidak ditemukan atau AudioSource null: " + path);
        }
    }

    // Contoh: Mainkan sound "buttonClick" dari Resources/Sounds/
    public void MainkanSoundButton()
    {
        MainkanSoundDariResources("Sounds/buttonClick");
    }

    // Contoh: Mainkan sound "collectItem" dari Resources/Sounds/
    public void MainkanSoundCollect()
    {
        MainkanSoundDariResources("Sounds/collectItem");
    }

    // Method untuk restart game
    public void RestartGame()
    {
        skorSaatIni = skorAwal;
        gameSelesai = false;
        UpdateTampilanSkor();
        Time.timeScale = 1f;

        if (panelWin != null)
        {
            panelWin.SetActive(false);
        }
    }

    // Method untuk testing di Inspector
    [ContextMenu("Test Kemenangan")]
    public void TestKemenangan()
    {
        skorSaatIni = skorMenang;
        Menang();
    }

    [ContextMenu("Test Play Victory Sound")]
    public void TestPlayVictorySound()
    {
        MainkanSuaraKemenangan();
    }

    // Property untuk akses dari script lain
    public int SkorSaatIni => skorSaatIni;
    public bool GameSelesai => gameSelesai;
}