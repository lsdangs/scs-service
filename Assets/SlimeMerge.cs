using UnityEngine;

public class SlimeMerge : MonoBehaviour
{
    [Header("합체 설정")]
    [Tooltip("합체로 생겨날 다음 단계 슬라임 프리팹. 킹슬라임(최종)은 비워두면 됨")]
    public GameObject nextTierPrefab;

    private SlimeTier myTier;
    private bool merged = false;  // 중복 합체 방지

    void Start()
    {
        myTier = GetComponent<SlimeTier>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 이미 합쳐졌으면 무시
        if (merged) return;

        // 부딪힌 상대가 슬라임인지 확인
        SlimeTier otherTier = collision.gameObject.GetComponent<SlimeTier>();
        if (otherTier == null) return;  // 슬라임 아니면(벽,바닥 등) 무시

        SlimeMerge otherMerge = collision.gameObject.GetComponent<SlimeMerge>();
        if (otherMerge == null || otherMerge.merged) return;

        // 단계가 같을 때만 합체
        if (otherTier.tier != myTier.tier) return;

        // 최종 단계(다음 프리팹 없음)면 합체 안 함
        if (nextTierPrefab == null) return;

        // ★ 둘 중 하나만 합체 처리 (서로 동시에 처리하면 두 개 생김)
        // 자기 ID가 상대보다 작은 쪽이 대표로 처리
        if (GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            DoMerge(collision.gameObject);
        }
    }

    void DoMerge(GameObject other)
    {
        merged = true;

        // 두 슬라임의 중간 지점 계산
        Vector3 midPoint = (transform.position + other.transform.position) / 2f;

        // 다음 단계 슬라임을 중간 지점에 생성
        Instantiate(nextTierPrefab, midPoint, Quaternion.identity);

        // 합쳐진 두 슬라임 제거
        Destroy(other);
        Destroy(gameObject);

        Debug.Log("합체! 다음 단계 슬라임 생성됨");
    }
}