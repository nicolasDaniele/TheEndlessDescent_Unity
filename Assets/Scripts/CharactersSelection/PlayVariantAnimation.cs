using UnityEngine;

// This behaviour allows only the selected character in Character Selection Screen
// to play an Idle Variant animation every so often.
// The non selected characters must play only the standart Idle animation
public class PlayVariantAnimation : StateMachineBehaviour
{
    [SerializeField]
    private float minTimeToPlayVariant = 8f;
    [SerializeField]
    private float maxTimeToPlayVariant = 12f;
    [SerializeField]
    private string variantAnimationName = "";

    private float timeElapsed;
    private float timeToPlayVariant;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetTimer();

        if (!CheckForSelectedAnimator(animator))
            return;

        CharacterSelectionManager.OnCharacterSwitched += ResetTimer;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!CheckForSelectedAnimator(animator))
            return;

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeToPlayVariant)
            PlayVariant(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!CheckForSelectedAnimator(animator))
            return;

        CharacterSelectionManager.OnCharacterSwitched -= ResetTimer;
    }

    private void PlayVariant(Animator animator)
    {
        if (!CheckForSelectedAnimator(animator))
            return;

        animator.CrossFade(variantAnimationName, 0);
    }

    // This funtion triggers when the selected character is switched in CharacterSelectionManager
    // The action returns an int type, but it is not used here
    private void ResetTimer(int index = 0)
    {
        timeElapsed = 0f;
        timeToPlayVariant = UnityEngine.Random.Range(minTimeToPlayVariant, maxTimeToPlayVariant);
    }

    private bool CheckForSelectedAnimator(Animator animator)
    {
        return animator.GetBool("IsCharacterSelected");
    }
}