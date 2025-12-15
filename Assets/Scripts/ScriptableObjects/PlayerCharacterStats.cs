using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacterStats", menuName = "Scriptable Objects/PlayerCharacterStats")]
public class PlayerCharacterStats : ScriptableObject
{
    public float Speed => speed;
    public float Attack => attack;
    public float Defense => defense;

    [SerializeField] public float speed = 0f;
    [SerializeField] public float attack = 0f;
    [SerializeField] public float defense = 0f;
}