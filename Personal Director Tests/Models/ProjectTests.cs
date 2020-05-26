using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal_Director.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Personal_Director_Tests.Models
{
    [TestClass]
    public class ProjectTests
    {
        //將專案Json轉為字串
        [TestMethod]
        public void GetProjectInfoTest()
        {
            //Arrange
            string jsonString = @"{""MediaCabinet"":[{""path"":""C:\\Users\\LaAaa\\Videos\\guitar.mp4"",""Guid"":""c4d41b0b-4f29-47cc-b84d-09273e6268ad""}],""Script"":[{""Guid"":""22f9f73a-d951-422e-9f69-240635feea80""}]}";
            Project sut = new Project(jsonString);

            //Act
            string result = sut.GetProjectInfo();

            //Assert
            Assert.AreEqual(@"{""MediaCabinet"":[{""path"":""C:\\Users\\LaAaa\\Videos\\guitar.mp4"",""Guid"":""c4d41b0b-4f29-47cc-b84d-09273e6268ad""}],""Script"":[{""Guid"":""22f9f73a-d951-422e-9f69-240635feea80""}]}", result);
        }

        //新增媒體至媒體櫃Json
        [TestMethod]
        public void AddMediaIntoCabinetJsonTest()
        {
            //Arrange
            string jsonString = @"{""MediaCabinet"":[{""path"":""C:\\Users\\LaAaa\\Videos\\guitar.mp4"",""Guid"":""c4d41b0b-4f29-47cc-b84d-09273e6268ad""}],""Script"":[{""Guid"":""22f9f73a-d951-422e-9f69-240635feea80""}]}";
            Project sut = new Project(jsonString);
            Guid guid = Guid.NewGuid();
            Media media = new Media(guid);

            //Act
            sut.AddMediaIntoCabinetJson("C:\\Users\\LaAaa\\Videos\\TestVideo.mp4", media);

            //Assert
            Assert.AreEqual("C:\\Users\\LaAaa\\Videos\\TestVideo.mp4", sut.GetMediaCabinetPaths().Last());
            Assert.AreEqual(guid, Guid.Parse(sut.GetMediaCabinetGuids().Last()));
        }

        //從JsonArray取得媒體櫃中的影片路徑
        [TestMethod]
        public void GetMediaCabinetPathTest()
        {
            //Arrange
            string jsonString = @"{""MediaCabinet"":[{""path"":""C:\\Users\\LaAaa\\Videos\\guitar.mp4"",""Guid"":""c4d41b0b-4f29-47cc-b84d-09273e6268ad""}],""Script"":[{""Guid"":""22f9f73a-d951-422e-9f69-240635feea80""}]}";
            Project sut = new Project(jsonString);

            //Act
            List<string> result = sut.GetMediaCabinetPaths();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("C:\\Users\\LaAaa\\Videos\\guitar.mp4", result[0]);
        }

        //取得媒體櫃中Media的Guid集合
        [TestMethod]
        public void GetMediaCabinetGuidTest()
        {
            //Arrange
            string jsonString = @"{""MediaCabinet"":[{""path"":""C:\\Users\\LaAaa\\Videos\\guitar.mp4"",""Guid"":""c4d41b0b-4f29-47cc-b84d-09273e6268ad""}],""Script"":[{""Guid"":""22f9f73a-d951-422e-9f69-240635feea80""}]}";
            Project sut = new Project(jsonString);

            //Act
            List<string> result = sut.GetMediaCabinetGuids();

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("c4d41b0b-4f29-47cc-b84d-09273e6268ad", result[0]);
        }

        [TestMethod]
        public void AddStoryBoardIntoScriptJsonTest()
        {
            //Arrange
            string jsonString = @"{""MediaCabinet"":[{""path"":""C:\\Users\\LaAaa\\Videos\\guitar.mp4"",""Guid"":""c4d41b0b-4f29-47cc-b84d-09273e6268ad""}],""Script"":[{""Guid"":""22f9f73a-d951-422e-9f69-240635feea80""}]}";
            Project sut = new Project(jsonString);
            Guid guid = Guid.NewGuid();
            StoryBoard storyBoard = new StoryBoard(new Media(guid));

            //Act
            sut.AddStoryBoardIntoScriptJson(storyBoard);

            //Assert
            Assert.AreEqual(2, sut.GetMediaSourceGuids().Count);
            Assert.AreEqual(guid, sut.GetMediaSourceGuids().Last());
        }
    }
}
