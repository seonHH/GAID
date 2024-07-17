// 도형 소지 훈련의 Lv1 "별을 찾아줘"를 위한 스크립트
// 범위에 btn_star_yellow, btn_star_red, btn_star_blue가 위치 할 좌표를 난수로 생성한다( 배열로 저장한다 ).
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FindStar : MonoBehaviour
{
    // 초기 시간, 종료 시간을 저장 할 변수
    private int startTime;
    private float endTime;

    // 시도 횟수를 저장 할 변수
    private int tryCount = 0;

    // 찾은 별의 개수
    private int starCount = 0;

    // 각 오브젝트를 씬에서 찾아서 할당
    public GameObject btn_animal;
    public GameObject btn_star_yellow;
    public GameObject btn_star_red;
    public GameObject btn_star_blue;

    public GameObject msg_congrate;
    public GameObject msg_retry;

    public float offset = 10.0f;

    private Vector3[] starPos = new Vector3[3];

    public float xRandomRange = 4.4f;
    public float yRandomRange = 2f;

    // 진척도 계산 및 DB 저장을 위한 스크립트
    public GameObject ProgressScoreCalculate;

    // 유저 고유 진척도 저장 변수
    private int initialProgressScore;

    void Start()
    {
        // 시작 시간 설정
        startTime = int.Parse(DateTime.Now.ToString("HHmmss"));

        // 각 오브젝트를 씬에서 찾아서 할당
        // 오브젝트는 FindStarCanvas의 자식으로 들어가 있어용
        btn_animal = GameObject.Find("FindStarCanvas/btn_animal");
        btn_star_yellow = GameObject.Find("FindStarCanvas/btn_star_yellow");
        btn_star_red = GameObject.Find("FindStarCanvas/btn_star_red");
        btn_star_blue = GameObject.Find("FindStarCanvas/btn_star_blue");

        msg_congrate = GameObject.Find("FindStarCanvas/msg_congrate");
        msg_retry = GameObject.Find("FindStarCanvas/msg_retry");

        // msg_congrate, msg_retry는 비활성화 상태로 시작
        msg_congrate.SetActive(false);
        msg_retry.SetActive(false);

        RectTransform rect = btn_animal.GetComponent<RectTransform>();

        // 중심에서 범위 내에 랜덤 좌표를 생성하고, 배열에 저장한다.
        // 각 좌표의 범위를 변수에 저장 해놓는다.
        // xRandomRange = 4.4f;
        // yRandomRange = 2f;

        starPos[0] = new Vector3(UnityEngine.Random.Range(xRandomRange, -xRandomRange), UnityEngine.Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[1] = new Vector3(UnityEngine.Random.Range(xRandomRange, -xRandomRange), UnityEngine.Random.Range(yRandomRange, -yRandomRange), 0);
        starPos[2] = new Vector3(UnityEngine.Random.Range(xRandomRange, -xRandomRange), UnityEngine.Random.Range(yRandomRange, -yRandomRange), 0);

        RectTransform yellowRectTransform = btn_star_yellow.GetComponent<RectTransform>();
        RectTransform blueRectTransform = btn_star_blue.GetComponent<RectTransform>();
        RectTransform redRectTransform = btn_star_red.GetComponent<RectTransform>();

        // Now you can correctly access the rectTransform property
        yellowRectTransform.position = btn_animal.transform.position + starPos[0];
        blueRectTransform.position = btn_animal.transform.position + starPos[1];
        redRectTransform.position = btn_animal.transform.position + starPos[2];


        // 각 좌표들의 위치를 확인하기 위한 디버깅용 코드
        Debug.Log("yellow Star Position: " + yellowRectTransform.position);
        Debug.Log("Blue Star Position: " + blueRectTransform.position);
        Debug.Log("Red Star Position: " + redRectTransform.position);

    }

    // touch 된게 star면, congrate 메세지를 1초 동안 띄우고, 해당 별을 비활성화 시킨다.
    // touch 된게 animal이면, retry 메세지를 1초 동안  띄운다.
    public void btnClicked()
    {
        // 시도 횟수 증가
        tryCount++;

        // 클릭 된 오브젝트를 저장
        GameObject clkedObj = EventSystem.current.currentSelectedGameObject;
        Debug.Log("Clicked Object Name: " + clkedObj.name);

        if (clkedObj.name == "btn_star_yellow" || clkedObj.name == "btn_star_red" || clkedObj.name == "btn_star_blue")
        {
            msg_congrate.SetActive(true);
            StartCoroutine(ShowCongrate());
            clkedObj.SetActive(false);

            // 찾은 별의 개수 증가
            starCount++;

            // 모든 별을 찾았을 경우
            if (starCount == 3)
            {
                // 종료 시간 설정
                endTime = int.Parse(DateTime.Now.ToString("HHmmss"));

                // 진척도 계산 함수 호출
                // concentrationScore 아직 미구현
                // CalculateProgressScore("fg", 0, startTime, endTime, tryCount, concentrationScore );

                // 게임 종료 코드 추가
            }
        }
        else if (clkedObj.name == "btn_animal")
        {
            msg_retry.SetActive(true);
            StartCoroutine(ShowRetry());
        }
    }

    IEnumerator ShowCongrate()
    {
        yield return new WaitForSeconds(1.0f);
        msg_congrate.SetActive(false);
    }
    IEnumerator ShowRetry()
    {
        yield return new WaitForSeconds(1.0f);
        msg_retry.SetActive(false);
    }


    void Update()
    {
        // 터치가 들어오고, 클릭 된 오브젝트가 있으면, btnClicked 함수를 호출한다
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                btnClicked();
            }
        }
    }
}
