using System;
using System.IO;

namespace KTool.FileIo
{
    public static class IoText
    {
        #region Properties

        #endregion

        #region Method
        public static ResultReadText Read(string path, object state = null)
        {
            if (!File.Exists(path))
                return ResultReadText.Fail(PathUnit.ERROR_FILE_NOT_FOUND, state);
            //
            ResultReadText result;
            Stream stream = null;
            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(stream);
                string text = sr.ReadToEnd();
                sr.Close();
                //
                result = ResultReadText.Success(text, state);
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Close();
                result = ResultReadText.Fail(ex.Message, state);
            }
            //
            return result;
        }
        public static ResultWrite Write(string path, string text, object state = null)
        {
            string error;
            Stream stream = null;
            try
            {
                if (File.Exists(path))
                {
                    stream = File.Open(path, FileMode.Truncate, FileAccess.Write);
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(path);
                    DirectoryInfo directory = fileInfo.Directory;
                    if (!directory.Exists)
                        directory.Create();
                    stream = File.Open(path, FileMode.Create, FileAccess.Write);
                }
                StreamWriter sw = new StreamWriter(stream);
                sw.Write(text);
                sw.Flush();
                sw.Close();
                //
                error = string.Empty;
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Close();
                error = ex.Message;
            }
            //
            if (string.IsNullOrEmpty(error))
                return ResultWrite.Success(state);
            else
                return ResultWrite.Fail(error, state);
        }
        #endregion
    }
}
