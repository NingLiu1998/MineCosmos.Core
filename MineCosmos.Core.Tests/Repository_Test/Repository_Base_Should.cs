﻿using MineCosmos.Core.Model.Models;
using Xunit;
using System;
using System.Linq;
using Autofac;
using MineCosmos.Core.IRepository.Base;

namespace MineCosmos.Core.Tests
{
    public class Repository_Base_Should
    {
        private IBaseRepository<BlogArticle> baseRepository;
        DI_Test dI_Test = new DI_Test();

        public Repository_Base_Should()
        {

            var container = dI_Test.DICollections();

            baseRepository = container.Resolve<IBaseRepository<BlogArticle>>();

            //DbContext.Init(BaseDBConfig.ConnectionString,(DbType)BaseDBConfig.DbType);
        }


        [Fact]
        public async void Get_Blogs_Test()
        {
            var data = await baseRepository.Query();

            Assert.NotNull(data);
        }

        [Fact]
        public async void Add_Blog_Test()
        {
            BlogArticle blogArticle = new BlogArticle()
            {
                Title = "xuint test title",
                Content = "xuint test content",
                Submitter = "xuint： test repositoryBase add blog",
            };

            var BId = await baseRepository.Add(blogArticle);
            Assert.True(BId > 0);
        }


        [Fact]
        public async void Update_Blog_Test()
        {
            var IsUpd = false;
            var updateModel = (await baseRepository.Query(d => d.Title == "xuint test title")).FirstOrDefault();

            Assert.NotNull(updateModel);

            updateModel.Content = "xuint: test repositoryBase content update";

            IsUpd = await baseRepository.Update(updateModel);

            Assert.True(IsUpd);
        }

        [Fact]
        public async void Delete_Blog_Test()
        {
            var IsDel = false;
            var deleteModel = (await baseRepository.Query(d => d.Title == "xuint test title")).FirstOrDefault();

            Assert.NotNull(deleteModel);

            IsDel = await baseRepository.Delete(deleteModel);

            Assert.True(IsDel);
        }
    }
}
