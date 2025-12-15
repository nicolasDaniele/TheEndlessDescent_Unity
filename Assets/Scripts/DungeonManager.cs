using Unity.Cinemachine;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance => instance;

    [Header("Player Character")]
    [SerializeField]
    private Transform playerCharacterSpawningTransform;
    [SerializeField]
    private GameObject defaultPlayerCharacter;

    [Header("Camera")]
    [SerializeField]
    private CinemachineCamera followCamera;

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
        playerCharacterToSpawn = PlayerCharacterDatabaseAccessor.GetPlayerCharacterPrefab();

        if (playerCharacterToSpawn == null)
        {
            Debug.LogWarning("[DungeonManager]  PlayerCharacterToSpawn is null!");

            if(defaultPlayerCharacter != null)
                playerCharacterToSpawn = defaultPlayerCharacter;
            else
                Debug.LogWarning("[DungeonManager] DefaultPlayerCharacter is null!");

            return;
        }

        GameObject spawnedCharacter = Instantiate(playerCharacterToSpawn,
                    playerCharacterSpawningTransform.position,
                    playerCharacterSpawningTransform.rotation);

        if (followCamera != null)
            followCamera.Target.TrackingTarget = spawnedCharacter.transform;
        else
            Debug.LogWarning("[DungeonManager] Camera is not set.");
    }
}