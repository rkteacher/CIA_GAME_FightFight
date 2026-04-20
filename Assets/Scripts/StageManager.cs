using UnityEngine;

public class StageManager : MonoBehaviour
{

    public Transform spawnPoint_Player_1;

    [SerializeField] private GameObject player_1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Instantiate(player_1).transform.parent = spawnPoint_Player_1;
    }

}
