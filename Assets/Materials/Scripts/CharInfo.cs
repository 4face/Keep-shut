using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public sealed class CharInfo : MonoBehaviour
{
    public string Name;
    public CinemachineVirtualCamera Camera;
    public string[] Dialogues;
    public string[] Questions;
    public string[] Answers;
    public TextAsset inkJSON;
    public DialogueTrigger dialogueTriggers;
}
