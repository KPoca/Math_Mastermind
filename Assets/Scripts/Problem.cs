//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Patch 2.0
[System.Serializable]
public class Problem
{
    public float firstNumber;           // first number in the problem
    public float secondNumber;          // second number in the problem
    public MathsOperation operation;    // operator between the two numbers
    public float correctAnswer;
    public float[] answers;             // array of all possible answers including the correct one

    [Range(0, 3)]
    public int correctTube;             // index of the correct tube
    //Patch 2.0
    public Problem()
    {
        //firstNumber = Random.Range(1, 50);
        //secondNumber = Random.Range(1, 50);
        //operation = (MathsOperation)Random.Range(0, 4);
        //switch (operation)
        //{
        //    case MathsOperation.Addition:
        //        correctAnswer = firstNumber + secondNumber;
        //        break;
        //    case MathsOperation.Subtraction:
        //        correctAnswer = firstNumber - secondNumber;
        //        break;
        //    case MathsOperation.Multiplication:  // Nhân
        //        correctAnswer = firstNumber * secondNumber;
        //        break;
        //    case MathsOperation.Division: 
        //        correctAnswer = firstNumber / secondNumber;  // Đảm bảo chia hết
        //        correctAnswer = Mathf.Round(correctAnswer);
        //        if (correctAnswer * secondNumber != firstNumber)
        //        {
        //            firstNumber = correctAnswer * secondNumber;
        //        }
        //        break;
        //}
        //Debug.Log(firstNumber + " " + secondNumber + " " + correctAnswer);
        //correctTube = Random.Range(0, 4);
        //answers = new float[4];
        //answers[correctTube] = correctAnswer;

        //List<int> randomCoefficients = new List<int> { -2, -1, 1, 2 };
        //randomCoefficients = randomCoefficients.OrderBy(x => Random.value).ToList();  // Trộn danh sách ngẫu nhiên

        //for (int i = 0; i < 4; i++)
        //{
        //    if (i != correctTube)
        //    {
        //        answers[i] = correctAnswer + randomCoefficients[i] * 10;
        //    }
        //}
    }
}
// MathsOperation.cs
public enum MathsOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Power
}