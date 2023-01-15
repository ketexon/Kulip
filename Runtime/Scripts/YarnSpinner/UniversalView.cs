using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;
using TMPro;
using UnityEngine.UI;

using Kulip.Utility;

namespace Kulip
{
    static class UniversalViewEffects
    {
        [System.Serializable]
        public class TypewriterSettings
        {
            [Tooltip("In characters per second")]
            public float SpeedMult = 15.0f;
            public bool SkipWhitespace = false;
            public bool ImmediatelyShowFirstCharacter = false;
            [Tooltip("Whether the speed should affect the character before or after it is shown")]
            public bool SpeedChangeStartsAfterFirstCharacterShown = true;
            [Tooltip("Whether the speed should affect the last character before or after it is shown")]
            public bool SpeedChangeEndsAfterLastCharacterShown = false;
        }

        public static IEnumerator TypewriterEffect(TypewriterSettings settings, LocalizedLine line, TMP_Text tmp, Action finishedPresenting)
        {
            // the positions where the speed should return back to normal
            // eg. "[speed=1]Hello[speed=2] there[/speed] my friend[/speed]
            // When [speed=2] is parsed, it will push the previous speed (1) 
            // and the position of the end of the attribtue (before "there")
            // so that once that position is passed, the speed will return
            // to the speed prior
            var speedReturnStack = new Stack<(int, float)>();
            // tuple of character position and speed change
            // note, position is note unique, but it is ordered
            var speedChanges = new Queue<(int, float)>();

            var pauses = new Queue<(int, float)>();

            float lastSpeed = 1;
            foreach(var attribute in line.TextWithoutCharacterName.Attributes)
            {
                if(attribute.Name == "speed")
                {
                    while (speedReturnStack.TryPeek(out var speedReturn) && speedReturn.Item1 <= attribute.Position)
                    {
                        speedChanges.Enqueue(speedReturnStack.Pop());
                    }
                    var newSpeed = attribute.Properties["speed"].FloatValue;
                    var start = attribute.Position;
                    var end = attribute.Position + attribute.Length + 1;
                    var length = attribute.Length + 1;
                    if (attribute.Properties.TryGetValue("starts", out var startsProperty))
                    {
                        var when = startsProperty.StringValue;
                        switch (when)
                        {
                            case "before": 
                                break;
                            case "after": 
                                start++;
                                break;
                            default: 
                                Debug.LogErrorFormat("Line with ID {0} has invalid \"starts\" attribute", line.TextID);
                                break;
                        }
                    }
                    else
                    {
                        start += settings.SpeedChangeStartsAfterFirstCharacterShown ? 1 : 0;
                    }

                    if (attribute.Properties.TryGetValue("ends", out var endsProperty))
                    {
                        var when = endsProperty.StringValue;
                        switch (when)
                        {
                            case "before":
                                break;
                            case "after":
                                end++;
                                break;
                            default:
                                Debug.LogErrorFormat("Line with ID {0} has invalid \"starts\" attribute", line.TextID);
                                break;
                        }
                    }
                    else
                    {
                        end += settings.SpeedChangeEndsAfterLastCharacterShown ? 1 : 0;
                    }
                    speedChanges.Enqueue((start, newSpeed));
                    speedReturnStack.Push((end, lastSpeed));
                    lastSpeed = newSpeed;
                }
                else if(attribute.Name == "pause")
                {
                    pauses.Enqueue((attribute.Position, attribute.Properties["pause"].FloatValue));
                }
            }
            while(speedReturnStack.TryPop(out var speedReturn))
            {
                speedChanges.Enqueue(speedReturn);
            }

            float currentSpeed = 1;

            var richText = new LazyRichText(tmp.text);
            var plainText = richText.PlainText;
            int nCharacters = plainText.Length;
            tmp.maxVisibleCharacters = settings.ImmediatelyShowFirstCharacter ? 1 : 0;
            while(tmp.maxVisibleCharacters < nCharacters)
            {
                if (settings.SkipWhitespace && char.IsWhiteSpace(plainText[tmp.maxVisibleCharacters]))
                {
                    tmp.maxVisibleCharacters++;
                    continue;
                }
                while(
                    speedChanges.TryPeek(out var speedChange) 
                    && tmp.maxVisibleCharacters >= speedChange.Item1
                ) {
                    speedChanges.Dequeue();
                    currentSpeed = speedChange.Item2;
                }
                while (pauses.TryPeek(out var pause) && pause.Item1 <= tmp.maxVisibleCharacters)
                {
                    pauses.Dequeue();
                    yield return new WaitForSeconds(pause.Item2);
                }
                yield return new WaitForSeconds(1 / (currentSpeed * settings.SpeedMult));
                tmp.maxVisibleCharacters++;
            }
            finishedPresenting();
        }
    }

    public class UniversalView : DialogueViewBase
    {
        [SerializeField] CanvasGroup _canvasGroup;

        [SerializeField] CanvasGroup _lineCanvasGroup;
        [SerializeField] TMP_Text _lineCharacterName;
        [SerializeField] TMP_Text _lineText;
        [SerializeField] Button _lineContinueButton;

        [SerializeField] bool _useTypewriter = true;
        [SerializeField] UniversalViewEffects.TypewriterSettings _typewriterSettings;

        [SerializeField] CanvasGroup _optionsListViewCanvasGroup;
        [SerializeField] GameObject _optionsListParent;
        [SerializeField] GameObject _optionPrefab;
        [SerializeField] List<RectTransform> _optionsListLayoutsToRebuild = new List<RectTransform>();
        [SerializeField] bool _rebuildTwice = false;

