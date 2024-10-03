//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

// Parse conditional expressions from CFXR_MaterialInspector to show/hide some parts of the UI easily

namespace CartoonFX
{
    public static class ExpressionParser
    {
        #region Public Nested Types

        public delegate bool EvaluateFunction(string content);

        //--------------------------------------------------------------------------------------------------------------------------------
        // Expression Token

        public class Token
        {
            #region Public Nested Types

            public enum TokenType
            {
                OPEN_PAREN,
                CLOSE_PAREN,
                UNARY_OP,
                BINARY_OP,
                LITERAL,
                EXPR_END,
            }

            #endregion

            #region Variables

            private static readonly Dictionary<char, KeyValuePair<TokenType, string>> typesDict = new()
            {
                { '(', new KeyValuePair<TokenType, string>(TokenType.OPEN_PAREN, "(") },
                { ')', new KeyValuePair<TokenType, string>(TokenType.CLOSE_PAREN, ")") },
                { '!', new KeyValuePair<TokenType, string>(TokenType.UNARY_OP, "NOT") },
                { '&', new KeyValuePair<TokenType, string>(TokenType.BINARY_OP, "AND") },
                { '|', new KeyValuePair<TokenType, string>(TokenType.BINARY_OP, "OR") },
            };

            public TokenType type;
            public string value;

            #endregion

            #region Setup/Teardown

            public Token(StringReader s)
            {
                int c = s.Read();
                if (c == -1)
                {
                    type = TokenType.EXPR_END;
                    value = "";
                    return;
                }

                var ch = (char)c;

                //Special case: solve bug where !COND_FALSE_1 && COND_FALSE_2 would return True
                bool embeddedNot = ch == '!' && s.Peek() != '(';

                if (typesDict.ContainsKey(ch) && !embeddedNot)
                {
                    type = typesDict[ch].Key;
                    value = typesDict[ch].Value;
                }
                else
                {
                    var str = "";
                    str += ch;
                    while (s.Peek() != -1 && !typesDict.ContainsKey((char)s.Peek()))
                    {
                        str += (char)s.Read();
                    }

                    type = TokenType.LITERAL;
                    value = str;
                }
            }

            #endregion

            #region Public methods

            public static List<Token> TransformToPolishNotation(List<Token> infixTokenList)
            {
                var outputQueue = new Queue<Token>();
                var stack = new Stack<Token>();

                var index = 0;
                while (infixTokenList.Count > index)
                {
                    Token t = infixTokenList[index];

                    switch (t.type)
                    {
                        case TokenType.LITERAL:
                            outputQueue.Enqueue(t);
                            break;
                        case TokenType.BINARY_OP:
                        case TokenType.UNARY_OP:
                        case TokenType.OPEN_PAREN:
                            stack.Push(t);
                            break;
                        case TokenType.CLOSE_PAREN:
                            while (stack.Peek().type != TokenType.OPEN_PAREN)
                            {
                                outputQueue.Enqueue(stack.Pop());
                            }

                            stack.Pop();
                            if (stack.Count > 0 && stack.Peek().type == TokenType.UNARY_OP)
                            {
                                outputQueue.Enqueue(stack.Pop());
                            }

                            break;
                    }

                    index++;
                }

                while (stack.Count > 0)
                {
                    outputQueue.Enqueue(stack.Pop());
                }

                var list = new List<Token>(outputQueue);
                list.Reverse();
                return list;
            }

            #endregion
        }

        //--------------------------------------------------------------------------------------------------------------------------------
        // Boolean Expression Classes

        public abstract class Expression
        {
            #region Public methods

            public abstract bool Evaluate();

            #endregion
        }

        public class ExpressionLeaf : Expression
        {
            #region Variables

            private readonly string content;
            private readonly EvaluateFunction evalFunction;

            #endregion

            #region Setup/Teardown

            public ExpressionLeaf(EvaluateFunction _evalFunction, string _content)
            {
                evalFunction = _evalFunction;
                content = _content;
            }

            #endregion

            #region Public methods

