using UnityEngine;
using UnityEngine.UI;

public class CharactersSelectionUI : MonoBehaviour
{
    [SerializeField]
    private Text characterNameText;

    private void OnEnable()
    {
        CharacterSelectionManager.OnCharacterNameUpdated += UpdateCharacterNameText;
    }

    private void OnDisable()
    {
        CharacterSelectionManager.OnCharacterNameUpdated += UpdateCharacterNameText;
    }

    private void UpdateCharacterNameText(string newCharacterName)
    {
        if (characterNameText == null)
        {
            Debug.LogWarning("CharacterNameText Not Set");
        }

        characterNameText.text = newCharacterName;
    }
}
