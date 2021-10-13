using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBoardWorking : MonoBehaviour
{
    private CsGlobals gl;
    public string roomName = "Mechanics";
    GameObject attachedFrontPlane;
    GameObject[] attachedAnswerPlates;
    GameObject[] attachedAnswerPlates_FrontPlates;

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

    public bool AttachNextQandA(bool start)
    {
        if (thisBoardInformation.NextQuestion(start, ref thisBoardObjectMaterials, ref thisPlatesObjectMaterials))
        {
            AttachMaterial(attachedFrontPlane, thisBoardObjectMaterials.FrontMaterial);            
            for (int i = 0; i < thisPlatesObjectMaterials.Length; i++)
            {
                AttachMaterial(attachedAnswerPlates_FrontPlates[i], thisPlatesObjectMaterials[i].FrontMaterial);                
            }
            return true;
        }
        else return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gl = FindObjectOfType(typeof(CsGlobals)) as CsGlobals;
        //поиск всех прикреплЄнных запчастей
        attachedFrontPlane = transform.Find("frontPlane").gameObject;
        attachedAnswerPlates = new GameObject[3];
        attachedAnswerPlates_FrontPlates = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            attachedAnswerPlates[i] = transform.Find("AnswerPlate" + (i + 1)).gameObject;
            attachedAnswerPlates[i].SetActive(false);
            attachedAnswerPlates_FrontPlates[i] = attachedAnswerPlates[i].transform.Find("FrontPlate").gameObject;
        }

        // далее будет обертка - проверка наличи€ сохранени€
        thisBoardInformation = new TaskBoardInformation(roomName);
        thisBoardObjectMaterials = new ObjectMaterials();
        thisPlatesObjectMaterials = new ObjectMaterials[3];
        for (int i = 0; i < 3; i++) thisPlatesObjectMaterials[i] = new ObjectMaterials();        

        thisBoardObjectMaterials.SetTexture(thisBoardInformation.MaterialWelcome);      

        AttachMaterial(attachedFrontPlane, thisBoardObjectMaterials.FrontMaterial);     
    }

    // Update is called once per frame
    void Update()
    {       
        // ¬заимодействие с доской с платформы
        if (isStandingOnPlatform)
        {
            if (thisBoardInformation.WelcomeMessageMode)
            {
                // запуск теста
                gl.textUI_pressF.SetActive(true); // --- можно заменить менюшкой из нескольких кнопок
                if (Input.GetKeyDown(KeyCode.F))
                {
                    thisBoardInformation.WelcomeMessageMode = false;
                    AttachNextQandA(true);
                    foreach (GameObject plate in attachedAnswerPlates) plate.SetActive(true);                    
                }
            }
            else
            {
                // предложение пропустить вопрос / начать сначала...
                gl.textUI_pressF.SetActive(true);                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    AttachNextQandA(false);                    
                }
            }
        }
        else
        {
            if (isAnswerUp && Input.GetKeyDown(KeyCode.F))
            {
                // «афиксировать, какой именно выбран ответ
                bool answerFound = false;
                byte answerNum = 0; byte i = 0;
                while (!answerFound)
                {
                    answerFound = attachedAnswerPlates[i].transform.Find("plateTrigger").GetComponent<UpBlock>().IsPlateUp;
                    if (answerFound) answerNum = i;
                    else i++;
                }

                // ѕроверить ответ              
                if (thisBoardInformation.CheckAnswer(answerNum, thisBoardInformation.CurrentQuestion))
                {
                    Debug.Log("Correct Answer!****%");
                }
                else Debug.Log("Wrong Answer!---!");

                AttachNextQandA(false);
            }
        }
        
    }
}
