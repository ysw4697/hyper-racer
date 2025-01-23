using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // 프리팹
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject roadPrefab;
    
    // UI 관련
    [SerializeField] private MoveButton leftMoveButton;
    [SerializeField] private MoveButton rightMoveButton;
    [SerializeField] private TMP_Text gasText;
    
    // 자동차
    private CarController _carController;
    
    // 도로 오브젝트 풀
    private Queue<GameObject> _roadPool = new ();
    private int _roadPoolSize = 3;
    
    // 도로 이동
    private List<GameObject> _activeRoads = new ();
    
    // 상태
    public enum State { Start, Play, End }
    public State GameState { get; private set; } = State.Start;
    
    // 싱글톤
    public static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // Road 오브젝트 풀 초기화
        InitializeRoadPool();
        
        // 게임 시작 메서드
        StartGame();
    }

    private void Update()
    {
        // 활성화된 도로를 아래로 서서히 이동
        foreach (var activeRoad in _activeRoads)
        {
            activeRoad.transform.Translate(Vector3.back * Time.deltaTime);
        }
        
        // Gas 정보 출력
        if (_carController != null)
        {
            gasText.text = $" Gas: {_carController.Gas}";
        }
    }
    
    private void StartGame()
    {
        // 도로 생성
        SpwanRoad(Vector3.zero);
        
        // 자동차 생성
        _carController = Instantiate(carPrefab, new Vector3(0.0f ,0.0f, -3.0f), Quaternion.identity)
                                      .GetComponent<CarController>();
        
        // Left, Right move button에 자동차 컨트롤 기능 적용
        leftMoveButton.OnMoveButtonDown += () => _carController.Move(-1.0f);
        rightMoveButton.OnMoveButtonDown += () => _carController.Move(1.0f);
    }
    
    #region 도로 생성 및 관리
    
    /// <summary>
    /// 도로 오브젝트 풀 초기화
    /// </summary>
    private void InitializeRoadPool()
    {
        for (int i = 0; i < _roadPoolSize; i++)
        {
            GameObject road = Instantiate(roadPrefab);
            road.SetActive(false);
            _roadPool.Enqueue(road);
        }
    }
       
    /// <summary>
    ///  도로 오브젝트 풀에서 불러와 배치하는 메서드
    /// </summary>
    public void SpwanRoad(Vector3 position)
    {
        if (_roadPool.Count > 0)
        {
            GameObject road = _roadPool.Dequeue();
            road.transform.position = position;
            road.SetActive(true);
            
            // 활성화된 길을 움직이기 위해 list에 저장
            _activeRoads.Add(road);
        }
        else
        {
            GameObject road = Instantiate(roadPrefab, position, Quaternion.identity);
            _activeRoads.Add(road);
        }
    }

    public void DestroyRoad(GameObject road)
    {
        road.SetActive(false);
        _activeRoads.Remove(road);
        _roadPool.Enqueue(road);
    }
    
    #endregion
   
}
