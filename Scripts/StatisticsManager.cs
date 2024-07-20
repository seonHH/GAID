using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;
using System;

// TODO
// 1. 선 그래프 패널 추가
// 

// start에서 데이터들 불러오고 저장 해둔다

public class StatisticsManager : MonoBehaviour
{
    // 씬의 오브젝트들을 저장할 변수
    // 각 패널들을 저장 할 변수
    private GameObject panel_statistics;
    private GameObject panel_record;
    private GameObject panel_section;
    private GameObject panel_progress;

    // 각 패널들의 버튼들을 저장 할 변수
    // StatisticsPanel의 오브젝트들
    private GameObject btn_game_record;
    private GameObject btn_section_stage;
    private GameObject btn_prog_stage;


    // RecordPanel의 오브젝트들
    private GameObject btn_record_back;
    private GameObject Content;
    private GameObject Content_template;

    // SectionPanel의 오브젝트들
    private GameObject btn_section_back;
    private GameObject btn_section_next;
    private GameObject btn_section_prev;
    private GameObject top_graph_vm;
    private GameObject top_graph_pc;
    private GameObject top_graph_sp;
    private GameObject top_graph_sr;
    private GameObject top_graph_fg;
    private int section_index = 0;

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
            { "time", 10 },
            { "attention", 98 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2024-06-23-12:00:00" },
            { "game", "sp" },
            { "lvl", 1 },
            { "prog", 200 },
            { "corr", 0.5 },
            { "time", 50 },
            { "attention", 50 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2022-07-01-12:00:00" },
            { "game", "sr" },
            { "lvl", 0 },
            { "prog", 100 },
            { "corr", 0.4 },
            { "time", 70 },
            { "attention", 40 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2021-06-01-12:00:00" },
            { "game", "fg" },
            { "lvl", 0 },
            { "prog", 100 },
            { "corr", 0.28 },
            { "time", 70 },
            { "attention", 20 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2024-06-23-12:00:00" },
            { "game", "vm" },
            { "lvl", 1 },
            { "prog", 200 },
            { "corr", 0.5 },
            { "time", 30 },
            { "attention", 90 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2024-06-24-15:00:00" },
            { "game", "vm" },
            { "lvl", 0 },
            { "prog", 100 },
            { "corr", 0.4 },
            { "time", 70 },
            { "attention", 40 }
        });
        gameRecords.Add(new Dictionary<string, object>
        {
            { "date", "2024-06-26-18:00:00" },
            { "game", "vm" },
            { "lvl", 1 },
            { "prog", 200 },
            { "corr", 0.5 },
            { "time", 50 },
            { "attention", 50 }
        });


        // 씬의 오브젝트들을 찾아서 변수에 저장
        panel_statistics = GameObject.Find("StatisticsCanvas/StatisticsPanel");
        panel_record = GameObject.Find("StatisticsCanvas/RecordPanel");
        panel_section = GameObject.Find("StatisticsCanvas/SectionPanel");
        panel_progress = GameObject.Find("StatisticsCanvas/ProgressPanel");

        btn_game_record = GameObject.Find("StatisticsCanvas/StatisticsPanel/btn_game_record");
        btn_section_stage = GameObject.Find("StatisticsCanvas/StatisticsPanel/btn_section_stage");
        btn_prog_stage = GameObject.Find("StatisticsCanvas/StatisticsPanel/btn_prog_stage");

        btn_record_back = GameObject.Find("StatisticsCanvas/RecordPanel/btn_back");
        btn_section_back = GameObject.Find("StatisticsCanvas/SectionPanel/btn_back");
        btn_prog_back = GameObject.Find("StatisticsCanvas/ProgressPanel/btn_back");

        Content_template = GameObject.Find("StatisticsCanvas/RecordPanel/Scroll View/Viewport/Content/GameData_template");


        btn_next = GameObject.Find("StatisticsCanvas/ProgressPanel/btn_next");
        btn_prev = GameObject.Find("StatisticsCanvas/ProgressPanel/btn_prev");

        // 각 패널 render
        RenderRecordPanel();
        RenderSectionPanel();
        RenderProgressPanel();

        // 각 패널들을 비활성화
        panel_record.SetActive(false);
        panel_section.SetActive(false);
        panel_progress.SetActive(false);

        // 각 패널들의 버튼들에 이벤트 추가
        btn_game_record.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel_record.SetActive(true);
            panel_statistics.SetActive(false);
        });
        btn_section_stage.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel_section.SetActive(true);
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
            // // content의 content_template을 제외한 모든 자식 오브젝트 삭제
            // foreach (Transform child in Content.transform)
            // {
            //     if (child.name != "GameData_template")
            //     {
            //         Destroy(child.gameObject);
            //     }
            // }
        });
        // section 패널의 뒤로가기 버튼
        btn_section_back.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel_section.SetActive(false);
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

        // content의 content_template을 제외한 모든 자식 오브젝트 삭제
        foreach (Transform child in Content.transform)
        {
            if (child.name != "GameData_template")
            {
                Destroy(child.gameObject);
            }
        }
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


    // SectionPanel 구성 함수

    // TODO
    // gameRecords의 데이터를 날짜, 게임별로 분류하여 저장
    // 날짜를 DateTime으로 변환하고, 날짜별로 정렬
    void RenderSectionPanel()
    {
        // btn_section_next, btn_section_prev 오브젝트를 찾아서 변수에 저장
        btn_section_next = GameObject.Find("StatisticsCanvas/SectionPanel/btn_next");
        btn_section_prev = GameObject.Find("StatisticsCanvas/SectionPanel/btn_prev");

        // graph_vm, graph_pc, graph_sp, graph_sr, graph_fg 오브젝트를 찾아서 변수에 저장
        GraphChart graph_vm = GameObject.Find("StatisticsCanvas/SectionPanel/graph_vm/graph_vm").GetComponent<GraphChart>();
        GraphChart graph_pc = GameObject.Find("StatisticsCanvas/SectionPanel/graph_pc/graph_pc").GetComponent<GraphChart>();
        GraphChart graph_sp = GameObject.Find("StatisticsCanvas/SectionPanel/graph_sp/graph_sp").GetComponent<GraphChart>();
        GraphChart graph_sr = GameObject.Find("StatisticsCanvas/SectionPanel/graph_sr/graph_sr").GetComponent<GraphChart>();
        GraphChart graph_fg = GameObject.Find("StatisticsCanvas/SectionPanel/graph_fg/graph_fg").GetComponent<GraphChart>();

        // 날짜 및 시간 형식 변경
        graph_vm.CustomDateTimeFormat = (date) => { return date.ToString("yyyy\nMM/dd"); };
        graph_pc.CustomDateTimeFormat = (date) => { return date.ToString("yyyy\nMM/dd"); };
        graph_sp.CustomDateTimeFormat = (date) => { return date.ToString("yyyy\nMM/dd"); };
        graph_sr.CustomDateTimeFormat = (date) => { return date.ToString("yyyy\nMM/dd"); };
        graph_fg.CustomDateTimeFormat = (date) => { return date.ToString("yyyy\nMM/dd"); };

        // 게임별로 gameRecords의 데이터를 저장할 배열
        List<Dictionary<string, object>> vmRecords = new List<Dictionary<string, object>>();
        List<Dictionary<string, object>> pcRecords = new List<Dictionary<string, object>>();
        List<Dictionary<string, object>> spRecords = new List<Dictionary<string, object>>();
        List<Dictionary<string, object>> srRecords = new List<Dictionary<string, object>>();
        List<Dictionary<string, object>> fgRecords = new List<Dictionary<string, object>>();

        // gameRecords의 데이터를 날짜, 게임별로 분류하여 저장
        // 날짜를 DateTime으로 변환하고, 날짜별로 정렬
        // 날짜별로 정렬
        gameRecords.Sort((a, b) => a["date"].ToString().CompareTo(b["date"].ToString()));


        // gameRecords의 길이만큼 반복
        for (int i = 0; i < gameRecords.Count; i++)
        {
            // gameRecords의 game 데이터를 확인하여 해당하는 인덱스에 데이터 저장
            switch (gameRecords[i]["game"].ToString())
            {
                case "vm":
                    vmRecords.Add(gameRecords[i]);
                    break;
                case "pc":
                    pcRecords.Add(gameRecords[i]);
                    break;
                case "sp":
                    spRecords.Add(gameRecords[i]);
                    break;
                case "sr":
                    srRecords.Add(gameRecords[i]);
                    break;
                case "fg":
                    fgRecords.Add(gameRecords[i]);
                    break;
            }
        }


        // graph_vm에 데이터 삽입
        if (graph_vm != null)
        {
            graph_vm.DataSource.StartBatch();
            graph_vm.DataSource.ClearCategory("Time");
            graph_vm.DataSource.ClearCategory("AttentionScore");
            graph_vm.DataSource.ClearCategory("ProgressScore");

            DateTime date;

            // vmRecords의 길이만큼 반복
            for (int i = 0; i < vmRecords.Count; i++)
            {
                // gameRecords의 데이터를 DateTime으로 변환
                TryParseCustomDateTime(vmRecords[i]["date"].ToString(), out date);

                // String 형태의 날짜를 DateTime으로 변환
                graph_vm.DataSource.AddPointToCategory("Time", date, float.Parse(vmRecords[i]["time"].ToString()));
                graph_vm.DataSource.AddPointToCategory("AttentionScore", date, float.Parse(vmRecords[i]["attention"].ToString()));
                graph_vm.DataSource.AddPointToCategory("ProgressScore", date, float.Parse(vmRecords[i]["prog"].ToString()));

            }

            graph_vm.DataSource.EndBatch();
        }

        // graph_pc에 데이터 삽입
        if (graph_pc != null)
        {
            graph_pc.DataSource.StartBatch();
            graph_pc.DataSource.ClearCategory("Accuracy");
            graph_pc.DataSource.ClearCategory("Time");
            graph_pc.DataSource.ClearCategory("AttentionScore");
            graph_pc.DataSource.ClearCategory("ProgressScore");

            DateTime date;

            // pcRecords의 길이만큼 반복
            for (int i = 0; i < pcRecords.Count; i++)
            {
                // gameRecords의 데이터를 DateTime으로 변환
                TryParseCustomDateTime(pcRecords[i]["date"].ToString(), out date);

                // String 형태의 날짜를 DateTime으로 변환
                graph_pc.DataSource.AddPointToCategory("Accuracy", date, float.Parse(pcRecords[i]["corr"].ToString()));
                graph_pc.DataSource.AddPointToCategory("Time", date, float.Parse(pcRecords[i]["time"].ToString()));
                graph_pc.DataSource.AddPointToCategory("AttentionScore", date, float.Parse(pcRecords[i]["attention"].ToString()));
                graph_pc.DataSource.AddPointToCategory("ProgressScore", date, float.Parse(pcRecords[i]["prog"].ToString()));

            }
        }

        // graph_sp에 데이터 삽입
        if (graph_sp != null)
        {
            graph_sp.DataSource.StartBatch();
            graph_sp.DataSource.ClearCategory("Accuracy");
            graph_sp.DataSource.ClearCategory("Time");
            graph_sp.DataSource.ClearCategory("AttentionScore");
            graph_sp.DataSource.ClearCategory("ProgressScore");

            DateTime date;

            // spRecords의 길이만큼 반복
            for (int i = 0; i < spRecords.Count; i++)
            {
                // gameRecords의 데이터를 DateTime으로 변환
                TryParseCustomDateTime(spRecords[i]["date"].ToString(), out date);

                // String 형태의 날짜를 DateTime으로 변환
                graph_sp.DataSource.AddPointToCategory("Accuracy", date, float.Parse(spRecords[i]["corr"].ToString()));
                graph_sp.DataSource.AddPointToCategory("Time", date, float.Parse(spRecords[i]["time"].ToString()));
                graph_sp.DataSource.AddPointToCategory("AttentionScore", date, float.Parse(spRecords[i]["attention"].ToString()));
                graph_sp.DataSource.AddPointToCategory("ProgressScore", date, float.Parse(spRecords[i]["prog"].ToString()));

            }
        }

        // graph_sr에 데이터 삽입
        if (graph_sr != null)
        {
            graph_sr.DataSource.StartBatch();
            graph_sr.DataSource.ClearCategory("Accuracy");
            graph_sr.DataSource.ClearCategory("Time");
            graph_sr.DataSource.ClearCategory("AttentionScore");
            graph_sr.DataSource.ClearCategory("ProgressScore");

            DateTime date;

            // srRecords의 길이만큼 반복
            for (int i = 0; i < srRecords.Count; i++)
            {
                // gameRecords의 데이터를 DateTime으로 변환
                TryParseCustomDateTime(srRecords[i]["date"].ToString(), out date);

                // String 형태의 날짜를 DateTime으로 변환
                graph_sr.DataSource.AddPointToCategory("Accuracy", date, float.Parse(srRecords[i]["corr"].ToString()));
                graph_sr.DataSource.AddPointToCategory("Time", date, float.Parse(srRecords[i]["time"].ToString()));
                graph_sr.DataSource.AddPointToCategory("AttentionScore", date, float.Parse(srRecords[i]["attention"].ToString()));
                graph_sr.DataSource.AddPointToCategory("ProgressScore", date, float.Parse(srRecords[i]["prog"].ToString()));

            }
        }

        // graph_fg에 데이터 삽입
        if (graph_fg != null)
        {
            graph_fg.DataSource.StartBatch();
            graph_fg.DataSource.ClearCategory("Accuracy");
            graph_fg.DataSource.ClearCategory("Time");
            graph_fg.DataSource.ClearCategory("AttentionScore");
            graph_fg.DataSource.ClearCategory("ProgressScore");

            DateTime date;

            // fgRecords의 길이만큼 반복
            for (int i = 0; i < fgRecords.Count; i++)
            {
                // gameRecords의 데이터를 DateTime으로 변환
                TryParseCustomDateTime(fgRecords[i]["date"].ToString(), out date);

                // String 형태의 날짜를 DateTime으로 변환
                graph_fg.DataSource.AddPointToCategory("Accuracy", date, float.Parse(fgRecords[i]["corr"].ToString()));
                graph_fg.DataSource.AddPointToCategory("Time", date, float.Parse(fgRecords[i]["time"].ToString()));
                graph_fg.DataSource.AddPointToCategory("AttentionScore", date, float.Parse(fgRecords[i]["attention"].ToString()));
                graph_fg.DataSource.AddPointToCategory("ProgressScore", date, float.Parse(fgRecords[i]["prog"].ToString()));

            }
        }

        // 초기에 graph_vm만 활성화
        // graph_pc, graph_sp, graph_sr, graph_fg는 비활성화
        top_graph_fg = GameObject.Find("StatisticsCanvas/SectionPanel/graph_fg");
        top_graph_pc = GameObject.Find("StatisticsCanvas/SectionPanel/graph_pc");
        top_graph_sp = GameObject.Find("StatisticsCanvas/SectionPanel/graph_sp");
        top_graph_sr = GameObject.Find("StatisticsCanvas/SectionPanel/graph_sr");
        top_graph_vm = GameObject.Find("StatisticsCanvas/SectionPanel/graph_vm");

        top_graph_vm.SetActive(true);
        top_graph_pc.SetActive(false);
        top_graph_sp.SetActive(false);
        top_graph_sr.SetActive(false);
        top_graph_fg.SetActive(false);

        // 각 버튼에 이벤트 추가
        btn_section_next.GetComponent<Button>().onClick.AddListener(() =>
        {
            section_index++;
            if (section_index > 4)
            {
                section_index = 0;
            }
            SectionGraphRender(section_index);
        });

        btn_section_prev.GetComponent<Button>().onClick.AddListener(() =>
        {
            section_index--;
            if (section_index < 0)
            {
                section_index = 4;
            }
            SectionGraphRender(section_index);
        });

    }

    void SectionGraphRender(int index)
    {
        switch (index)
        {
            case 0:
                top_graph_vm.SetActive(true);
                top_graph_pc.SetActive(false);
                top_graph_sp.SetActive(false);
                top_graph_sr.SetActive(false);
                top_graph_fg.SetActive(false);
                break;
            case 1:
                top_graph_vm.SetActive(false);
                top_graph_pc.SetActive(true);
                top_graph_sp.SetActive(false);
                top_graph_sr.SetActive(false);
                top_graph_fg.SetActive(false);
                break;
            case 2:
                top_graph_vm.SetActive(false);
                top_graph_pc.SetActive(false);
                top_graph_sp.SetActive(true);
                top_graph_sr.SetActive(false);
                top_graph_fg.SetActive(false);
                break;
            case 3:
                top_graph_vm.SetActive(false);
                top_graph_pc.SetActive(false);
                top_graph_sp.SetActive(false);
                top_graph_sr.SetActive(true);
                top_graph_fg.SetActive(false);
                break;
            case 4:
                top_graph_vm.SetActive(false);
                top_graph_pc.SetActive(false);
                top_graph_sp.SetActive(false);
                top_graph_sr.SetActive(false);
                top_graph_fg.SetActive(true);
                break;
        }
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
        // 0: vm, 1: pc, 2: fg, 3: sr, 4: sp
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
            graph_playtime.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SP", playtime[4]);

            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "VM", accuracy[0]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PC", accuracy[1]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "FG", accuracy[2]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SR", accuracy[3]);
            graph_accuracy.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SP", accuracy[4]);

            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "VM", progress[0]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "PC", progress[1]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "FG", progress[2]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SR", progress[3]);
            graph_progress.GetComponent<RadarChart>().DataSource.SetValue("Player 1", "SP", progress[4]);
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

    // String을 DateTime으로 변환하는 함수
    static bool TryParseCustomDateTime(string dateString, out DateTime dateTime)
    {
        dateTime = DateTime.MinValue; // 기본값 설정

        // 하이픈으로 날짜 부분과 시간 부분을 분리
        string[] dateParts = dateString.Split('-');
        if (dateParts.Length == 4) // 연도, 월, 일, 시간 부분이 올바른지 확인
        {
            int year = int.Parse(dateParts[0]); // 연도
            int month = int.Parse(dateParts[1]); // 월
            int day = int.Parse(dateParts[2]); // 일

            // 시간 부분을 콜론으로 분리
            string[] timeParts = dateParts[3].Split(':');
            if (timeParts.Length == 3) // 시, 분, 초 부분 확인
            {
                int hour = int.Parse(timeParts[0]); // 시
                int minute = int.Parse(timeParts[1]); // 분
                int second = int.Parse(timeParts[2]); // 초

                // DateTime 객체 생성
                dateTime = new DateTime(year, month, day, hour, minute, second);
                return true; // 변환 성공
            }
        }
        return false; // 변환 실패
    }
}