            public override bool Evaluate()
            {
                //embedded not, see special case in Token declaration
                if (content.StartsWith("!"))
                {
                    return !evalFunction(content.Substring(1));
                }

                return evalFunction(content);
            }

            #endregion
        }

        public class ExpressionAnd : Expression
        {
            #region Variables

            private readonly Expression left;
            private readonly Expression right;

            #endregion

            #region Setup/Teardown

            public ExpressionAnd(Expression _left, Expression _right)
            {
                left = _left;
                right = _right;
            }

            #endregion

            #region Public methods

            public override bool Evaluate()
            {
                return left.Evaluate() && right.Evaluate();
            }

            #endregion
        }

        public class ExpressionOr : Expression
        {
            #region Variables

            private readonly Expression left;
            private readonly Expression right;

            #endregion

            #region Setup/Teardown

            public ExpressionOr(Expression _left, Expression _right)
            {
                left = _left;
                right = _right;
            }

            #endregion

            #region Public methods

            public override bool Evaluate()
            {
                return left.Evaluate() || right.Evaluate();
            }

            #endregion
        }

        public class ExpressionNot : Expression
        {
            #region Variables

            private readonly Expression expr;

            #endregion

            #region Setup/Teardown

            public ExpressionNot(Expression _expr)
            {
                expr = _expr;
            }

            #endregion

            #region Public methods

            public override bool Evaluate()
            {
                return !expr.Evaluate();
            }

            #endregion
        }

        #endregion

        #region Public methods

        //--------------------------------------------------------------------------------------------------------------------------------
        // Main Function to use

        public static bool EvaluateExpression(string expression, EvaluateFunction evalFunction)
        {
            //Remove white spaces and double && ||
            var cleanExpr = "";
            for (var i = 0; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case ' ': break;
                    case '&':
                        cleanExpr += expression[i];
                        i++;
                        break;
                    case '|':
                        cleanExpr += expression[i];
                        i++;
                        break;
                    default:
                        cleanExpr += expression[i];
                        break;
                }
            }

            var tokens = new List<Token>();
            var reader = new StringReader(cleanExpr);
            Token t = null;
            do
            {
                t = new Token(reader);
                tokens.Add(t);
            }
            while (t.type != Token.TokenType.EXPR_END);

            List<Token> polishNotation = Token.TransformToPolishNotation(tokens);

            List<Token>.Enumerator enumerator = polishNotation.GetEnumerator();
            enumerator.MoveNext();
            Expression root = MakeExpression(ref enumerator, evalFunction);

            return root.Evaluate();
        }

        public static Expression MakeExpression(ref List<Token>.Enumerator polishNotationTokensEnumerator,
            EvaluateFunction _evalFunction)
        {
            if (polishNotationTokensEnumerator.Current.type == Token.TokenType.LITERAL)
            {
                Expression lit = new ExpressionLeaf(_evalFunction, polishNotationTokensEnumerator.Current.value);
                polishNotationTokensEnumerator.MoveNext();
                return lit;
            }

            if (polishNotationTokensEnumerator.Current.value == "NOT")
            {
                polishNotationTokensEnumerator.MoveNext();
                Expression operand = MakeExpression(ref polishNotationTokensEnumerator, _evalFunction);
                return new ExpressionNot(operand);
            }

            if (polishNotationTokensEnumerator.Current.value == "AND")
            {
                polishNotationTokensEnumerator.MoveNext();
                Expression left = MakeExpression(ref polishNotationTokensEnumerator, _evalFunction);
                Expression right = MakeExpression(ref polishNotationTokensEnumerator, _evalFunction);
                return new ExpressionAnd(left, right);
            }

            if (polishNotationTokensEnumerator.Current.value == "OR")
            {
                polishNotationTokensEnumerator.MoveNext();
                Expression left = MakeExpression(ref polishNotationTokensEnumerator, _evalFunction);
                Expression right = MakeExpression(ref polishNotationTokensEnumerator, _evalFunction);
                return new ExpressionOr(left, right);
            }

            return null;
        }

        #endregion
    }
}