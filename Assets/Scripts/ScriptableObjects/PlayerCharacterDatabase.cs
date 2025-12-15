using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterDatabase", menuName = "Scriptable Objects/PlayerCharacterDatabase")]
public class PlayerCharacterDatabase : ScriptableObject
{
    public PlayerCharacterData[] PlayerCharacters => playerCharacters;
    public int PlayerCharactersCount => playerCharacters.Length;
    public GameObject SelectedPlayerCharacterPrefab => 
                        playerCharacters[selectedPlayerCharacterIndex].Prefab;
    public PlayerCharacterStats SelectedPlayerCharacterStats =>
                        playerCharacters[selectedPlayerCharacterIndex].CharacterStats;

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
}