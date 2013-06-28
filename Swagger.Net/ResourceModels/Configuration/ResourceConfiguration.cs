using System;
using System.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Collections;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Net.Http;
using System.Diagnostics;
using Swagger.Net.ResourceModels.Configuration.Operands;

namespace Swagger.Net.ResourceModels.Configuration
{
    /// <summary>
    /// Represents a configuration for an API resource.
    /// </summary>
    [DebuggerDisplay("{ControllerType} {CanShow}")]
    public abstract class ResourceConfiguration<TController> : ConfigurationBase, IResourceConfiguration
		where TController : class
	{
		#region Fields
		private List<OperationConfiguration> m_operationsMap = new List<OperationConfiguration>();
		#endregion

		#region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfiguration{TController}"/> class.
        /// </summary>
        protected ResourceConfiguration()
        {
        }
		#endregion

		#region Properties
        /// <summary>
        /// Gets the type of the controller.
        /// </summary>
        Type IResourceConfiguration.ControllerType
        {
            get
            {
                return typeof(TController);
            }
        }

        /// <summary>
        /// A configuration for all operations on current resource that do not have a specific configuration..
        /// </summary>
        /// <returns></returns>
        public OperationConfiguration AllOperations
        {
            get
            {
                var operation = GetAllOperationConfig();

                if (operation == null)
                {
                    operation = new OperationConfiguration(HttpMethod.Get, "*");
                    m_operationsMap.Add(operation);
                }                

                return operation;
            }
        }
		#endregion

        #region Methods
        /// <summary>
        /// Creates a configuration for an operation on current resource.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="path">The path.</param>        
        public OperationConfiguration Operation(HttpMethod httpMethod, string path)
		{
            var operation = new OperationConfiguration(httpMethod, path);
            m_operationsMap.Add(operation);

            return operation;
		}       

        public OperandBase HttpHeader(string headerName)
        {
            var operand = new HttpHeaderOperand(headerName);
            LeftOperands.Add(operand);

            return operand;
        }

        /// <summary>
        /// Gets if the targer of configuration can be shown.
        /// </summary>
		bool? IResourceConfiguration.CanShow
		{
            get
            {
                return base.IsMapped(null);
            }
		}

        /// <summary>
        /// Determines whether this the operation can be shown.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        bool IResourceConfiguration.IsOperationMapped(HttpMethod httpMethod, string path)
		{
            var operation = GetOperation(httpMethod, path);    

            if (operation != null)
            {
               return operation.IsMapped(null);
            }

            return true;  
        }

        public OperationConfiguration GetOperation(HttpMethod httpMethod, string path)
        {
            return GetOperation(httpMethod, path, o => true);
        }

        public OperationConfiguration GetOperation(HttpMethod httpMethod, string path, Func<OperationConfiguration, bool> where)
        {
            var operation = m_operationsMap.Where(where).FirstOrDefault(
                a => path.EndsWith(a.Path, StringComparison.OrdinalIgnoreCase)
                  && httpMethod.Equals(a.HttpMethod));

            if (operation == null)
            {
                operation = GetAllOperationConfig();
            }

            return operation;
        }

        /// <summary>
        /// Gets all operations config.
        /// </summary>
        /// <returns></returns>
        private OperationConfiguration GetAllOperationConfig()
        {
            return m_operationsMap
                .Where(a => a.Path.Equals("*"))
                .OrderByDescending(a => a.IsMapped(null)) // Orders by CanShow, so the true or false one will be returned first.
                .FirstOrDefault();
        }
        #endregion
    }
}
