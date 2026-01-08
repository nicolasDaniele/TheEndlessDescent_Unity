using UnityEngine;

// This is a singleton class meant to access the PlayerCharacterDataBase
// Ohter classes should use this accessor to query the database
public class PlayerCharacterDatabaseAccessor : MonoBehaviour
{
    public static PlayerCharacterDatabaseAccessor Instance => instance;

    private static PlayerCharacterDatabase characterDatabase;

    private static PlayerCharacterDatabaseAccessor instance;

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

        characterDatabase = Resources.Load<PlayerCharacterDatabase>("ScriptableObjects/PlayerCharacterDatabase");

        if (characterDatabase == null)
            Debug.LogWarning("[PCDB_Accesor] CharacterDatabase is null");
    }

    public static GameObject GetPlayerCharacterPrefab()
    {
        return characterDatabase == null ? null :
                characterDatabase.SelectedPlayerCharacterPrefab;
    }

    public static PlayerCharacterStats GetPlayerCharacterStats()
    {
        return characterDatabase == null ? null :
                characterDatabase.SelectedPlayerCharacterStats;
    }
}