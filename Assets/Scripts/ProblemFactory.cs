using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProblemFactory : IProblemFactory
{
    public Problem CreateProblem()
    {
        Problem problem = new Problem();

        problem.firstNumber = Random.Range(1, 50);
        problem.secondNumber = Random.Range(1, 50);
        problem.operation = (MathsOperation)Random.Range(0, 4);

        switch (problem.operation)
        {
            case MathsOperation.Addition:
                problem.correctAnswer = problem.firstNumber + problem.secondNumber;
                break;
            case MathsOperation.Subtraction:
                problem.correctAnswer = problem.firstNumber - problem.secondNumber;
                break;
            case MathsOperation.Multiplication:
                problem.correctAnswer = problem.firstNumber * problem.secondNumber;
                break;
            case MathsOperation.Division:
                problem.correctAnswer = problem.firstNumber / problem.secondNumber;
                problem.correctAnswer = Mathf.Round(problem.correctAnswer);
                if (problem.correctAnswer * problem.secondNumber != problem.firstNumber)
                {
                    problem.firstNumber = problem.correctAnswer * problem.secondNumber;
                }
                break;
        }

        problem.correctTube = Random.Range(0, 4);
        problem.answers = new float[4];
        problem.answers[problem.correctTube] = problem.correctAnswer;
        Debug.Log(problem.firstNumber + " " + problem.secondNumber + " " + problem.correctAnswer);
        List<int> randomCoefficients = new List<int> { -2, -1, 1, 2 };
        randomCoefficients = randomCoefficients.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < 4; i++)
        {
            if (i != problem.correctTube)
            {
                problem.answers[i] = problem.correctAnswer + randomCoefficients[i] * 10;
            }
        }

        return problem;
    }
    // Tạo câu hỏi đặc biệt với phép toán phức tạp
    public Problem CreateSpecialProblem()
    {
        Problem problem = new Problem();

        // Tạo các số ngẫu nhiên và phép toán phức tạp
        problem.firstNumber = Random.Range(1, 20);
        problem.secondNumber = Random.Range(1, 20);

        // Chọn phép toán ngẫu nhiên giữa cộng, nhân và lũy thừa
        int operationType = Random.Range(0, 3);

        switch (operationType)
        {
            case 0: // Cộng
                problem.operation = MathsOperation.Addition;
                problem.correctAnswer = problem.firstNumber + problem.secondNumber;
                break;
            case 1: // Nhân
                problem.operation = MathsOperation.Multiplication;
                problem.correctAnswer = problem.firstNumber * problem.secondNumber;
                break;
            case 2: // Lũy thừa
                problem.operation = MathsOperation.Power;
                problem.correctAnswer = Mathf.Pow(problem.firstNumber, problem.secondNumber);
                break;
        }

        // Đảm bảo câu trả lời đúng là một số nguyên nếu là phép lũy thừa
        problem.correctAnswer = Mathf.Round(problem.correctAnswer);

        problem.correctTube = Random.Range(0, 4);
        problem.answers = new float[4];
        problem.answers[problem.correctTube] = problem.correctAnswer;

        // Tạo các câu trả lời sai ngẫu nhiên
        List<int> randomCoefficients = new List<int> { -2, -1, 1, 2 };
        randomCoefficients = randomCoefficients.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < 4; i++)
        {
            if (i != problem.correctTube)
            {
                problem.answers[i] = problem.correctAnswer + randomCoefficients[i] * 10;
            }
        }

        return problem;
    }
}
