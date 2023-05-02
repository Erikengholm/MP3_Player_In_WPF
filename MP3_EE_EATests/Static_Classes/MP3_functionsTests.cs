using Microsoft.VisualStudio.TestTools.UnitTesting;
using MP3_EE_EA.Models;
using MP3_EE_EA.Static_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MP3_EE_EA.Static_Classes.Tests
{
    [TestClass()]
    public class MP3_functionsTests
    {
        [TestMethod()]
        public void TimeToString_Test()
        {

            var result = MP3_functions.TimeToString(-2000);
            var result2 = MP3_functions.TimeToString(2000);
            var result3 = MP3_functions.TimeToString(4000);


            Assert.AreEqual("00:00",result);
            Assert.AreEqual("33:20",result2);
            Assert.AreEqual("01:06:40", result3);

        }

        [TestMethod()]
        public void Get_MP3_Folder_Test()
        {
            var folder = List_Helper.TryGetMp3Folder();

            Assert.IsNotNull(folder);


            string name = folder.Name;

            Assert.AreEqual("Mp3_Files", name);

        }

        [TestMethod()]
        public void Fill_Mp3_List_From_Folder_Test()
        {
            var folder = List_Helper.Fill_List_From_Folder();

            Assert.IsNotNull(folder);
        }

        [TestMethod()]
        public void Shuffle_Test()
        {
            var NumberList = Enumerable.Range(0, 100).ToList();

            var NumberList2 = NumberList.ToList();

            NumberList.Shuffle();


            Assert.AreNotEqual(NumberList, NumberList2);
        }

        //[TestMethod()]
        //public void Media_Player_Singleton_Test()
        //{
        //    Media_Player_Singleton.Instance.SongModels = List_Helper.Fill_List();


        //    Application.Current.Dispatcher.Invoke((Action)delegate {
        //        DataGrid dataGrid = new DataGrid();

        //        dataGrid.DataContext = Media_Player_Singleton.Instance.SongModels;


        //        dataGrid.SelectedItem = Media_Player_Singleton.Instance.SongModels[10];

        //        Image img = new Image()
        //        {
        //            DataContext = dataGrid.SelectedItem,
        //        };


        //        Media_Player_Singleton.Instance.Shuffle_The_MP3(img, dataGrid);


        //        Assert.IsNotNull(Media_Player_Singleton.Instance.Shuffle_Song_Order);
        //        Assert.Equals(true, Media_Player_Singleton.Instance.Shuffle_Song_Order.Any());
        //        Assert.Equals(10, Media_Player_Singleton.Instance.Shuffle_Index);
        //    });
        //}

    }
}