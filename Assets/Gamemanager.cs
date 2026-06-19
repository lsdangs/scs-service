using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("턴 설정")]
    [Tooltip("사용 가능한 총 발사 횟수")]
    public int maxTurns = 8;

    private int turnsUsed = 0;
    private bool gameOver = false;

    // 살아있는 목표물 명단 (모든 슬라임/타겟이 공유)
    private static List<Target> targets = new List<Target>();
    private static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    public static void RegisterTarget(Target t)
    {
        if (!targets.Contains(t)) targets.Add(t);
    }

    public static void UnregisterTarget(Target t)
    {
        targets.Remove(t);

        // 목표물 다 부숴졌나 확인
        if (instance != null) instance.CheckClear();
    }

    void CheckClear()
    {
        if (gameOver) return;

        if (targets.Count == 0)
        {
            gameOver = true;
            Debug.Log("★ 클리어! 목표물 전부 파괴!");
        }
    }

    // 발사할 때마다 호출됨 (발사대가 부름)
    public static void UseTurn()
    {
        if (instance != null) instance.CountTurn();
    }

    void CountTurn()
    {
        if (gameOver) return;

        turnsUsed++;
        int left = maxTurns - turnsUsed;
        Debug.Log("발사! 남은 턴: " + left);

        // 턴 다 썼는데 목표물 남아있으면 실패
        if (turnsUsed >= maxTurns && targets.Count > 0)
        {
            gameOver = true;
            Debug.Log("? 실패... 턴을 다 썼지만 목표물이 남음: " + targets.Count + "개");
        }
    }

    void OnDestroy()
    {
        // 씬 다시 시작할 때 명단 초기화 (안 하면 이전 판 데이터 남음)
        targets.Clear();
    }
}