        LocalizedLine _currentLine = null;
        Action _onCurrentLineFinished = null;
        bool _canInterruptLine = false;
        bool _canFinishLine = false;

        Coroutine _typewriterCoroutine = null;

        List<UniversalViewOption> _options = new List<UniversalViewOption>();

        void Awake()
        {
            DisableAllViews();
            DestroyOptionsListParentChildren();

            if (_lineContinueButton != null)
            {
                _lineContinueButton.onClick.AddListener(UserRequestedViewAdvancement);
            }
        }

        void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void DismissLine(Action onDismissalComplete)
        {
            onDismissalComplete();
        }

        public override void DialogueComplete()
        {
            DisableAllViews();
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            _currentLine = dialogueLine;
            
            _canFinishLine = false;
            _canInterruptLine = true;
            _onCurrentLineFinished = onDialogueLineFinished;

            UpdateLineViewText(dialogueLine);

            EnableLineView();
            EnableContinueButton();

            if (_useTypewriter)
            {
                _typewriterCoroutine = StartCoroutine(
                    UniversalViewEffects.TypewriterEffect(
                        _typewriterSettings,
                        dialogueLine,
                        _lineText,
                        PresentLine
                    )
                );
            }
        }

        void PresentLine()
        {
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                _typewriterCoroutine = null;
            }
            _canFinishLine = true;
            _canInterruptLine = false;

            EnableContinueButton();
            ShowFullLine();
        }

        /// <summary>
        /// Called when the line should complete as quickly as possible.
        /// It should cancel any effects and show the line.
        /// </summary>
        public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            if(_currentLine != dialogueLine)
            {
                UpdateLineViewText(dialogueLine);
            }
            _currentLine = dialogueLine;

            PresentLine();
            _onCurrentLineFinished = onDialogueLineFinished;
        }

        public override void UserRequestedViewAdvancement()
        {
            if(_canInterruptLine)
            {
                requestInterrupt?.Invoke();
            }
            else if (_canFinishLine)
            {
                _onCurrentLineFinished?.Invoke();
            }
        }

        #region Global
        void EnableCanvasGroup()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            EnableAllInteraction();
        }

        void DisableAllViews()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;

            DisableLineView();
            DisableOptionsListView();
        }

        void EnableAllInteraction()
        {
            _canvasGroup.interactable = true;
        }

        void DisableAllInteraction()
        {
            _canvasGroup.interactable = false;
        }
        #endregion


        #region LineView
        void EnableLineView()
        {
            EnableCanvasGroup();
            _lineCanvasGroup.alpha = 1;
            _lineCanvasGroup.blocksRaycasts = true;
        }

        void DisableLineView()
        {
            _lineCanvasGroup.alpha = 1;
            _lineCanvasGroup.blocksRaycasts = true;
            DisableContinueButton();
        }

        void UpdateLineViewText(LocalizedLine dialogueLine)
        {
            if (dialogueLine is LocalizedLine line)
            {
                if(_lineText != null)
                {
                    _lineText.text = line.TextWithoutCharacterName.Text;
                }
                if(_lineCharacterName != null)
                {
                    _lineCharacterName.text = line.CharacterName;
                }
            }
        }

        void ShowFullLine()
        {
            EnableLineView();
            _lineText.maxVisibleCharacters = int.MaxValue;
        }

        void EnableContinueButton()
        {
            if (_lineContinueButton != null)
            {
                _lineContinueButton.enabled = true;
            }
        }

        void DisableContinueButton()
        {
            if(_lineContinueButton != null)
            {
                _lineContinueButton.enabled = false;
            }
        }
        #endregion


        #region OptionsListView
        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            DisableContinueButton();
            _canInterruptLine = false;
            _canFinishLine = false;

            EnableOptionsListView();
            DestroyAllOptions();
            
            void OnOptionSelected(int optionID)
            {
                DisableOptionsListView();
                onOptionSelected(optionID);
            }

            foreach (var dialogueOption in dialogueOptions)
            {
                var optionGO = Instantiate(_optionPrefab);
                optionGO.name = _optionPrefab.name + dialogueOption.DialogueOptionID;
                optionGO.transform.SetParent(_optionsListParent.transform);
                var option = optionGO.GetComponent<UniversalViewOption>();
                option.SetDialogueOption(dialogueOption);
                option.OnSelect = OnOptionSelected;
                _options.Add(option);
                LayoutRebuilder.MarkLayoutForRebuild(optionGO.GetComponent<RectTransform>());
            }
            foreach (var layout in _optionsListLayoutsToRebuild)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
                if (_rebuildTwice)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
                }
            }
        }

        void EnableOptionsListView()
        {
            EnableCanvasGroup();
            _optionsListViewCanvasGroup.alpha = 1;
            _optionsListViewCanvasGroup.blocksRaycasts = true;
        }

        void DisableOptionsListView()
        {
            DestroyAllOptions();
            _optionsListViewCanvasGroup.alpha = 0;
            _optionsListViewCanvasGroup.blocksRaycasts = false;
        }


        void DestroyAllOptions()
        {
            foreach(var option in _options){
                Destroy(option.gameObject);
            }
            _options.Clear();
        }

        void DestroyOptionsListParentChildren()
        {
            foreach(Transform child in _optionsListParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion
    }
}
