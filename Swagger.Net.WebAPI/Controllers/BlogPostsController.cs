using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Swagger.Net.WebApi.Controllers
{
    public class BlogPostsController : ApiController
    {
        // GET api/blogposts
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/blogposts/5
        public string Get(int id)
        {
            return "value";
        }


        /// <summary>
        /// Posts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception code="400" message="Value should be greater than zero." Test1="keep-alive" Test2="something" Test3="v2"/>				
        public void Post([FromBody]string value)
        {
        }

        // PUT api/blogposts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/blogposts/5
        public void Delete(int id)
        {
        }
    }
}
