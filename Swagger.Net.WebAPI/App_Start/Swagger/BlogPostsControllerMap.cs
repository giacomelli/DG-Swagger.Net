using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Swagger.Net.ResourceModels.Configuration;
using Swagger.Net.WebApi.Controllers;

namespace Swagger.Net.WebApi.App_Start.Swagger
{
    public class BlogPostsControllerMap : ResourceConfiguration<BlogPostsController>
    {
        public BlogPostsControllerMap()
        {
            
            //this.When().HttpHeader("Connection", "keep-alive").Show();
            
            //this.AllActions().When().HttpHeader("Connection", "keep-alive").Hide();
            //this.Action(HttpMethod.Get, "/{id}").When().HttpHeader("Connection", "keep-alive").Show();


            //this.AllOperations().ErrorResponses().When().HttpHeader("Connection", "keep-alive").Hide();
            //this.AllOperations().ErrorResponses().When().Attribute("Test1", "Connection").Hide();
            //this.AllOperations().ErrorResponses().When().Attribute("Test2", "Connection").Show();
            //this.AllOperations().When().HttpHeader("Connection", "keep-alive").Show();
            //this.Operation(HttpMethod.Get, "{id}").When().HttpHeader("Connection", "keep-alive").Hide();            
            //this.AllOperations().ErrorResponses().When().HttpHeader(

            this.AllOperations.ErrorResponses.Attribute("Test1").IsEqualToHttpHeader("Connection");
            this.AllOperations.ErrorResponses.Attribute("Test2").IsNotEqualToHttpHeader("Connection");            
            this.AllOperations.ErrorResponses.Attribute("Test3").IsEqualToValue("v3", "v2");

            //this.AllOperations().HttpHeader("Connection").IsEqualToHttpHeader("fdsasd");
        }
    }
}