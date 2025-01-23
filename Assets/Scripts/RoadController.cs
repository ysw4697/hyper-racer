using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    /// <summary>
    /// 플레이어 차향이 도로에 진입하면 다음 도로를 생성
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SpwanRoad(transform.position + new Vector3(0.0f, 0.0f, 10.0f));
        }
    }

    /// <summary>
    /// 플레이어 차량이 도로를 벗어나면 해당 도로를 제거
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }
}
