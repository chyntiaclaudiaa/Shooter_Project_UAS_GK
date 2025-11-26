using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] daftarMusuh;

    public float waktuMuncul = 2f; 
    private float timer;

    public float posisiX = 15f; 
    public float tinggiMin = -4f;
    public float tinggiMax = 4f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waktuMuncul)
        {
            SpawnMusuh();
            timer = 0;
        }
    }

    void SpawnMusuh()
    {
        if (daftarMusuh.Length == 0) return;

        // 1. Pilih musuh acak (Bola? Kubus?)
        int index = Random.Range(0, daftarMusuh.Length);

        // 2. Pilih tinggi acak (Atas? Bawah?)
        float yAcak = Random.Range(tinggiMin, tinggiMax);
        Vector3 posisiSpawn = new Vector3(posisiX, yAcak, 0);

        Instantiate(daftarMusuh[index], posisiSpawn, Quaternion.identity);
    }
}