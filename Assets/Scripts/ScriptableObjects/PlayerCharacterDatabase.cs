using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterDatabase", menuName = "Scriptable Objects/PlayerCharacterDatabase")]
public class PlayerCharacterDatabase : ScriptableObject
{
    public PlayerCharacterData[] PlayerCharacters => playerCharacters;
    public int PlayerCharactersCount => playerCharacters.Length;

    [SerializeField] private PlayerCharacterData[] playerCharacters = { };
    private int selectedPlayerCharacterIndex;

    public PlayerCharacterData GetPlayerCharacterByIndex(int index)
    {
        return playerCharacters[index];
    }

    public void SetSelectedPlayerCharacter(int index)
    {
        selectedPlayerCharacterIndex = index;
    }

    public GameObject GetSelectedPlayerCharacterPrefab()
    {
        return playerCharacters[selectedPlayerCharacterIndex].Prefab;
    }
}