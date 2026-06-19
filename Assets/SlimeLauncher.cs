using UnityEngine;

public class SlimeLauncher : MonoBehaviour
{
    [Header("발사 설정 - 여기 숫자 바꿔가며 손맛 조절")]
    [Tooltip("발사 세기. 클수록 멀리/빠르게 날아감")]
    public float launchPower = 10f;

    [Tooltip("조준선 색")]
    public Color aimLineColor = Color.white;

    private Rigidbody2D rb;
    private Camera cam;
    private bool launched = false;
    private LineRenderer aimLine;

    void Start()
    {
        // 이 슬라임의 물리 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        // 발사 전엔 중력 안 받게 (공중에 떠서 조준 대기)
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        // 조준선 만들기
        aimLine = gameObject.AddComponent<LineRenderer>();
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = aimLineColor;
        aimLine.endColor = aimLineColor;
        aimLine.startWidth = 0.1f;
        aimLine.endWidth = 0.1f;
        aimLine.positionCount = 2;
        aimLine.sortingOrder = 10;
    }

    void Update()
    {
        // 이미 발사했으면 아무것도 안 함
        if (launched) return;

        // 마우스 위치를 게임 월드 좌표로 변환
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        // 슬라임에서 마우스로 향하는 방향
        Vector2 dir = ((Vector2)(mouseWorld - transform.position)).normalized;

        // 조준선 그리기 (슬라임 위치 → 마우스 방향으로 살짝)
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, transform.position + (Vector3)(dir * 2f));

        // 마우스 왼쪽 클릭하면 발사
        if (Input.GetMouseButtonDown(0))
        {
            Launch(dir);
        }
    }

    void Launch(Vector2 direction)
    {
        launched = true;
        rb.gravityScale = 1f;              // 이제 중력 받음
        rb.linearVelocity = direction * launchPower;  // 그 방향으로 발사
        aimLine.enabled = false;           // 조준선 끄기
    }
}