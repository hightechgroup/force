using System;
using Demo.WebApp.Features.Blog;
using Xunit;

namespace Force.AspNetCore.Mvc.Tests
{
    public class ControllerExtensionsTests
    {
        [Fact]
        public void Test1()
        {
            var postController = new PostController();
            var postQuery = new GetPostList();
            var res = postController.Get(postQuery);
        }
    }
}