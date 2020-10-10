using UnityEngine;

public class BoundScript : MonoBehaviour
{
    private PlayerScript player;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerScript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            player.SIGSEGV();
    }
}
