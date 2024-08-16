using UnityEngine;
using UnityEngine.Serialization;

public class Scanner : MonoBehaviour
{
    [FormerlySerializedAs("scanRange")]
    public float ScanRange;

    [FormerlySerializedAs("targetLayer")]
    public LayerMask TargetLayer;

    private RaycastHit2D[] _targets;

    [FormerlySerializedAs("nearestTarget")]
    public Transform NearestTarget;

    private void FixedUpdate()
    {
        // ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�
        _targets = Physics2D.CircleCastAll(transform.position, ScanRange, Vector2.zero, 0, TargetLayer);
        NearestTarget = GetNearest();
    }

    private Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (var target in _targets)
        {
            var myPos = transform.position;
            var targetPos = target.transform.position;
            var curDiff = Vector3.Distance(myPos, targetPos); // Distance(A,B) : A�� B�� �Ÿ��� ���

            if (!(curDiff < diff)) continue;
            diff = curDiff;
            result = target.transform;
        }

        return result;
    }
}