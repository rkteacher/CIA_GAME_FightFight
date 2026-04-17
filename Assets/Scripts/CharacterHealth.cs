using UnityEngine;

public class CharacterHealth : MonoBehaviour
{

    public float health;


    private void Start()
    {
        // Set the health to an ammount read from a initialization class.
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            health--;
            CheckHealthStatus();
        }
    }


    void CheckHealthStatus()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
