using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Create bool expression tree.
    /// Anderstand breakets, 'and', 'or' operations.
    /// And equality opeators.
    /// </summary>
    public static class Parser
    {
        private enum LetterType
        {
            Word,
            Operator,
            Space,
            Unknown,
            Sentence,
        }

        /// <summary>
        /// Build expretion tree by string.
        /// </summary>
        /// <param name="str">Expretion.</param>
        /// <returns>Tree.</returns>
        internal static IExpressionBoolOperator Parse(string str)
        {
            var list = Split(str);
            IExpressionBoolOperator root;
            int index = 0;
            root = GetExpression(list, ref index, null);

            return root;
        }

        private static IExpressionBoolOperator GetExpression(List<string> words, ref int index, IExpressionBoolOperator root)
        {
            if (index >= words.Count)
            {
                return root;
            }

            switch (words[index])
            {
                case "(":
                    index++;
                    return GetExpression(words, ref index, null);

                case ")":
                    index++;
                    return root;
                case "and":
                    {
                        var andOperator = new AndOperator();
                        index++;
                        var temp = GetExpression(words, ref index, andOperator);
                        if (root is OrOperator)
                        {
                            var castedRoot = root as IExpressionBoolOperation;
                            andOperator.OperandA = castedRoot.OperandB;
                            andOperator.OperandB = temp;
                            castedRoot.OperandB = andOperator;
                            return GetExpression(words, ref index, root);
                        }

                        andOperator.OperandA = root;
                        andOperator.OperandB = temp;
                        return GetExpression(words, ref index, andOperator);
                    }

                case "or":
                    {
                        var orOperator = new OrOperator();
                        index++;
                        orOperator.OperandA = root;
                        orOperator.OperandB = GetExpression(words, ref index, orOperator);

                        return GetExpression(words, ref index, orOperator);
                    }

                default:
                    {
                        var operation = GetOperation(words, ref index, GetElement(words, ref index));
                        if (root is null)
                        {
                            return GetExpression(words, ref index, operation);
                        }

                        return operation;
                    }
            }
        }

        private static IExpressionBoolOperator GetOperation(List<string> words, ref int index, ExpressionElement leftOperand)
        {
            if (leftOperand is null)
            {
                leftOperand = GetElement(words, ref index);
            }

            switch (words[index])
            {
                case "=":
                case "==":
                    {
                        index++;
                        return new EqualOperator(leftOperand, GetElement(words, ref index));
                    }

                case "<":
                    {
                        index++;
                        return new LessOperator(leftOperand, GetElement(words, ref index));
                    }

                case ">":
                    {
                        index++;
                        return new BiggerOperator(leftOperand, GetElement(words, ref index));
                    }

                case "!=":
                    {
                        index++;
                        return new NotEqualOperator(leftOperand, GetElement(words, ref index));
                    }

                case "<=":
                    {
                        index++;
                        return new LessOrEqualOperator(leftOperand, GetElement(words, ref index));
                    }

                case ">=":
                    {
                        index++;
                        return new BiggerOrEqualOperator(leftOperand, GetElement(words, ref index));
                    }

                default:
                    throw new ArgumentException("Bad operator");
            }
        }

        private static ExpressionElement GetElement(List<string> words, ref int index)
        {
            var result = new ExpressionElement(words[index]);
            index++;
            return result;
        }

        /// <summary>
        /// Split section to tokens.
        /// </summary>
        /// <param name="str">String.</param>
        /// <returns>String tokens.</returns>
        private static List<string> Split(string str)
        {
            List<string> words = new ();
            StringBuilder currentWord = new ();
            string symbols = "()<>=!";
            string numberSymbols = ".,";
            LetterType previousType = LetterType.Unknown;
            foreach (var letter in str)
            {
                LetterType currentType = LetterType.Unknown;
                if (previousType == LetterType.Sentence)
                {
                    if (letter != '\'')
                    {
                        currentType = LetterType.Sentence;
                    }
                    else
                    {
                        currentType = LetterType.Unknown;
                        currentWord.Append('\'');
                    }
                }
                else if (char.IsLetterOrDigit(letter) || numberSymbols.Contains(letter))
                {
                    currentType = LetterType.Word;
                }
                else if (symbols.Contains(letter))
                {
                    currentType = LetterType.Operator;
                }
                else if (letter == ' ' && previousType != LetterType.Sentence)
                {
                    currentType = LetterType.Space;
                }
                else if (letter == '\'')
                {
                    currentType = LetterType.Sentence;
                }

                if (currentType == previousType)
                {
                    currentWord.Append(letter);
                }
                else
                {
                    if (previousType != LetterType.Space && previousType != LetterType.Unknown)
                    {
                        words.Add(currentWord.ToString());
                    }

                    currentWord = new StringBuilder($"{letter}");
                    previousType = currentType;
                }
            }

            if (previousType != LetterType.Space && previousType != LetterType.Unknown)
            {
                words.Add(currentWord.ToString());
            }

            return words;
        }
    }
}
