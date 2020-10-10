using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartScript : MonoBehaviour
{
    private AudioSource audioSrc;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        SoundManager.PlayWith(audioSrc, SoundManager.Clip.CLICK, () =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
