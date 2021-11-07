using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskBoardWorking : MonoBehaviour
{
    private CsGlobals gl;
    public byte boardNum;
    GameObject attachedFrontPlane;
    GameObject[] attachedAnswerPlates;
    GameObject[] attachedAnswerPlates_FrontPlates;
    GameObject attachedBoardText;
    private Key attachedKey;

    TaskBoardInformation thisBoardInformation;
    ObjectMaterials thisBoardObjectMaterials;
    ObjectMaterials[] thisPlatesObjectMaterials;

    private bool isStandingOnPlatform = false;
    private bool isAnswerUp = false;
    
    public bool IsStandingOnPlatform { get { return isStandingOnPlatform; } set { isStandingOnPlatform = value; } }
    public bool IsAnswerUp { get { return isAnswerUp; } set { isAnswerUp = value; } }

    public void AttachMaterial(GameObject obj, Material mat)
    {
        obj.GetComponent<Renderer>().material = mat;
    }

    public void WriteOnBoard(string text)
    {
        thisBoardObjectMaterials.SetTexture(thisBoardInformation.MaterialWelcome);
        AttachMaterial(attachedFrontPlane, thisBoardObjectMaterials.FrontMaterial);
        attachedBoardText.transform.GetComponent<TextMeshPro>().text = text;
        attachedBoardText.SetActive(true);
    }

    public void CleanBoard()
    {
        attachedBoardText.SetActive(false);
    }

    public bool AttachNextQandA(bool start)
    {
        if (thisBoardInformation.NextQuestion(start, thisBoardInformation.OnlyMistakesMode, ref thisBoardObjectMaterials, ref thisPlatesObjectMaterials))
        {
            AttachMaterial(attachedFrontPlane, thisBoardObjectMaterials.FrontMaterial);            
            for (int i = 0; i < thisPlatesObjectMaterials.Length; i++)
            {
                AttachMaterial(attachedAnswerPlates_FrontPlates[i], thisPlatesObjectMaterials[i].FrontMaterial);                
            }
            return true;
        }
        else
        {
            thisBoardInformation.MessageMode = 2;
            foreach (GameObject plate in attachedAnswerPlates) { plate.SetActive(false); }            

            thisBoardInformation.NumberOfCorrectAnswers = thisBoardInformation.CountRightAnswers();
            gl.playerInfo.SetNumOfRightAnswers(thisBoardInformation.RoomNumber, thisBoardInformation.NumberOfCorrectAnswers);

            if (!thisBoardInformation.KeyWasGiven && thisBoardInformation.NumberOfCorrectAnswers >= thisBoardInformation.NumberOfQuestions * 0.5)
            {
                thisBoardInformation.KeyWasGiven = true;
                Vector3 bP = this.transform.position;
                bP.z += 1;
                attachedKey.ChangeKeyPosition(bP);
            }

            //------------- form result
            WriteOnBoard("������ �������: "+ thisBoardInformation.NumberOfCorrectAnswers + " �� " + thisBoardInformation.NumberOfQuestions + ".\n������ �� ���������, ����� ���������.");
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        attachedKey = gl.key.GetComponent<Key>();

        attachedFrontPlane = transform.Find("frontPlane").gameObject;
        attachedAnswerPlates = new GameObject[3];
        attachedAnswerPlates_FrontPlates = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            attachedAnswerPlates[i] = transform.Find("AnswerPlate" + (i + 1)).gameObject;
            attachedAnswerPlates[i].SetActive(false);
            attachedAnswerPlates_FrontPlates[i] = attachedAnswerPlates[i].transform.Find("FrontPlate").gameObject;
        }
        attachedBoardText = transform.Find("BoardText").gameObject;

        thisBoardInformation = gl.boardsInfo[boardNum];

        thisBoardObjectMaterials = new ObjectMaterials();
        thisPlatesObjectMaterials = new ObjectMaterials[3];
        for (int i = 0; i < 3; i++) thisPlatesObjectMaterials[i] = new ObjectMaterials();

        WriteOnBoard("����� ����������!\n������ �� ���������, ����� ������!");
    }

    // Update is called once per frame
    void Update()
    {      
        if (isStandingOnPlatform)
        {
            switch (thisBoardInformation.MessageMode)
            {
                // Welcome message
                case 0:
                    gl.PrintLabel("����� F, ����� ������ ����");
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        CleanBoard();
                        thisBoardInformation.MessageMode = 1;
                        AttachNextQandA(true);
                        foreach (GameObject plate in attachedAnswerPlates) plate.SetActive(true);
                    }
                    break;
                // Answering mode
                case 1:
                    gl.PrintLabel("����� F, ����� ���������� ������");
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        AttachNextQandA(false);
                    }
                    break;
                // Test completed mode
                case 2:
                    gl.PrintLabel("F - �������� �������, Q - �������� �� ������� � ��������");
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        WriteOnBoard("������ �������: 0.");
                        thisBoardInformation.OnlyMistakesMode = false;
                        thisBoardInformation.Restart();
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        CleanBoard();
                        thisBoardInformation.OnlyMistakesMode = true;
                        thisBoardInformation.MessageMode = 1;
                        thisBoardInformation.CurrentQuestion = 0;
                        if (AttachNextQandA(true))
                            foreach (GameObject plate in attachedAnswerPlates) plate.SetActive(true);
                        else
                            WriteOnBoard("������ �� ��� ������� ���� �����!");
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (isAnswerUp && Input.GetKeyDown(KeyCode.F))
            {                
                bool answerFound = false;
                byte answerNum = 0; byte i = 0;
                while (!answerFound)
                {
                    answerFound = attachedAnswerPlates[i].transform.Find("plateTrigger").GetComponent<UpBlock>().IsPlateUp;
                    if (answerFound) answerNum = i;
                    else i++;
                }
                         
                if (thisBoardInformation.CheckAnswer(answerNum, thisBoardInformation.CurrentQuestion))
                {
                    Debug.Log("Correct Answer!");
                }
                else Debug.Log("Wrong Answer!");

                AttachNextQandA(false);
            }
        }
        
    }
}
