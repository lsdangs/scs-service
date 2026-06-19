using UnityEngine;

public class SlimeTier : MonoBehaviour
{
    [Header("슬라임 단계")]
    [Tooltip("1=작은, 2=중간, 3=킹슬라임")]
    public int tier = 1;

    [Header("단계별 크기 (보기 좋게)")]
    [Tooltip("이 단계일 때의 크기")]
    public float tierScale = 1f;

    void Start()
    {
        // 시작할 때 단계에 맞는 크기로 맞춤
        ApplyScale();
    }

    public void ApplyScale()
    {
        transform.localScale = new Vector3(tierScale, tierScale, 1f);
    }
}