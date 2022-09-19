using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueSystemUI : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _optionButtonPrefab;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private bool _isPrinting;
    [SerializeField] private bool _stopTyper;

    public void Awake()
    {
        DialogueEvents.skipTalking += StopTypeWrite;
    }

    public void OnDisable()
    {
        _stopTyper = false;
        _isPrinting = false;
        for (int i = 0; i < _options.transform.childCount; i++)
        {
            _options.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        DialogueEvents.skipTalking -= StopTypeWrite;
    }

    public void DisplayDialogue(Entry entry)
    {
        _dialogueText.text = string.Empty;
        StartCoroutine(StartTypeWriter(entry));
    }

    public void TryDisplayOptions(Entry entry)
    {
        if (entry.GetType() != typeof(DecisionEntry)) { return; }
        
        DecisionEntry decisionEntry = (DecisionEntry)entry;
        _options.SetActive(true);
        for (int i = 0; i < decisionEntry.options.Count; i++)
        {
            Button optionButton = null;
            if (i >= _options.transform.childCount)
            {
                optionButton = Instantiate(_optionButtonPrefab, _options.transform).GetComponent<Button>();
                int optionIndex = i;
                optionButton.onClick.AddListener(() =>
                {
                    DialogueUIEvents.OptionPressed(optionIndex);
                    _options.SetActive(false);
                });
            }
            else
            {
                optionButton = _options.transform.GetChild(i).gameObject.GetComponent<Button>();
            }

            TextMeshProUGUI optionText = optionButton.transform.GetComponentInChildren<TextMeshProUGUI>();
            optionText.text = decisionEntry.GetOptionAtIndex(i).text;
            optionButton.gameObject.SetActive(true);
        }
    }

    private IEnumerator StartTypeWriter(Entry entry)
    {
        string text = entry.GetDialogueText();
        _isPrinting = true;
        
        for (int i = 0; i < text.Length; i++)
        {
            if (_stopTyper)
            {
                _dialogueText.text = text;
                break;
            }

            _dialogueText.text += text[i];
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        _stopTyper = false;
        _isPrinting = false;
        TryDisplayOptions(entry);
        DialogueUIEvents.SendPrintingCompleted();
    }

    private void StopTypeWrite()
    {
        _stopTyper = true;
    }

}
