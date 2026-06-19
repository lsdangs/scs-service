using UnityEngine;

// 이 스크립트가 붙은 오브젝트 = 부숴야 할 목표물
public class Target : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.RegisterTarget(this);   // 생길 때 등록
    }

    public void DestroyTarget()
    {
        GameManager.UnregisterTarget(this); // 부서질 때 명단에서 빼기
        Destroy(gameObject);
    }
}