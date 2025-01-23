using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private int gas = 100;
    [SerializeField] private float moveSpeed = 1.0f;
    
    public int Gas { get => gas; }      // Gas 정보

    private void Start()
    {
        StartCoroutine(GasCoroutine());
    }

    IEnumerator GasCoroutine()
    {
        while (true)
        {
            gas -= 10;
            if (gas <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(1.0f);
        }
        // TODO: 게임 종료 메서드 구현 예정
    }
    
    /// <summary>
    /// 자동차 이동 메서드
    /// </summary>
    /// <param name="direction"></param>
    public void Move(float direction)
    {
        transform.Translate(Vector3.right * (direction * moveSpeed * Time.deltaTime));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.0f, 2.0f), 0.0f, transform.position.z);
    }

    /// <summary>
    /// Gas 아이템 획득 시 호출되는 메서드
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gas"))
        {
            gas += 30;
            
            // TODO: 가스 아이템 풀링 디스트로이어 메서드
        }
    }
}
