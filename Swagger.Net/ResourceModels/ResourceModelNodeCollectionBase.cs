using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Swagger.Net.ResourceModels
{
	/// <summary>
	/// A base class for resource model node collection.
	/// </summary>
	public abstract class ResourceModelNodeCollectionBase<TResourceModel> : System.Dynamic.DynamicObject where TResourceModel : ResourceModelNodeBase
	{
		#region Fields
		private List<TResourceModel> m_models;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Swagger.Net.ResourceModels.ResourceModelNodeCollectionBase{TResourceModel}"/> class.
		/// </summary>
		protected ResourceModelNodeCollectionBase()
		{
			m_models = new List<TResourceModel>();
		}
		#endregion

        #region Properties
        public int Count
        {
            get
            {
                return m_models.Count;
            }
        }
        #endregion

        #region Methods
        /// <summary>
		/// Adds a model to the collection.
		/// </summary>
		/// <param name="model">The model.</param>
		public void Add(TResourceModel model)
		{
			if (model != null)
			{
				m_models.Add(model);
			}
		}

        public void AddRange(IEnumerable<TResourceModel> models)
        {
            foreach (var m in models)
            {
                if (!m_models.Any(a => a.Id.Equals(m.Id, System.StringComparison.Ordinal)))
                {
                    m_models.Add(m);
                }
            }
        }

		/// <summary>
		/// Returns the enumeration of all dynamic member names.
		/// </summary>
		/// <returns>
		/// A sequence that contains dynamic member names.
		/// </returns>
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return m_models.Select(m => m.Id).ToList();
		}

		/// <summary>
		/// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
		/// </summary>
		/// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
		/// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result"/>.</param>
		/// <returns>
		/// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
		/// </returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = m_models.FirstOrDefault(m => m.Id.Equals(binder.Name));
			return true;
		}
		#endregion
	}
}
