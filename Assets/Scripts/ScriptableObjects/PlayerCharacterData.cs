using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Scriptable Objects/PlayerCharacterData")]
public class PlayerCharacterData : ScriptableObject
{
    public int Id => id;
    public string CharacterName => characterName;
    public GameObject Prefab => prefab;

    [SerializeField] private int id = 0;
    [SerializeField] private string characterName = "";
    [SerializeField] private GameObject prefab = null;
}