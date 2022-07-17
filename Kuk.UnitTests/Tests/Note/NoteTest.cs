using Kuk.Common.Messages;
using Kuk.Data;
using Kuk.Data.Repositories;
using Kuk.Entities.EntityModels;
using Kuk.Services.Services.Note.Implementation;
using Kuk.Services.Services.Note.Messaging;
using Kuk.Services.Services.Note.ViewModel;
using Kuk.UnitTests.Mock;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Kuk.UnitTests.Tests.Note
{

    [TestFixture]
    public class NoteTest
    {
        private NoteService _noteService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var context = new MockKukDbContext();
            _noteService = new NoteService(new NoteRepository(context.MockAndSeedDbContext()));
        }

        [Test, Order(1)]
        [Category("GetById")]
        public async Task TestGetById_CheckIsSuccess_WithZeroId()
        {
            var result = await _noteService.GetByIdAsync(0);
            Assert.AreEqual(result.IsSuccess, false);
        }

        [Test, Order(1)]
        [Category("GetById")]
        public async Task TestGetById_CheckMessage_WithZeroId()
        {
            var result = await _noteService.GetByIdAsync(0);
            Assert.AreEqual(result.Message, ValidationMessagesResource.IdGtZero);
        }

        [Test, Order(1)]
        [Category("GetById")]
        public async Task TestGetById_CheckMessageAndIsSuccess_WithFalseInput()
        {
            var result = await _noteService.GetByIdAsync(3);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, MessagesResource.NotExistData);
        }

        [Test, Order(1)]
        [Category("GetById")]
        public async Task TestGetById_CheckMessageAndIsSuccess_WithTrueInput()
        {
            var result = await _noteService.GetByIdAsync(1);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, MessagesResource.GetSuccess);
        }

        [Test, Order(2)]
        [Category("GetAllPaged")]
        public void TestGetAllPaged_CheckTotalCount()
        {
            var result = _noteService.GetAllPaged(new NoteGetAllPageRequest { Page = 1, PageSize = 10 });
            Assert.AreEqual(result.RowCount, 2);
        }

        [Test, Order(2)]
        [Category("GetAllPaged")]
        public void TestGetAllPaged_CheckPageSize_WithOneResult()
        {
            var result = _noteService.GetAllPaged(new NoteGetAllPageRequest { Page = 1, PageSize = 1 });
            Assert.AreEqual(result.Entity.Count, 1);
        }

        [Test, Order(2)]
        [Category("GetAllPaged")]
        public void TestGetAllPaged_CheckPageIndex_InSecondPage()
        {
            var result = _noteService.GetAllPaged(new NoteGetAllPageRequest { Page = 2, PageSize = 10 });
            Assert.AreEqual(result.Entity.Count, 0);
        }

        [Test, Order(2)]
        [Category("GetAllPaged")]
        public void TestGetAllPaged_CheckFalseSearchValue()
        {
            var result = _noteService.GetAllPaged(new NoteGetAllPageRequest { Page = 1, PageSize = 10, Search = "aaa" });
            Assert.AreEqual(result.Entity.Count, 0);
        }

        [Test, Order(2)]
        [Category("GetAllPaged")]
        public void TestGetAllPaged_CheckTrueSearchValue()
        {
            var result = _noteService.GetAllPaged(new NoteGetAllPageRequest { Page = 1, PageSize = 10, Search = "text1" });
            Assert.AreEqual(result.Entity.Count, 1);
        }

        [Test, Order(5)]
        [Category("Create")]
        public async Task TestCreate_CheckInputValidation_WithLongTitle()
        {
            var result = await _noteService.CreateAsync(new NoteCreateRequest
            {
                Entity = new NoteCreateRequestVm
                {
                    Title = "shkjahsdkjashdkjashdkjashdkjashdjkashdjkhasjkdhasjkdhajksdhjkasdhjkahsdkjhasjkdhasjkdajskdhjkashd",
                    TextBody = "asdasdasdasdasdasdasd"
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.InvalidMaxLength);
        }

        [Test, Order(5)]
        [Category("Create")]
        public async Task TestCreate_CheckInputValidation_WithEmptyTitle()
        {
            var result = await _noteService.CreateAsync(new NoteCreateRequest
            {
                Entity = new NoteCreateRequestVm
                {
                    Title = string.Empty,
                    TextBody = "asdasdasdasdasdasdasd"
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.TitleNotEmpty);
        }

        [Test, Order(5)]
        [Category("Create")]
        public async Task TestCreate_CheckInputValidation_WithNullTitle()
        {
            var result = await _noteService.CreateAsync(new NoteCreateRequest
            {
                Entity = new NoteCreateRequestVm
                {
                    TextBody = "asdasdasdasdasdasdasd"
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.TitleNotEmpty);
        }

        [Test, Order(5)]
        [Category("Create")]
        public async Task TestCreate_CheckInputValidation_WithNullTextBody()
        {
            var result = await _noteService.CreateAsync(new NoteCreateRequest
            {
                Entity = new NoteCreateRequestVm
                {
                    Title = "text3",
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.TextBodyNotEmpty);
        }

        [Test, Order(5)]
        [Category("Create")]
        public async Task TestCreate_CheckDuplicateTitle()
        {
            var result = await _noteService.CreateAsync(new NoteCreateRequest
            {
                Entity = new NoteCreateRequestVm
                {
                    Title = "text2",
                    TextBody = "some text",
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, MessagesResource.DuplicateData);
        }

        [Test, Order(5)]
        [Category("Create")]
        public async Task TestCreate_WithTrueValue()
        {
            var result = await _noteService.CreateAsync(new NoteCreateRequest
            {
                Entity = new NoteCreateRequestVm { Title = "text4", TextBody = "some text", }
            });
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, MessagesResource.AddSuccess);
        }

        [Test, Order(3)]
        [Category("Update")]
        public async Task TestUpdate_CheckInputValidation_WithZeroId()
        {
            var result = await _noteService.UpdateAsync(new NoteUpdateRequest
            {
                Entity = new NoteUpdateRequestVm { Id = 0, Title = "text3", TextBody = "some text" }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.IdGtZero);
        }

        [Test, Order(3)]
        [Category("Update")]
        public async Task TestUpdate_CheckInputValidation_WithLongTitle()
        {
            var result = await _noteService.UpdateAsync(new NoteUpdateRequest
            {
                Entity = new NoteUpdateRequestVm { 
                    Id = 1, 
                    Title = "shkjahsdkjashdkjashdkjashdkjashdjkashdjkhasjkdhasjkdhajksdhjkasdhjkahsdkjhasjkdhasjkdajskdhjkashd",
                    TextBody = "some text"
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.InvalidMaxLength);
        }

        [Test, Order(3)]
        [Category("Update")]
        public async Task TestUpdate_CheckInputValidation_WithEmptyTitle()
        {
            var result = await _noteService.UpdateAsync(new NoteUpdateRequest
            {
                Entity = new NoteUpdateRequestVm
                {
                    Id = 1,
                    Title = "",
                    TextBody = "some text"
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.TitleNotEmpty);
        }

        [Test, Order(3)]
        [Category("Update")]
        public async Task TestUpdate_CheckInputValidation_WithEmptyTextBody()
        {
            var result = await _noteService.UpdateAsync(new NoteUpdateRequest
            {
                Entity = new NoteUpdateRequestVm
                {
                    Id = 1,
                    Title = "text3",
                    TextBody = ""
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.TextBodyNotEmpty);
        }

        [Test, Order(3)]
        [Category("Update")]
        public async Task TestUpdate_CheckInputValidation_WithNullTextBody()
        {
            var result = await _noteService.UpdateAsync(new NoteUpdateRequest
            {
                Entity = new NoteUpdateRequestVm
                {
                    Id = 1,
                    Title = "text3",
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.TextBodyNotEmpty);
        }

        [Test, Order(3)]
        [Category("Update")]
        public async Task TestUpdate_CheckInputValidation_WithDuplicateTitle()
        {
            var result = await _noteService.UpdateAsync(new NoteUpdateRequest
            {
                Entity = new NoteUpdateRequestVm
                {
                    Id = 2,
                    Title = "text1",
                    TextBody = "some text"
                }
            });
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, MessagesResource.DuplicateData);
        }

        [Test, Order(4)]
        [Category("Update")]
        public async Task TestUpdate_CheckInputValidation_WithTrueValues()
        {
            var result = await _noteService.UpdateAsync(new NoteUpdateRequest
            {
                Entity = new NoteUpdateRequestVm
                {
                    Id = 1,
                    Title = "text3",
                    TextBody = "some text"
                }
            });
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, MessagesResource.EditSuccess);
        }

        [Test, Order(6)]
        [Category("Delete")]
        public async Task TestDelete_CheckInputValidation_WithZeroId()
        {
            var result = await _noteService.DeleteAsync(0);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, ValidationMessagesResource.IdGtZero);
        }

        [Test, Order(6)]
        [Category("Delete")]
        public async Task TestDelete_CheckInputValidation_WithFalseInput()
        {
            var result = await _noteService.DeleteAsync(10);
            Assert.AreEqual(result.IsSuccess, false);
            Assert.AreEqual(result.Message, MessagesResource.NotExistData);
        }

        [Test, Order(7)]
        [Category("Delete")]
        public async Task TestDelete_CheckInputValidation_WithTrueInput()
        {
            var result = await _noteService.DeleteAsync(1);
            Assert.AreEqual(result.IsSuccess, true);
            Assert.AreEqual(result.Message, MessagesResource.DeleteSuccess);
        }
    }
}
