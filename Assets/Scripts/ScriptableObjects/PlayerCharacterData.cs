using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Scriptable Objects/PlayerCharacterData")]
public class PlayerCharacterData : ScriptableObject
{
    public int Id => id;
    public string CharacterName => characterName;
    public string CharacterClasss => characterClass;
    public GameObject Prefab => prefab;
    public PlayerCharacterStats CharacterStats => characterStats;

    [SerializeField] private int id = 0;
    [SerializeField] private string characterName = "";
    [SerializeField] private string characterClass = "";
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private PlayerCharacterStats characterStats = null;
}