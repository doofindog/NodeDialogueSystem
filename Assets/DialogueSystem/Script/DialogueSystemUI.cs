using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using DialogueSystem;

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

    public void DisplayDialogue(Entry p_entry)
    {
        _dialogueText.text = string.Empty;
        
        StartCoroutine(StartTypeWriter(p_entry));
    }

    private void TryDisplayOptions(Entry p_entry)
    {
        if (p_entry.GetType() != typeof(DecisionEntry)) { return; }
        
        _options.SetActive(true);
        
        DecisionEntry decisionEntry = (DecisionEntry)p_entry;
        for (int i = 0; i < decisionEntry.options.Count; i++)
        {
            Button optionButton = null;
            if (i >= _options.transform.childCount)
            {
                int optionIndex = i;
                
                optionButton = Instantiate(_optionButtonPrefab, _options.transform).GetComponent<Button>();
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

    private IEnumerator StartTypeWriter(Entry p_entry)
    {
        _isPrinting = true;
        
        string text = p_entry.GetDialogueText();
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
        
        TryDisplayOptions(p_entry);
        
        DialogueUIEvents.SendPrintingCompleted();
    }

    private void StopTypeWrite()
    {
        _stopTyper = true;
    }

}
