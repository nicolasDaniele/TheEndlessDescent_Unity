using System;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    // int direction: 1 = right (next character);
    // -1 = left (previous character)
    public static Action<int> OnCharacterSwitched;
    public static Action<string> OnCharacterNameUpdated;

    [SerializeField]
    private PlayerCharacterDatabase characterDatabase;

    private int selectedCharacter = 0;

    private void Start()
    {
        OnCharacterNameUpdated?.Invoke(characterDatabase.PlayerCharacters[selectedCharacter].CharacterName);
    }

    public void NextCharacter()
    {
        selectedCharacter++;

        if(selectedCharacter >= characterDatabase.PlayerCharactersCount)
        {
            selectedCharacter = 0;
        }

        OnCharacterSwitched?.Invoke(1);

        OnCharacterNameUpdated?.Invoke(characterDatabase.PlayerCharacters[selectedCharacter].CharacterName);
    }

    public void PreviousCharacter()
    {
        selectedCharacter--;

        if (selectedCharacter < 0)
        {
            selectedCharacter = characterDatabase.PlayerCharactersCount -1;
        }

        OnCharacterSwitched?.Invoke(-1);

        OnCharacterNameUpdated?.Invoke(characterDatabase.PlayerCharacters[selectedCharacter].CharacterName);
    }

    public void SaveSelecterCharacter()
    {
        characterDatabase.SetSelectedPlayerCharacter(selectedCharacter);
    }
}