using UnityEngine;

public class DeteksiMusuhLewat : MonoBehaviour
{
    public GerakPesawat scriptPesawat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 1. Lapor ke pesawat: "Kurangi nyawa & mengecil!"
            if (scriptPesawat != null)
            {
                scriptPesawat.KenaHit();
            }

            // 2. Hancurkan musuh 
            Destroy(other.gameObject);
        }
    }
}