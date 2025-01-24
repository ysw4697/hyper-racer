using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    [SerializeField] private GameObject[] gasObjects;

    private void OnDisable()
    {
        // 가스 오브젝트 비활성화
        foreach (GameObject gasObject in gasObjects)
        {
            gasObject.SetActive(false);
        }
    }

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
            GameManager.Instance.DestroyRoad(this.gameObject);
        }
    }

    /// <summary>
    /// 랜덤으로 가스 아이템을 표시
    /// </summary>
    public void SpawnGas()
    {
        int index = Random.Range(0, gasObjects.Length);
        gasObjects[index].SetActive(true);
    }
}
