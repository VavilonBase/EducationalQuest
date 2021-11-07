using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Progress : MonoBehaviour
{
    CsGlobals gl;

    private GameObject textPoints;
    private GameObject textRank;
    private GameObject textProgress;    

    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        textPoints = transform.Find("Points").gameObject;
        textRank = transform.Find("Rank").gameObject;
        textProgress = transform.Find("Progress").gameObject;               
    }

    void Update()
    {
        textPoints.GetComponent<TextMeshProUGUI>().text = "Набранные очки: " + gl.playerInfo.points;
        textProgress.GetComponent<TextMeshProUGUI>().text = "Прогресс: " + gl.playerInfo.NumRightAnswersTotal + " из ///" + 0;
        textRank.GetComponent<TextMeshProUGUI>().text = "Звание: " + gl.ranks[gl.playerInfo.cur_rank];
    }
}
