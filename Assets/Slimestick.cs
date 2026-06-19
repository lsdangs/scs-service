using UnityEngine;

public class SlimeStick : MonoBehaviour
{
    [Header("점착 설정")]
    [Tooltip("체크하면 무언가에 닿는 순간 그 자리에 고정됨")]
    public bool stickOnContact = true;

    private Rigidbody2D rb;
    private bool stuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 무언가와 충돌하는 순간 자동 호출됨
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 점착 꺼져있거나 이미 붙었으면 무시
        if (!stickOnContact || stuck) return;

        Stick();
    }

    void Stick()
    {
        stuck = true;

        // 움직임 완전히 멈추고 그 자리에 고정
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;  // 물리 영향 안 받는 고정 상태로

        Debug.Log("슬라임 점착 고정됨!");  // 콘솔에 찍혀서 작동 확인용
    }
}