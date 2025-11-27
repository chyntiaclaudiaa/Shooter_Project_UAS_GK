using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public void RestartGame()
    {
        // Mengembalikan waktu jadi normal (karena tadi di-pause saat game over)
        Time.timeScale = 1;

        // Load ulang scene yang sedang aktif sekarang
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}