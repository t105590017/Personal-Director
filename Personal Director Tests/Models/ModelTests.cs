using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal_Director.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace Personal_Director_Tests.Models
{
    [TestClass]
    public class ModelTests
    {

        //新增Media至Model
        [TestMethod]
        public void AddMediaIntoCabinetDataTest()
        {
            //Arrange
            Model sut = new Model();
            Media media = new Media();

            //Act
            sut.AddMediaIntoCabinetData(media);

            //Assert
            Assert.AreEqual(1, sut.getAllMediaCabinetData().Count);
        }

        //新增分鏡至腳本
        [TestMethod]
        public void AddStoryBoardIntoScriptDataTest()
        {
            //Arrange
            Model sut = new Model();
            StoryBoard storyBoard = new StoryBoard(new Media());
            //Act
            sut.AddStoryBoardIntoScriptData(storyBoard);

            //Assert
            Assert.AreEqual(1, sut.getAllStoryBoardScriptData().Count);

        }

        //插入分鏡至腳本
        [TestMethod]
        public void InsertStoryBoardIntoScriptData()
        {
            //Arrange
            Model sut = new Model();
            Media media = new Media();
            media.Describe = "storyBoard1";
            StoryBoard storyBoard = new StoryBoard(media);

            //Act
            sut.InsertStoryBoardIntoScriptData(0, storyBoard);

            //Assert
            Assert.AreEqual("storyBoard1", storyBoard.MediaSource.Describe);

        }

        //從分鏡腳本內刪除分鏡
        [TestMethod]
        public void RemoveStoryBoardFromScriptDataTest()
        {
            //Arrange
            Model model = new Model();
            StoryBoard storyBoard = new StoryBoard(new Media());

            //Act
            model.AddStoryBoardIntoScriptData(storyBoard);
            model.RemoveStoryBoardFromScriptData(storyBoard);

            //Assert
            Assert.AreEqual(0, model.getAllStoryBoardScriptData().Count);
        }

        //取得媒體櫃所有資料
        [TestMethod]
        public void GetAllMediaCabinetDataTest()
        {
            //Arrange
            Model model = new Model();

            //Act
            ObservableCollection<Media> mediaCabinet = model.getAllMediaCabinetData();

            //Assert
            Assert.AreEqual(0, mediaCabinet.Count);
        }

        //取得媒體櫃所有資料
        [TestMethod]
        public void GetAllStoryBoardScriptDataTest()
        {
            //Arrange
            Model model = new Model();

            //Act
            ObservableCollection<StoryBoard> script = model.getAllStoryBoardScriptData();

            //Assert
            Assert.AreEqual(0, script.Count);
        }
    }
}
