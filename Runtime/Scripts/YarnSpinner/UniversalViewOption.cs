using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Yarn.Unity;

namespace Kulip
{
    public class UniversalViewOption : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] TMP_Text _text;

        public System.Action<int> OnSelect;

        int _id;

        public void SetDialogueOption(DialogueOption option)
        {
            _text.text = option.Line.Text.Text;
            _id = option.DialogueOptionID;
        }

        void Reset()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<TMP_Text>();
        }

        void Awake()
        {
            _button.onClick.AddListener(() => OnSelect(_id));
        }
    }
}
