using Quest.Core.Task;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Quest.UI.Quest_Tracker
{
    public class TaskDescriptor : MonoBehaviour
    {
        [FormerlySerializedAs("text")]
        [SerializeField]
        private TextMeshProUGUI _text;

        [FormerlySerializedAs("normalColor")]
        [SerializeField]
        private Color _normalColor;

        [FormerlySerializedAs("taskCompletionColor")]
        [SerializeField]
        private Color _taskCompletionColor;

        [FormerlySerializedAs("taskSuccessCountColor")]
        [SerializeField]
        private Color _taskSuccessCountColor;

        [FormerlySerializedAs("strikeThroughColor")]
        [SerializeField]
        private Color _strikeThroughColor;

        public void UpdateText(string text)
        {
            _text.fontStyle = FontStyles.Normal;
            _text.text = text;
        }

        public void UpdateText(Task task)
        {
            _text.fontStyle = FontStyles.Normal;

            if (task.IsComplete)
            {
                var colorCode = ColorUtility.ToHtmlStringRGB(_taskCompletionColor);
                _text.text = BuildText(task, colorCode, colorCode);
            }
            else
                _text.text = BuildText(task, ColorUtility.ToHtmlStringRGB(_normalColor),
                    ColorUtility.ToHtmlStringRGB(_taskSuccessCountColor));
        }

        public void UpdateTextUsingStrikeThrough(Task task)
        {
            var colorCode = ColorUtility.ToHtmlStringRGB(_strikeThroughColor);
            _text.fontStyle = FontStyles.Strikethrough;
            _text.text = BuildText(task, colorCode, colorCode);
        }

        private string BuildText(Task task, string textColorCode, string successCountColorCode)
        {
            return
                $"<color=#{textColorCode}>�� {task.Description} <color=#{successCountColorCode}>{task.CurrentSuccess}</color>/{task.NeedSuccessToComplete}</color>";
        }
    }
}