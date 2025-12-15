using System.Collections.Generic;
using UnityEngine;

// This behaviour determines the aspect of the characters in the Character Selection screen
// based on wether they are currently selected
public class AppearanceInCharacterSelection : MonoBehaviour
{
    [SerializeField]
    private string characterName;
    [SerializeField]
    private bool isCharacterSelected = false;
    
    [Header("Mesh Materials")]
    [SerializeField]
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField]
    private Material unselectedCharacterMaterial;

    private Dictionary<SkinnedMeshRenderer, Material> defaultMaterials = 
                                                new Dictionary<SkinnedMeshRenderer, Material>();
    [SerializeField]
    private Animator animator;
    
    private void Awake()
    {
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();

        foreach (var renderer in skinnedMeshRenderers)
        {
            defaultMaterials.Add(renderer, renderer.material);
        }
    }

    private void Start()
    {
        UpdateMaterials();
        SetIsSelectedAnimatorParam(isCharacterSelected);
    }

    private void OnEnable()
    {
        CharacterSelectionManager.OnCharacterNameUpdated += OnSelectedCharacterSwitched;
    }

    private void OnDisable()
    {
        CharacterSelectionManager.OnCharacterNameUpdated -= OnSelectedCharacterSwitched;
    }

    // When CharacterSelectionManager switches the character,
    // this fucntion checks if the material of the meshes must be updated
    private void OnSelectedCharacterSwitched(string newCharacterName)
    {
        if(characterName.Equals(newCharacterName))
        {
            isCharacterSelected = true;

            UpdateMaterials();

            SetIsSelectedAnimatorParam(true);
        }
        else if(isCharacterSelected)
        {
            isCharacterSelected = false;
            
            UpdateMaterials();

            SetIsSelectedAnimatorParam(false);
        }
    }

    private void UpdateMaterials()
    {
        if (isCharacterSelected)
        {
            foreach(var renderer in skinnedMeshRenderers)
            {
                renderer.material = defaultMaterials[renderer];
            }
        }
        else
        {
            foreach (var renderer in skinnedMeshRenderers)
            {
                renderer.material = unselectedCharacterMaterial;
            }
        }
    }

    private void SetIsSelectedAnimatorParam(bool _enabled)
    {
        animator?.SetBool("IsCharacterSelected", _enabled);
    }
}