using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelperSharp;
using Swagger.Net.ResourceModels.Configuration.Operands;
using Swagger.Net.ResourceModels.Configuration.Operators;

namespace Swagger.Net.ResourceModels.Configuration
{
	/// <summary>
	/// Operand extensions.
	/// </summary>
    public static class OperandExtensions
    {
        #region HttpHeader
		/// <summary>
		/// Map if is equal to http header value specified name.
		/// </summary>
		/// <param name="leftOperand">Left operand.</param>
		/// <param name="httpHeaderName">Http header name.</param>
        public static ILogicOperator IsEqualToHttpHeader(this IOperand leftOperand, string httpHeaderName)
        {
            var op = new EqualsOperator(leftOperand, new HttpHeaderOperand(httpHeaderName));
            leftOperand.Operator = op;

            return op;
        }

		/// <summary>
		/// Map if is not equal to http header value specified name.
		/// </summary>
		/// <returns><c>true</c> if is not equal to http header the specified leftOperand httpHeaderName; otherwise, <c>false</c>.</returns>
		/// <param name="leftOperand">Left operand.</param>
		/// <param name="httpHeaderName">Http header name.</param>
        public static ILogicOperator IsNotEqualToHttpHeader(this IOperand leftOperand, string httpHeaderName)
        {
            return new NegationOperator(IsEqualToHttpHeader(leftOperand, httpHeaderName));
        }
        #endregion

        #region ContainsValue
		/// <summary>
		/// Map if contains the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="source">Source.</param>
		/// <param name="value">Value.</param>
        public static ILogicOperator ContainsValue(this IOperand source, string value)
        {
            var op = new ContainsOperator(source, new FixedValueOperand(value));
            source.Operator = op;

            return op;
        }

		/// <summary>
		/// Map if does not contain the value.
		/// </summary>
		/// <returns>The not contain value.</returns>
		/// <param name="source">Source.</param>
		/// <param name="value">Value.</param>
        public static ILogicOperator DoesNotContainValue(this IOperand source, string value)
        {
            return new NegationOperator(ContainsValue(source, value));
        }
        #endregion

        #region ContainsHttpHeader
		/// <summary>
		/// Map if contains the value of http header specified.
		/// </summary>
		/// <returns>The http header.</returns>
		/// <param name="source">Source.</param>
		/// <param name="httpHeaderName">Http header name.</param>
        public static ILogicOperator ContainsHttpHeader(this IOperand source, string httpHeaderName)
        {
            var op = new ContainsOperator(source, new HttpHeaderOperand(httpHeaderName));
            source.Operator = op;

            return op;
        }

		/// <summary>
		/// Map if does not contain the value of http header specified.
		/// </summary>
		/// <returns>The not contain http header.</returns>
		/// <param name="source">Source.</param>
		/// <param name="httpHeaderName">Http header name.</param>
        public static ILogicOperator DoesNotContainHttpHeader(this IOperand source, string httpHeaderName)
        {
            return new NegationOperator(ContainsHttpHeader(source, httpHeaderName));
        }
        #endregion

        #region Value
		/// <summary>
		/// Map if is equal to any of the values.
		/// </summary>
		/// <returns><c>true</c> if is equal to value the specified leftOperand values; otherwise, <c>false</c>.</returns>
		/// <param name="leftOperand">Left operand.</param>
		/// <param name="values">Values.</param>
        public static ILogicOperator IsEqualToValue(this IOperand leftOperand, params object[] values)
        {
            return CreateEqualsOperator(o => new EqualsOperator(o), "IsEqualToValue", leftOperand, values);
        }

		/// <summary>
		/// Map if is not equal to the values.
		/// </summary>
		/// <returns><c>true</c> if is not equal to value the specified leftOperand values; otherwise, <c>false</c>.</returns>
		/// <param name="leftOperand">Left operand.</param>
		/// <param name="values">Values.</param>
        public static ILogicOperator IsNotEqualToValue(this IOperand leftOperand, params object[] values)
        {
            return CreateEqualsOperator(o => { return new NegationOperator(new EqualsOperator(o)); }, "IsNotEqualToValue", leftOperand, values);
        }

		/// <summary>
		/// Creates the equals operator.
		/// </summary>
		/// <returns>The equals operator.</returns>
		/// <param name="createOperator">Create operator.</param>
		/// <param name="methodName">Method name.</param>
		/// <param name="leftOperand">Left operand.</param>
		/// <param name="values">Values.</param>
        private static ILogicOperator CreateEqualsOperator(Func<IOperand[], ILogicOperator> createOperator, string methodName, IOperand leftOperand, params object[] values)
        {
            IOperand[] operands;

            if (values == null)
            {
                operands = new IOperand[2];
                operands[1] = new FixedValueOperand(null);
            }
            else
            {
                if (values.Length < 1)
                {
                    throw new InvalidOperationException("An {0} need at least 1 value.".With(methodName));
                }

                operands = new IOperand[values.Length + 1];
               
                for (int i = 0; i < values.Length; i++)
                {
                    operands[i + 1] = new FixedValueOperand(values[i]);
                }

            }

            operands[0] = leftOperand;
            var op = createOperator(operands);
            leftOperand.Operator = op;

            return op;
        }
        #endregion
    }
}
