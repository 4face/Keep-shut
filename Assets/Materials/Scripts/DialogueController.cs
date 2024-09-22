using UnityEngine;
using TMPro;
using Ink.Runtime;

/// <summary>
///  Controller for do dialogues.
/// </summary>
public class DialogueController : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private CharacterControl characterControl;
    private Story currentStory;
    [Header("Bool handle")]
    public bool isPlaying;

    private static DialogueController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static DialogueController GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        characterControl = CharacterControl.GetInstance();
    }

    private void Update()
    {
        if (isPlaying && Input.GetMouseButtonUp(0))
        {
            ContinueStory();
        }
    }

    /// <summary>
    /// Start dialogue with using JSON-data.
    /// </summary>
    /// <param name="inkJSON">JSON-file of dialogue.</param>
    public void EnterDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    /// <summary>
    /// Continue dialogue or finish it, if story ended.
    /// </summary>
    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogue();
        }
    }

    /// <summary>
    /// Finish dialogue.
    /// </summary>
    private void ExitDialogue()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        Cursor.visible = false;
    }
}