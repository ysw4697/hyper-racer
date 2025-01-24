using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameTestScript
{
    private CarController _carController;
    private GameObject _leftMoveButton;
    private GameObject _rightMoveButton;
    
    // A Test behaves as an ordinary method
    [Test]
    public void GameTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        
        // 타입스케일 변경
        Time.timeScale = 5.0f;
        
        // 씬 로드하기
        SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
        yield return waitForSceneLoad();
        
        // 필수 오브젝트 확인
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Assert.IsNotNull(gameManager, "GameManager not found");
        
        GameObject startButton = GameObject.Find("Start Button");
        Assert.IsNotNull(startButton, "StartButton not found");
        
        // 게임 실행
        startButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        
        // 플레이어 자동차 확인
        _carController = GameObject.Find("Car(Clone)").GetComponent<CarController>();
        Assert.IsNotNull(_carController, "CarController not found");
        
        // 게임 제어 관련 버튼 확인
        _leftMoveButton = GameObject.Find("LeftMoveButton");
        Assert.IsNotNull(_leftMoveButton, "LeftMoveButton not found");
        _rightMoveButton = GameObject.Find("RightMoveButton");
        Assert.IsNotNull(_rightMoveButton, "RightMoveButton not found");
        
        // 가스의 스폰 위치 파악하기
        Vector3 leftPosition   = new Vector3(-1.0f, 0.2f, -3.0f);
        Vector3 rightPosition  = new Vector3(01.0f, 0.2f, -3.0f);
        Vector3 centerPosition = new Vector3(00.0f, 0.2f, -3.0f);
        
        float rayDistance = 10.0f;
        Vector3 rayDirection = Vector3.forward;
        
        // 플레이 시간
        float elapsedTime = 0.0f;
        float targetTime = 10.0f;
        
        // 반복
        while (gameManager.GameState == GameManager.State.Play)
        {
            RaycastHit hit;
            if (Physics.Raycast(leftPosition, rayDirection, out hit, rayDistance, 
                    LayerMask.GetMask("Gas")))
            {
                Debug.Log("left");
                MoveCar(hit.point);
            }
            else if (Physics.Raycast(rightPosition, rayDirection, out hit, rayDistance, 
                         LayerMask.GetMask("Gas")))
            {
                Debug.Log("right");
                MoveCar(hit.point);
            }
            else if (Physics.Raycast(centerPosition, rayDirection, out hit, rayDistance, 
                         LayerMask.GetMask("Gas")))
            {
                Debug.Log("center");
                MoveCar(hit.point);
            }
            else
            {
                Debug.Log("None");
                MoveButtonUp(_leftMoveButton);
                MoveButtonUp(_rightMoveButton);
            }
            
            Debug.DrawRay(leftPosition, rayDirection * rayDistance, Color.red);
            Debug.DrawRay(rightPosition, rayDirection * rayDistance, Color.green);
            Debug.DrawRay(centerPosition, rayDirection * rayDistance, Color.blue);
            
            // 시간 체크
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        if (elapsedTime < targetTime)
        {
            Assert.Fail("Game time is too short");
        }
        
        Time.timeScale = 1.0f;
    }

    private IEnumerator waitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }
    }

    /// <summary>
    /// 무브 버튼 다운
    /// </summary>
    /// <param name="moveButton"></param>
    private void MoveButtonDown(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerDownHandler);
    }
    
    /// <summary>
    /// 무브 버튼 업
    /// </summary>
    /// <param name="moveButton"></param>
    private void MoveButtonUp(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerUpHandler);
    }

    /// <summary>
    /// 플레이어 자동차 이름
    /// </summary>
    /// <param name="targetPosition"></param>
    private void MoveCar(Vector3 targetPosition)
    {
        if (Mathf.Abs(targetPosition.x - _carController.transform.position.x) < 0.01f)
        {
            MoveButtonUp(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
            return;
        }
        
        if (targetPosition.x < _carController.transform.position.x)
        {
            // 왼쪽으로 이동
            MoveButtonDown(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
        }
        else if (targetPosition.x > _carController.transform.position.x)
        {
            // 오른쪽으로 이동
            MoveButtonDown(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
        }
        else
        {
            MoveButtonUp(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
        }
    }
}
