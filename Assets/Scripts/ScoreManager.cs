using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI textSkor; 
    public int skorSaatIni = 0;     

    void Start()
    {
        // Reset skor jadi 0 setiap game mulai
        skorSaatIni = 0;
        UpdateTampilanSkor();
    }

    // Fungsi ini dipanggil musuh kalau dia mati
    public void TambahSkor(int nilai)
    {
        skorSaatIni += nilai;
        UpdateTampilanSkor();
    }

    void UpdateTampilanSkor()
    {
        // Ubah angka jadi tulisan
        if (textSkor != null)
        {
            textSkor.text = skorSaatIni.ToString();
        }
    }
}