using Extensions.Reflection;
using Extensions.String;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Runtasker.Test
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void IndexOfTest()
        {
            string s = "bla blach toMark bla blach";
            string toMark = "toMark";
            int start = s.IndexOf(toMark);

            //int index = s.FindIndex(x => x.StartsWith("whatever"));
            Assert.AreEqual(10, start);
        }

        [TestMethod]
        public void MarkTextTest()
        {
            string s = "bla blach toMark bla blach";
            string toMark = "toMark";

            Assert.AreEqual("bla blach <mark>toMark</mark> bla blach", s.MarkText(toMark));
        }

        [TestMethod]
        public void ManyMarkTest()
        {
            string keyWordsString = "keyword1,keyword2";

            string postText = "bla bla bla text la keyword1fd sdfj dfsfs keyword2 se aq";

            string expectedResult = "bla bla bla text la <mark>keyword1</mark>fd sdfj dfsfs <mark>keyword2</mark> se aq";

            string result = postText.MarkManyText(keyWordsString);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void KeyValueString()
        {
            object a = new { id = 1, prop = "ses" };

            string a_string = a.RenderAttributesKeyValuePair();

            Assert.AreEqual(" id='1' prop='ses'", a_string);
        }

        [TestMethod]
        public void KeyValueExceptTest()
        {
            object a = new { id = 1, prop = "ses", ser = 3 };

            string a_string = a.RenderAttributesKeyValuePairExcept("id", "ser");

            Assert.AreEqual(" prop='ses'", a_string);
        }

        [TestMethod]
        public void ToIdTest()
        {
            string s = "Description";

            string sId = s.ToId();

            Assert.AreEqual("description", sId);
        }


        [TestMethod]
        public void LeftJustFileNameTest()
        {
            string fileName = "aed71b55-bd73-4167-8d86-fab8124193f3.zip";
            string filePath = $"C:\\Runtasker\\Runtasker\\Files\\Attachments/{fileName}";

            string gottenFileName = filePath.LeftJustFileName();
            string gottenFileName2 = fileName.LeftJustFileName();

            Assert.AreEqual(fileName, gottenFileName);
            Assert.AreEqual(fileName, fileName);
        }

        [TestMethod]
        public void UriTest()
        {
            string url = @"https://developer.xamarin.com/guides/xamarin-forms/getting-started/introduction-to-xamarin-forms/";
            Uri uri = new Uri(url);
            Assert.AreEqual("developer.xamarin.com", uri.Host);
        }

        [TestMethod]
        public void UrlWrapTest()
        {
            string url = @"https://developer.xamarin.com/guides/";
            string text = $"test text {url} about something";

            string wrappedText = text.WrapUrls();

            Uri uri = new Uri(url);
            string expectedText = $"test text <a href=\"{url}\">{uri.Host}</a> about something";
            Assert.AreEqual(expectedText, wrappedText);
        }

        [TestMethod]
        public void StringAndNameTest()
        {
            string s1 = "ДимаСердюков";
            string s2 = "Дима Сердюков";

            string result = s1.GetNameAndSurname();

            Assert.AreEqual(s2, result);
        }

        [TestMethod]
        public void LeftJustNamesTest()
        {
            List<string> filepaths = new List<string>()
            {
                @"C:\Runtasker\Runtasker\Files\Orders\2\file1.jpg",
                @"C:\Runtasker\Runtasker\Files\Orders\2/file2.jpg"
            };
            List<string> newFilepaths = filepaths.LeftJustNames();

            Assert.AreEqual(newFilepaths[0], "file1.jpg");
            Assert.AreEqual(newFilepaths[1], "file2.jpg");
        }

        [TestMethod]
        public void MakeUniqueAtTest()
        {
            List<string> filenames = new List<string>()
            {
                "file.jpg",
                "file(1).jpg"
            };

            string newFileName = "file.jpg";

            string uniqueName = newFileName.MakeFileNameUniqueAtList(filenames);

            string anotherFileName = "file.jpeg";
            string anotherUniqueName = anotherFileName.MakeFileNameUniqueAtList(filenames);

            Assert.AreEqual("file(2).jpg", uniqueName);
            Assert.AreEqual("file.jpeg", anotherUniqueName);
        }

        [TestMethod]
        public void TryGetNameAndExtText()
        {
            string filename = "file.ext.jpg";
   
            filename.TryGetNameandExt(out string name, out string ext);

            Assert.AreEqual("file", name);
            Assert.AreEqual("ext.jpg", ext);
        }
    }
}
