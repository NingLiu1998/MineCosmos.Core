using Autofac;
using MineCosmos.Core.Controllers;
using MineCosmos.Core.IServices;
using MineCosmos.Core.Model;
using MineCosmos.Core.Model.Models;
using MineCosmos.Core.Model.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MineCosmos.Core.Tests
{
    public class BlogController_Should
    {
        Mock<IBlogArticleServices> mockBlogSev = new Mock<IBlogArticleServices>();
        Mock<ILogger<BlogController>> mockLogger = new Mock<ILogger<BlogController>>();
        BlogController blogController;

        private IBlogArticleServices blogArticleServices;
        DI_Test dI_Test = new DI_Test();



        public BlogController_Should()
        {
            mockBlogSev.Setup(r => r.Query());

            var container = dI_Test.DICollections();
            blogArticleServices = container.Resolve<IBlogArticleServices>();
            blogController = new BlogController(mockLogger.Object);
            blogController._blogArticleServices = blogArticleServices;
        }

        [Fact]
        public void TestEntity()
        {
            BlogArticle blogArticle = new BlogArticle();

            Assert.True(blogArticle.Id >= 0);
        }

        [Fact]
        public async void Get_Blog_Page_Test()
        {
            MessageModel<PageModel<BlogArticle>> blogs = await blogController.Get(1, 1, "技术博文", "");
            Assert.NotNull(blogs);
            Assert.NotNull(blogs.response);
            Assert.True(blogs.response.dataCount >= 0);
        }

        [Fact]
        public async void Get_Blog_Test()
        {
            MessageModel<BlogViewModels> blogVo = await blogController.Get(1);

            Assert.NotNull(blogVo);
        }

        [Fact]
        public async void Get_Blog_For_Nuxt_Test()
        {
            MessageModel<BlogViewModels> blogVo = await blogController.DetailNuxtNoPer(1);

            Assert.NotNull(blogVo);
        }

        [Fact]
        public async void Get_Go_Url_Test()
        {
            object urlAction = await blogController.GoUrl(1);

            Assert.NotNull(urlAction);
        }

        [Fact]
        public async void Get_Blog_By_Type_For_MVP_Test()
        {
            MessageModel<List<BlogArticle>> blogs = await blogController.GetBlogsByTypesForMVP("技术博文");

            Assert.NotNull(blogs);
            Assert.True(blogs.success);
            Assert.NotNull(blogs.response);
            Assert.True(blogs.response.Count >= 0);
        }

        [Fact]
        public async void Get_Blog_By_Id_For_MVP_Test()
        {
            MessageModel<BlogArticle> blog = await blogController.GetBlogByIdForMVP(1);

            Assert.NotNull(blog);
            Assert.True(blog.success);
            Assert.NotNull(blog.response);
        }

        [Fact]
        public async void PostTest()
        {
            BlogArticle blogArticle = new BlogArticle()
            {
                Title = "xuint :test controller addEntity",
                Content = "xuint :test controller addEntity. this is content.this is content."
            };

            var res = await blogController.Post(blogArticle);

            Assert.True(res.success);

            var data = res.response;

            Assert.NotNull(data);
        }

        [Fact]
        public async void Post_Insert_For_MVP_Test()
        {
            BlogArticle blogArticle = new BlogArticle()
            {
                Title = "xuint :test controller addEntity",
                Content = "xuint :test controller addEntity. this is content.this is content."
            };

            var res = await blogController.AddForMVP(blogArticle);

            Assert.True(res.success);

            var data = res.response;

            Assert.NotNull(data);
        }

        [Fact]
        public async void Put_Test()
        {
            BlogArticle blogArticle = new BlogArticle()
            {
                Id = 1,
                Title = "xuint put :test controller addEntity",
                Content = "xuint put :test controller addEntity. this is content.this is content."
            };

            var res = await blogController.Put(blogArticle);

            Assert.True(res.success);

            var data = res.response;

            Assert.NotNull(data);
        }

        [Fact]
        public async void Delete_Test()
        {
            var res = await blogController.Delete(99);

            Assert.False(res.success);

            var data = res.response;

            Assert.Null(data);
        }

        [Fact]
        public async void Apache_Update_Test()
        {
            var res = await blogController.ApacheTestUpdate();

            Assert.True(res.success);
        }
    }
}
