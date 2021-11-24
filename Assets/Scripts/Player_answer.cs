using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_answer : MonoBehaviour
{
    CsGlobals gl;
    TaskBoardInformation board;

    public int DELAY = 1000;
    int INPUT = 0;

    private Material materialWelcome;
    public Material[] materialArray;
    
    public GameObject[] answers;
    byte stage_number = 0;
    public byte[] right_answers;

    private bool answer_up = false;
    private bool answer_false = false;

    private bool[] question_right_answered;
    private byte room_right_answers = 0;

    private Key attachedKey;
    // Start is called before the first frame update
    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;

        byte[] answ = new byte[10] { 1, 0, 0, 2, 1, 1, 0, 1, 2, 0 };
        board = new TaskBoardInformation(0, "Mechanics", answ);

        
        /*
        materialArray = new Material[board.numberOfQuestions];
        
        for (int i=0; i < materialArray.Length; i++)
        {
            Debug.Log("loading..." + board.MaterialBoardPaths[i]);
            materialArray[i] = Resources.Load<Material>(board.MaterialBoardPaths[i]);
        }
        */
        Material newMaterial = new Material(Shader.Find("Standard"));
        //Debug.Log(board.MaterialWelcome);

        string path = Application.dataPath + "/Resources/" + board.MaterialWelcome;
        Debug.Log(path);
        WWW www = new WWW("file://" + path);
        Texture _texture = www.texture;
        newMaterial.mainTexture = _texture;

        //GetComponent<Renderer>().material.mainTexture = _texture;

        //var _texture = Resources.Load<Texture2D>(board.MaterialWelcome);

        //newMaterial.SetTexture("_W", _texture);        
        gameObject.GetComponent<Renderer>().material.mainTexture = _texture;

        
        
        attachedKey = gl.key.GetComponent<Key>();
        question_right_answered = new bool[right_answers.Length];
        //gameObject.GetComponent<Renderer>().material = materialArray[stage_number];

        //for (int i=0; i<answers.Length; ++i)
            //answers[i].transform.Find("Answer").GetComponent<Renderer>().material = answers[i].transform.Find("trigger").GetComponent<UpBlock>().materialArray[stage_number];
    }

    // Update is called once per frame
    void Update()
    {
        if (INPUT > 0) INPUT--;

        // ----- проверка, что хотя бы один ответ поднят
        int i = 0; int k = 0;
        while (k==0 && i < 3)
        {
            if (answers[i].transform.Find("trigger").GetComponent<UpBlock>().isUp) k++;
            i++;
        }
        if (k == 0) answer_up = false; else answer_up = true;

        /*
        if (answer_up && INPUT == 0 && Input.GetKeyDown(KeyCode.F))
        {
            if (answers[right_answers[stage_number]].transform.Find("trigger").GetComponent<UpBlock>().isUp)
            {
                // ----- начисление очков
                if (!question_right_answered[stage_number])
                {
                    question_right_answered[stage_number] = true;
                    room_right_answers++;
                    gl.playerInfo.rightAnswersGivenCount++;
                    int del = 1;
                    if (answer_false) del = 2;
                    answer_false = false;
                    gl.playerInfo.points += 10/del; //резать количество очков, если взята помощь учителя
                    gl.playerInfo.cur_rank = Check_rank(gl.playerInfo.points);

                }
                // ----- выдача ключа
                if (room_right_answers == 7)
                {
                    Vector3 position = answers[1].transform.position;
                    position = new Vector3(position.x, position.y + Convert.ToSingle(0.5), position.z - 1);
                    attachedKey.ChangeKeyPosition(position);
                }
                // ----- смена вопросов
                if (stage_number == right_answers.Length - 1) stage_number = 0;
                else stage_number++;
                gameObject.GetComponent<Renderer>().material = materialArray[stage_number];
                for (int j = 0; j < answers.Length; ++j)
                    answers[j].transform.Find("Answer").GetComponent<Renderer>().material = answers[j].transform.Find("trigger").GetComponent<UpBlock>().materialArray[stage_number];
            }
            else
            {
                INPUT = DELAY; //задержка после неправильного ответа
                answer_false = true;
            }
        

        }  
        */
    }

    private byte Check_rank(int points)
    {
        if (points < 10) return 0;
        if (points < 30) return 1;        
        if (points >= 30 + 40 * (gl.playerInfo.cur_rank - 1)) return (byte)(gl.playerInfo.cur_rank + 1);
        else return gl.playerInfo.cur_rank;
    }
}
