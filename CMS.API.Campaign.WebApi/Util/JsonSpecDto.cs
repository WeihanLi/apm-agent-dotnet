using System.Collections.Generic;

namespace CMS.API.Campaign.WebApi.Util
{
    /// <summary>
    /// Follow with json api top level spec
    /// https://jsonapi.org/format/#document-top-level
    /// </summary>
    public class JsonSpecDto<T>
    {
        /// <summary>
        /// the document’s “primary data”
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// an array of error objects
        /// https://jsonapi.org/format/#errors
        /// </summary>
        public List<string> Error { get; set; }

        /// <summary>
        /// a meta object that contains non-standard meta-information.
        /// https://jsonapi.org/format/#document-meta
        /// </summary>
        public object Meta { get; set; }

        public JsonSpecDto()
        {
            Data = default(T);
            Error = new List<string>();
            Meta = new object();
        }
    }

    /// <summary>
    /// Follow with json api top level spec
    /// https://jsonapi.org/format/#document-top-level
    /// </summary>
    public class JsonSpecDto
    {
        /// <summary>
        /// the document’s “primary data”
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// an array of error objects
        /// https://jsonapi.org/format/#errors
        /// </summary>
        public List<string> Error { get; set; }

        /// <summary>
        /// a meta object that contains non-standard meta-information.
        /// https://jsonapi.org/format/#document-meta
        /// </summary>
        public object Meta { get; set; }

        public JsonSpecDto()
        {
            Data = new object();
            Error = new List<string>();
            Meta = new object();
        }
    }
}