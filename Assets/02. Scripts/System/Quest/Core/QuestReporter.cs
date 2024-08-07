using System.Linq;
using UnityEngine;

public class QuestReporter : MonoBehaviour
{
    [SerializeField]
    private Category _category;
    [SerializeField]
    private TaskTarget _target;
    [SerializeField]
    private int _successCount;
    [SerializeField]
    private string[] _colliderTags;

    private void OnTriggerEnter(Collider other)
    {
        ReportIfPassCondition(other);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReportIfPassCondition(collision);
    }

    public void Report()
    {
        QuestSystem.Instance.ReceiveReport(_category, _target, _successCount);
    }

    private void ReportIfPassCondition(Component other)
    {
        if (_colliderTags.Any(other.CompareTag))
            Report();
    }
}
