using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskBoardInformation
{
    private byte roomNumber;
    public byte RoomNumber { get { return roomNumber; } }
    private string roomTitle;
    private byte messageMode;
    public byte MessageMode { get { return messageMode; } set { messageMode = value; } }
    private bool onlyMistakesMode = false;
    public bool OnlyMistakesMode { get { return onlyMistakesMode; } set { onlyMistakesMode = value; } }

    //byte numberOfQuestions;

    string materialWelcome;
    //string[] materialBoardPaths;
    //string[,] materialQuestionsPaths;

    byte[] rightAnswers;
    public int NumberOfQuestions { get { return rightAnswers.Length; } }

    bool[] questionsRightAnswered;
    private byte numberOfCorrectAnswers;
    public byte NumberOfCorrectAnswers { get { return numberOfCorrectAnswers; } set { numberOfCorrectAnswers = value; } }
    private byte currentQuestion;
    public byte CurrentQuestion { get { return currentQuestion; } set { currentQuestion = value; } }
    bool keyWasGiven;
    public bool KeyWasGiven { get { return keyWasGiven; } set { keyWasGiven = value; } }


    public string MaterialBoardPath(byte qNum)
    {
        string path = roomTitle + "/Q" + (qNum + 1) + ".png";
        return path;
    }

    public string[] MaterialQuestionPaths(byte qNum)
    {
        string[] paths = new string[3];
        for (int i = 0; i < 3; i++)
        {
            paths[i] = roomTitle + "/Q" + (qNum + 1) + "/" + (i + 1) + ".png";
        }
        return paths;
    }

    public string MaterialResultPath()
    {
        string path = roomTitle + "/Res.png";
        return path;
    }

    public TaskBoardInformation(byte roomNum, string roomName, byte[] rightAnsw)
    {
        roomNumber = roomNum;
        roomTitle = roomName;
        messageMode = 0;
        materialWelcome = "W.png";
        rightAnswers = rightAnsw;
        questionsRightAnswered = new bool[rightAnswers.Length]; // по умолчанию false
        numberOfCorrectAnswers = 0;
        currentQuestion = 0;
        keyWasGiven = false;
    }

    public bool NextQuestion(bool isFirstQuestion, bool onlyMistakesMode, ref ObjectMaterials board, ref ObjectMaterials[] plates)
    {
        try
        {
            if (currentQuestion == NumberOfQuestions - 1) return false;
            switch (onlyMistakesMode)
            {
                case false:
                    if (!isFirstQuestion) currentQuestion++;
                    break;
                case true:
                    bool wa = true;
                    if (!isFirstQuestion) currentQuestion++;
                    while (wa && currentQuestion < NumberOfQuestions)
                    {
                        if (questionsRightAnswered[currentQuestion]) currentQuestion++;
                        else wa = false;                      
                    }
                    if (wa) return false;
                    break;
            }           
           
            board.SetTexture(MaterialBoardPath(currentQuestion));
            for (int i = 0; i < plates.Length; i++)
            {
                plates[i].SetTexture(MaterialQuestionPaths(currentQuestion)[i]);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    public bool CheckAnswer(byte answerNum, byte questionNum)
    {
        if (rightAnswers[questionNum] == answerNum)
        {
            questionsRightAnswered[questionNum] = true;
            return true;
        }
        else
        {
            questionsRightAnswered[questionNum] = false;
            return false;
        }
    }

    public byte CountRightAnswers()
    {
        byte count = 0;
        foreach (bool ans in questionsRightAnswered) if (ans) count++;
        return count;
    }

    public void Restart()
    {
        currentQuestion = 0;
        messageMode = 0;
        for (int i = 0; i < questionsRightAnswered.Length; i++) questionsRightAnswered[i] = false;
        numberOfCorrectAnswers = 0;
    }


    //add new question to the end of the array
    public bool AddQuestion(byte rightAnswerNum)
    {
        Array.Resize(ref rightAnswers, rightAnswers.Length + 1);
        rightAnswers[NumberOfQuestions - 1] = rightAnswerNum;
        return true;
    }

    //public byte NumberOfQuestions { get { return numberOfQuestions; } }
    //public string[] MaterialBoardPaths { get { return materialBoardPaths; } }
    public string MaterialWelcome { get { return materialWelcome; } }

}

public class ObjectMaterials
{
    Material frontMaterial;
    public Material FrontMaterial { get { return frontMaterial; } }
    Texture frontTexture;

    public ObjectMaterials()
    {
        frontMaterial = new Material(Shader.Find("Standard"));
    }

    public void SetTexture(string pathFromResources)
    {
        string fullPath = Application.dataPath + "/Resources/" + pathFromResources;
        WWW www = new WWW("file://" + fullPath);
        Texture _texture = www.texture;
        frontMaterial.mainTexture = _texture;
    }
}