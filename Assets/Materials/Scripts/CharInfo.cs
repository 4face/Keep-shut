using UnityEngine;
using Cinemachine;
using System.Collections;
using Dreamteck.Splines;

/// <summary>
/// Contain information of character for interacting and dialogues.
/// </summary>
public sealed class CharInfo : MonoBehaviour
{
    [Header("Character Info")]
    public string Name;
    public CinemachineVirtualCamera CharCamera;
    public TextAsset inkJSON;
    public bool dialogueReady;

    private Animator animator;
    private DialogueController dialogueController;
    private SplineFollower splineFollower;
    private CharacterControl characterControl;

    private const float dialogueEnterDuration = 1f;
    private const float dialogueEndDelay = 1f;
    private const float cooldownDuration = 3f;

    private void Start()
    {
        dialogueReady = true;
        dialogueController = DialogueController.GetInstance();
        splineFollower = GetComponent<SplineFollower>();
        characterControl = CharacterControl.GetInstance();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (dialogueController.isPlaying && splineFollower.follow)
        {
            PrepareForDialogue();
        }
        else if (!dialogueController.isPlaying && !splineFollower.follow)
        {
            EndDialogue();
        }
    }

    private void PrepareForDialogue()
    {
        dialogueReady = false;
        splineFollower.follow = false;
        animator.SetBool("inDialogue", true);
        StartCoroutine(DialogueEnterPose());
    }

    private void EndDialogue()
    {
        dialogueReady = false;
        CharCamera.gameObject.SetActive(false);
        StartCoroutine(DialogueEnd(dialogueEndDelay));
    }

    private IEnumerator DialogueEnterPose()
    {
        Vector3 directionToPlayer = characterControl.transform.position - transform.position;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        float timeElapsed = 0;

        while (timeElapsed < dialogueEnterDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, timeElapsed / dialogueEnterDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        CharCamera.gameObject.SetActive(true);
    }

    private IEnumerator DialogueEnd(float time)
    {
        yield return new WaitForSeconds(time);
        splineFollower.follow = true;
        animator.SetBool("inDialogue", false);
        StartCoroutine(DialogueCooldown(cooldownDuration));
    }

    private IEnumerator DialogueCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        dialogueReady = true;
    }
}