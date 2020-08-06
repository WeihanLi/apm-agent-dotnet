using System.Collections.Generic;

namespace CMS.API.Campaign.WebApi.Util
{
    /// <summary>
    /// Follow with jsonapi top level spec
    /// </summary>
    /// <see cref="https://jsonapi.org/format/#document-top-level"/>
    public class JsonSpecDto<T>
    {
        /// <summary>
        /// the document’s “primary data”
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// an array of error objects
        /// </summary>
        /// <see cref="https://jsonapi.org/format/#errors"/>
        public List<string> Error { get; set; }
        /// <summary>
        /// a meta object that contains non-standard meta-information.
        /// </summary>
        /// <see cref="https://jsonapi.org/format/#document-meta"/>
        public object Meta { get; set; }

        public JsonSpecDto()
        {
            Data = default(T);
            Error = new List<string>();
            Meta = new object();
        }
    }

    /// <summary>
    /// Follow with jsonapi top level spec
    /// </summary>
    /// <see cref="https://jsonapi.org/format/#document-top-level"/>
    public class JsonSpecDto
    {
        /// <summary>
        /// the document’s “primary data”
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// an array of error objects
        /// </summary>
        /// <see cref="https://jsonapi.org/format/#errors"/>
        public List<string> Error { get; set; }
        /// <summary>
        /// a meta object that contains non-standard meta-information.
        /// </summary>
        /// <see cref="https://jsonapi.org/format/#document-meta"/>
        public object Meta { get; set; }

        public JsonSpecDto()
        {
            Data = new object();
            Error = new List<string>();
            Meta = new object();
        }
    }
}
