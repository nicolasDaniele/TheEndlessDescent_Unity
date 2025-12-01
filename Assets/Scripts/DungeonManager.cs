using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance => instance;

    [SerializeField]
    private PlayerCharacterDatabase characterDatabase;
    [SerializeField]
    private Transform playerCharacterSpawningTransform;
    private GameObject playerCharacterToSpawn;
    private static DungeonManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(characterDatabase == null)
        {
            Debug.LogWarning("characterDataBase is Not Set");
            return;
        }

        playerCharacterToSpawn = characterDatabase.GetSelectedPlayerCharacterPrefab();

        if (playerCharacterToSpawn == null)
        {
            Debug.LogWarning("playerCharacterToSpawn is null!");
            return;
        }

        Instantiate(playerCharacterToSpawn, 
                    playerCharacterSpawningTransform.position, 
                    playerCharacterSpawningTransform.rotation);
    }
}
