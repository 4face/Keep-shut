using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;
using System.Linq;
using System;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textComp;
    public string[] lines;
    public string[] questions;
    public bool InProgress = false;
    [SerializeField] private float textSpeed;

    private int index;
    private int indexQuestion;
    //void Start()
    //{
    //    textComp.text = string.Empty;
    //    StartDialogue();
    //}
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (index >= 0 && index < lines.Length && textComp.text == lines[index])
            {
                textComp.text = string.Empty;
                NextLine(lines);
            }
            // Проверяем, что indexQuestion находится в допустимых границах массива questions
            else if (indexQuestion >= 0 && indexQuestion < questions.Length && textComp.text == questions[indexQuestion])
            {
                textComp.text = string.Empty;
                NextLine(lines); // Возможно, здесь должен быть вызов NextLine(questions)
            }
            else
            {
                textComp.text = string.Empty;
                StopAllCoroutines();
                textComp.text = lines[index];
            }
        }   
    }
    public void StartDialogue()
    {
        InProgress = true;
        index = 0;
        indexQuestion = 0;
        StartCoroutine(TypeLine(lines,index));
    }
    public void StartQuestion(string[] textQuestion)
    {
        int indexOfNextQuestion = Array.FindIndex(lines, line => line == "<>");

        if (indexOfNextQuestion != -1)
        {
            List<string> tempList = lines.ToList();
            tempList.RemoveAt(indexOfNextQuestion);
            lines = tempList.ToArray();
        }
        index++;
        StartCoroutine(TypeLine(textQuestion, indexQuestion));
        indexQuestion++;
    }
    IEnumerator TypeLine(string[] text, int index)
    {
        foreach (char i in text[index].ToCharArray())
        {
            textComp.text += i;

            yield return new WaitForSeconds(textSpeed);
        }
    }
    private void NextLine(string[] text)
    {
        if (index < text.Length - 1 && text[index + 1] == "<>")
        {
            StartQuestion(questions);
            textComp.text += string.Empty;
        }
        else if(index < text.Length - 1)
        {
            index++;
            textComp.text += string.Empty;
            StartCoroutine(TypeLine(text, index));
        }
        else
        {
            InProgress = false;
            gameObject.SetActive(false);
        }
    }
}
