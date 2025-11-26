using UnityEngine;
using UnityEngine.InputSystem;

public class GerakPesawat : MonoBehaviour
{
    // --- SETTINGAN ---
    public float kecepatan = 5f;
    public float batasAtas = 6.85f;
    public float batasBawah = -5.25f;
    public float kekuatanRotasi = 5f;

    public Transform modelPesawat; 

    // --- PELURU ---
    public GameObject prefabPeluru;
    public Transform titikTembak;

    private float rotasiModelAsliX;
    private float rotasiModelAsliY;
    private float rotasiModelAsliZ;

    void Start()
    {
        // Kita simpan rotasi asli si MODEL, bukan si Player
        if (modelPesawat != null)
        {
            rotasiModelAsliX = modelPesawat.localEulerAngles.x;
            rotasiModelAsliY = modelPesawat.localEulerAngles.y;
            rotasiModelAsliZ = modelPesawat.localEulerAngles.z;
        }
    }

    void Update()
    {
        // 1. INPUT
        float inputY = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputY = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputY = -1f;
        }

        // 2. GERAK POSISI 
        float yBaru = transform.position.y + (inputY * kecepatan * Time.deltaTime);
        if (yBaru > batasAtas) yBaru = batasAtas;
        else if (yBaru < batasBawah) yBaru = batasBawah;

        transform.position = new Vector3(transform.position.x, yBaru, transform.position.z);

        transform.rotation = Quaternion.identity;


        // 3. GERAK ROTASI 
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
}