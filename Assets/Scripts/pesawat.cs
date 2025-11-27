using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("UI Game Over")]
    public GameObject panelGameOver;

    [Header("Audio")]
    public AudioClip suaraTembak;   
    public AudioClip suaraGameOver;  
    private AudioSource audioSource; 

    private float rotasiModelAsliX;
    private float rotasiModelAsliY;
    private float rotasiModelAsliZ;

    void Start()
    {
        // Cari komponen AudioSource di benda ini
        audioSource = GetComponent<AudioSource>();

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
}