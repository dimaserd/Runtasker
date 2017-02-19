using Microsoft.Office.Interop.Word;

namespace Extensions.File.MicrosoftWord
{
    public static class MicrosoftWordReader
    {
        public static string ReadTextFromWordFile(string docPath)
        {
            //Create Doc

            Application app = new Application();
            Document doc = app.Documents.Open(docPath);

            //Get all words
            string allWords = doc.Content.Text;
            
            
            doc.Close();
            app.Quit();

            return allWords;
        }
    }
}
