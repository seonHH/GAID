using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;

// TODO
// 1. 통계 메인 화면에서 게임 기록 버튼 클릭 시 게임 기록 패널로 이동 - 완
// 2. Record 패널에 기록 된 게임 불러오기 - 완
// 3. Progress 패널에 스테이지별 진행도 표시

// start에서 데이터들 불러오고 저장 해둔다

public class StatisticsManager : MonoBehaviour
{
    // 씬의 오브젝트들을 저장할 변수
    // 각 패널들을 저장 할 변수
    private GameObject panel_statistics;
    private GameObject panel_record;
    private GameObject panel_progress;

    // 각 패널들의 버튼들을 저장 할 변수
    // StatisticsPanel의 오브젝트들
    private GameObject btn_game_record;
    private GameObject btn_prog_stage;

    // RecordPanel의 오브젝트들
    private GameObject btn_record_back;
    private GameObject Content;

    // ProgressPanel의 오브젝트들
    private GameObject btn_prog_back;
    private GameObject btn_next;
    private GameObject btn_prev;
    private GameObject graph_playtime;
    private GameObject graph_accuracy;
    private GameObject graph_progress;
    private int page_index = 0;

    // 로컬 JSON에서 불러온 데이터들을 저장할 배열
    private List<Dictionary<string, object>> gameRecords;



