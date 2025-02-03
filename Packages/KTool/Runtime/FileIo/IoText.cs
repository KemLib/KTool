using System;
using System.IO;

namespace KTool.FileIo
{
    public static class IoText
    {
        #region Properties

        #endregion

        #region Method
        public static Result<string> Read(string path, object state = null)
        {
            if (!File.Exists(path))
                return Result<string>.FailException(string.Empty, PathUnit.ERROR_FILE_NOT_FOUND, state);
            //
            Result<string> result;
            Stream stream = null;
            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(stream);
                string text = sr.ReadToEnd();
                sr.Close();
                //
                result = Result<string>.Success(text, state);
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Close();
                result = Result<string>.FailException(string.Empty, ex.Message, state);
            }
            //
            return result;
        }
        public static Result Write(string path, string text, object state = null)
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
                return Result.Success(state);
            else
                return Result.FailException(error, state);
        }
        #endregion
    }
}
