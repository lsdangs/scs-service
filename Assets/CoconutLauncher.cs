using UnityEngine;

public class CoconutLauncher : MonoBehaviour
{
    [Header("발사 설정")]
    [Tooltip("발사할 작은 코코넛 프리팹 (Coconut_S 넣기)")]
    public GameObject coconutPrefab;

    [Tooltip("발사 세기")]
    public float launchPower = 10f;

    [Tooltip("다음 코코넛 장전까지 딜레이(초)")]
    public float reloadDelay = 0.3f;

    [Header("조준선")]
    public Color aimLineColor = Color.white;

    private GameObject loadedCoconut;   // 현재 장전된 코코넛
    private Rigidbody2D loadedRb;
    private Camera cam;
    private LineRenderer aimLine;
    private bool canFire = false;

    void Start()
    {
        cam = Camera.main;

        // 조준선 준비
        aimLine = gameObject.AddComponent<LineRenderer>();
        aimLine.material = new Material(Shader.Find("Sprites/Default"));
        aimLine.startColor = aimLineColor;
        aimLine.endColor = aimLineColor;
        aimLine.startWidth = 0.1f;
        aimLine.endWidth = 0.1f;
        aimLine.positionCount = 2;
        aimLine.sortingOrder = 10;

        Reload();  // 첫 코코넛 장전
    }

    void Reload()
    {
        // 발사 위치(이 발사대 위치)에 코코넛 생성
        loadedCoconut = Instantiate(coconutPrefab, transform.position, Quaternion.identity);
        loadedRb = loadedCoconut.GetComponent<Rigidbody2D>();

        // 장전 중엔 중력 끄고 공중에 대기
        loadedRb.gravityScale = 0f;
        loadedRb.linearVelocity = Vector2.zero;

        canFire = true;
        aimLine.enabled = true;
    }

    void Update()
    {
        if (!canFire || loadedCoconut == null) return;

        // 마우스 방향 계산
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2 dir = ((Vector2)(mouseWorld - transform.position)).normalized;

        // 조준선 그리기
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, transform.position + (Vector3)(dir * 2f));

        // 클릭하면 발사
        if (Input.GetMouseButtonDown(0))
        {
            Fire(dir);
        }
    }

    void Fire(Vector2 direction)
    {
        canFire = false;
        aimLine.enabled = false;

        GameManager.UseTurn();   // ★ 이 줄 추가: 턴 하나 사용

        // 장전된 코코넛 발사
        loadedRb.gravityScale = 1f;
        loadedRb.linearVelocity = direction * launchPower;

        loadedCoconut = null;

        // 잠시 후 다음 코코넛 장전
        Invoke(nameof(Reload), reloadDelay);
    }
}