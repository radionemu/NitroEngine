using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//부스터에 관련한 스크립트입니다.

public class BoosterManager : MonoBehaviour
{
    [Header ("Status")]
    //최대 부스터 개수
    public int booster_maxnum;


    //큐로 만들어 관리한다
    //1 : 일반 부스터
    //2 : 팀 부스터

    [Header ("UI")]
    public Texture N2O_normal;
    public Texture N2O_team;

    public List<GameObject> boosters;
    public Queue<string> boosterqueue;

    public GameObject Grid;
    public GameObject Prefab;

    // Start is called before the first frame update
    private void Start()
    {
        boosters = new List<GameObject>();
        boosterqueue = new Queue<string>();
        for(int i =0; i<booster_maxnum; i++){
            GameObject b = Instantiate(Prefab) as GameObject;
            //0번 인덱스일 경우, b의 스케일 1.25배
            if(i == 0){
                b.transform.localScale = new Vector3(1.25f,1.25f,0f);
            }
            b.transform.SetParent(Grid.transform, false);
            boosters.Add(b);
        }
        RefreshFrame();
    }

    public void AddQueue(){
        //부스터 큐가 꽉 찬 경우 패스
        if(boosterqueue.Count == booster_maxnum)return;

        //그렇지 않을 경우
        //정수 큐에 추가
        boosterqueue.Enqueue("normal");
        //이후 UI에 변화 요청
        RefreshFrame();
    }

    public void DeQueue(){
        if(boosterqueue.Count == 0) return;
        boosterqueue.Dequeue();
        RefreshFrame();
    }

    public void RefreshFrame(){
        //function that synchronize UI and boosterqueue
        
        //부스터 오브젝트 리스트에 각각 큐의 값을 대입
        //널이면 둘다 지우고 1이면 노멀 부스터, 2면 팀부스터

        string [] temp = boosterqueue.ToArray();

        for(int i=0; i<booster_maxnum; i++){
            RawImage rawImage = boosters[i].transform.Find("Item").GetComponentInChildren<RawImage>();
            if(i>=temp.Length || temp[i] == null){
                rawImage.texture = null;
                rawImage.color = Color.clear;
            }else{
                if(temp[i] == "normal"){
                    rawImage.texture = N2O_normal;
                    rawImage.color = Color.white;
                }
            }    
        }
    }

    // 0 = NULL
    // 1 = NormalBooster
    public int getBoosterQueueFront(){
        if(boosterqueue.Count == 0) return 0;
        if(boosterqueue.Peek() == "normal") return 1;
        else return 0;
    }
}