    void Start()
    {
        // 로컬 JSON으로 부터 게임 기록들 불러오기
        gameRecords = new List<Dictionary<string, object>>();
        // gameRecords = JsonManager.LoadJsonToList("gameRecords");

        // 디버깅용 gameRecords 생성
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2021-06-01-12:00:00" },
            { "game", "pc" },
            { "lvl", 0 },
            { "prog", 100 },
            { "corr", 0.98 },
            { "time", 10 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2024-06-23-12:00:00" },
            { "game", "ps" },
            { "lvl", 1 },
            { "prog", 200 },
            { "corr", 0.5 },
            { "time", 50 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2022-07-01-12:00:00" },
            { "game", "sr" },
            { "lvl", 0 },
            { "prog", 100 },
            { "corr", 0.4 },
            { "time", 70 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2021-06-01-12:00:00" },
            { "game", "fg" },
            { "lvl", 0 },
            { "prog", 100 },
            { "corr", 0.28 },
            { "time", 70 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2024-06-23-12:00:00" },
            { "game", "vm" },
            { "lvl", 1 },
            { "prog", 200 },
            { "corr", 0.5 },
            { "time", 30 }
        });


        // 씬의 오브젝트들을 찾아서 변수에 저장
        panel_statistics = GameObject.Find("StatisticsCanvas/StatisticsPanel");
        panel_record = GameObject.Find("StatisticsCanvas/RecordPanel");
        panel_progress = GameObject.Find("StatisticsCanvas/ProgressPanel");

        btn_game_record = GameObject.Find("StatisticsCanvas/StatisticsPanel/btn_game_record");
        btn_prog_stage = GameObject.Find("StatisticsCanvas/StatisticsPanel/btn_prog_stage");

        btn_record_back = GameObject.Find("StatisticsCanvas/RecordPanel/btn_back");

        btn_prog_back = GameObject.Find("StatisticsCanvas/ProgressPanel/btn_back");
        btn_next = GameObject.Find("StatisticsCanvas/ProgressPanel/btn_next");
        btn_prev = GameObject.Find("StatisticsCanvas/ProgressPanel/btn_prev");

        // record 패널 render
        RenderRecordPanel();
        RenderProgressPanel();

        // 각 패널들을 비활성화
        panel_record.SetActive(false);
        panel_progress.SetActive(false);

        // 각 패널들의 버튼들에 이벤트 추가
        btn_game_record.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel_record.SetActive(true);
            panel_statistics.SetActive(false);
        });
        btn_prog_stage.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel_progress.SetActive(true);
            panel_statistics.SetActive(false);
        });
        // record 패널의 뒤로가기 버튼
        btn_record_back.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel_record.SetActive(false);
            panel_statistics.SetActive(true);
        });
        // progress 패널의 뒤로가기 버튼
        btn_prog_back.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel_progress.SetActive(false);
            panel_statistics.SetActive(true);
        });



    }

    // RecordPanel 구성 함수
    // 1. 게임 기록을 불러와서 표시
    void RenderRecordPanel()
    {
        // Content 오브젝트를 찾아서 변수에 저장
        Content = GameObject.Find("StatisticsCanvas/RecordPanel/Scroll View/Viewport/Content");

        // 기존에 Content의 자식 객체로 있는 GameData_1을 탬플릿으로 다른 오브젝트들을 생성 예정
        GameObject Content_template = GameObject.Find("StatisticsCanvas/RecordPanel/Scroll View/Viewport/Content/GameData_template");

        // 이후 Content_template을 복제 해 새로운 오브젝트를 생성
        // 생성된 오브젝트의 이름은 GameData_1, GameData_2, ... 순으로 생성
        // 생성된 오브젝트의 Text 오브젝트에는 게임 기록을 표시
        // text_date_day : YYYY-mm-DD / text_date_second : hh-MM-ss 
        // text_others :  game / lvl / prog / corr / time
        // text_others: game + "/ Lv " + lvl + " / + " + prog + / " + corr + " % / " + time + "s"

        // gameRecords의 길이만큼 반복
        for (int i = 0; i < gameRecords.Count; i++)
        {
            // Content_template을 복제
            GameObject newContent = Instantiate(Content_template, Content.transform);

            // 복제된 오브젝트의 이름을 GameData_1, GameData_2, ... 순으로 변경
            newContent.name = "GameData_" + i;

            // gameRecords에서 데이터 불러와서 포맷에 맞게 변수에 저장
            // date 양식은 yyyy-MM-dd-HH:mm:ss이다
            string date_day = gameRecords[i]["date"].ToString().Substring(0, 10);
            string date_second = gameRecords[i]["date"].ToString().Substring(11, 8);

            // 복제된 오브젝트의 Text 오브젝트에 게임 기록 표시
            newContent.transform.Find("text_date_day").GetComponent<Text>().text = date_day;
            newContent.transform.Find("text_date_second").GetComponent<Text>().text = date_second;
            newContent.transform.Find("text_others").GetComponent<Text>().text = " " + gameRecords[i]["game"].ToString() + "  / Lv " + gameRecords[i]["lvl"].ToString() + " / " + gameRecords[i]["prog"].ToString() + " / " + gameRecords[i]["corr"].ToString() + " % / " + gameRecords[i]["time"].ToString() + "s";

        }

        // Content_template 비활성화
        Content_template.SetActive(false);
    }

    // ProgressPanel 구성 함수
    // 1. 영역별 스테이지별 진행도 표시
    void RenderProgressPanel()
    {
        // ProgressPanel의 오브젝트들을 찾아서 변수에 저장
        graph_playtime = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_playtime/graph_playtime");
        graph_accuracy = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_accuracy/graph_accuracy");
        graph_progress = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_progress_score/graph_progress_score");

        // gameRecords에 있는 time, corr, prog 데이터를 각 변수에 저장
        // 0: vm, 1: pc, 2: fg, 3: sr, 4: ps
        float[] playtime = new float[5];
        float[] accuracy = new float[5];
        float[] progress = new float[5];

        // 각 훈련의 개수를 저장 할 배열
        int[] gameCount = new int[5];

        // gameRecords의 길이만큼 반복
        for (int i = 0; i < gameRecords.Count; i++)
        {
            // gameRecords의 game 데이터를 확인하여 해당하는 인덱스에 데이터 저장
            switch (gameRecords[i]["game"].ToString())
            {
                case "vm":
                    playtime[0] += float.Parse(gameRecords[i]["time"].ToString());
                    accuracy[0] += float.Parse(gameRecords[i]["corr"].ToString());
                    progress[0] += float.Parse(gameRecords[i]["prog"].ToString());
                    gameCount[0]++;
                    break;
                case "pc":
                    playtime[1] += float.Parse(gameRecords[i]["time"].ToString());
                    accuracy[1] += float.Parse(gameRecords[i]["corr"].ToString());
                    progress[1] += float.Parse(gameRecords[i]["prog"].ToString());
                    gameCount[1]++;
                    break;
                case "fg":
                    playtime[2] += float.Parse(gameRecords[i]["time"].ToString());
                    accuracy[2] += float.Parse(gameRecords[i]["corr"].ToString());
                    progress[2] += float.Parse(gameRecords[i]["prog"].ToString());
                    gameCount[2]++;
                    break;
                case "sr":
                    playtime[3] += float.Parse(gameRecords[i]["time"].ToString());
                    accuracy[3] += float.Parse(gameRecords[i]["corr"].ToString());
                    progress[3] += float.Parse(gameRecords[i]["prog"].ToString());
                    gameCount[3]++;
                    break;
                case "ps":
                    playtime[4] += float.Parse(gameRecords[i]["time"].ToString());
                    accuracy[4] += float.Parse(gameRecords[i]["corr"].ToString());
                    progress[4] += float.Parse(gameRecords[i]["prog"].ToString());
                    gameCount[4]++;
                    break;
            }

        }

        // accuracy는 0~1 사이의 값이므로 각 값을 각 훈련의 개수로 나누어 평균을 구함
        // count가 0이 아닐 때만 나눈다
        for (int i = 0; i < 5; i++)
        {
            if (gameCount[i] != 0)
            {
                accuracy[i] /= gameCount[i];
            }
        }

        // 데이터에 맞게 각 그래프에 데이터 삽입
        if (graph_playtime != null && graph_accuracy != null && graph_progress != null)
        {
            graph_playtime.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "VM", playtime[0]);
            graph_playtime.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PC", playtime[1]);
            graph_playtime.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "FG", playtime[2]);
            graph_playtime.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SR", playtime[3]);
            graph_playtime.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PS", playtime[4]);

            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "VM", accuracy[0]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PC", accuracy[1]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "FG", accuracy[2]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SR", accuracy[3]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PS", accuracy[4]);

            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "VM", progress[0]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PC", progress[1]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "FG", progress[2]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SR", progress[3]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PS", progress[4]);
        }


        GameObject top_graph_playtime = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_playtime");
        GameObject top_graph_accuracy = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_accuracy");
        GameObject top_graph_progress = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_progress_score");

        // 초기에 playtime 그래프만 활성화
        top_graph_playtime.SetActive(true);
        top_graph_accuracy.SetActive(false);
        top_graph_progress.SetActive(false);

        // progress 패널의 이전, 다음 버튼
        btn_next.GetComponent<Button>().onClick.AddListener(() =>
        {
            page_index++;
            if (page_index > 2)
            {
                page_index = 0;
            }
            ProgressRadarGraphRender(page_index);
        });

        btn_prev.GetComponent<Button>().onClick.AddListener(() =>
        {
            page_index--;
            if (page_index < 0)
            {
                page_index = 2;
            }
            ProgressRadarGraphRender(page_index);
        });
    }

    void ProgressRadarGraphRender(int index)
    {
        GameObject top_graph_playtime = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_playtime");
        GameObject top_graph_accuracy = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_accuracy");
        GameObject top_graph_progress = GameObject.Find("StatisticsCanvas/ProgressPanel/graph_progress_score");

        switch (index)
        {
            case 0:
                top_graph_playtime.SetActive(true);
                top_graph_accuracy.SetActive(false);
                top_graph_progress.SetActive(false);
                break;
            case 1:
                top_graph_playtime.SetActive(false);
                top_graph_accuracy.SetActive(true);
                top_graph_progress.SetActive(false);
                break;
            case 2:
                top_graph_playtime.SetActive(false);
                top_graph_accuracy.SetActive(false);
                top_graph_progress.SetActive(true);
                break;
        }
    }

}



