using System.Collections;
using UnityEngine;

public class CharactersRosterRotation : MonoBehaviour
{
    [SerializeField]
    private float charactersRotationRate = 2f;
    [SerializeField]
    private AnimationCurve charactersRotationCurve = AnimationCurve.EaseInOut(1, 1, 0, 0);

    // @TODO: Make this constant's value depend on the characters number (360 / charactersNum)
    private const float rotationAmount = 120f;

    private void OnEnable()
    {
        CharacterSelectionManager.OnCharacterSwitched += RotateCharactersRoster;
    }

    private void OnDisable()
    {
        CharacterSelectionManager.OnCharacterSwitched -= RotateCharactersRoster;
    }

    // direction: 1 = right; -1 = left
    public void RotateCharactersRoster(int direction)
    {
        StartCoroutine(RotateCharacters(rotationAmount * direction));
    }

    private IEnumerator RotateCharacters(float yRot)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(transform.eulerAngles.x,
                                                transform.eulerAngles.y + yRot,
                                                transform.eulerAngles.z);
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime * charactersRotationRate;
            float curvedT = charactersRotationCurve.Evaluate(t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, curvedT);

            if (t >= 1f)
                break;

            yield return null;
        }

        transform.rotation = endRot;
    }
}
