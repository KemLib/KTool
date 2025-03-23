using System;
using System.IO;

namespace KTool.FileIo
{
    public static class IoBinary
    {
        #region Properties
        private const int BUFFER_SIZE = 8192;
        #endregion

        #region Method
        public static ResultRead Read(string path, object state = null)
        {
            if (!File.Exists(path))
                return ResultRead.Fail(PathUnit.ERROR_FILE_NOT_FOUND, state);
            //
            ResultRead result;
            Stream stream = null;
            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(stream);
                byte[] data = new byte[stream.Length];
                int offset = 0;
                while (offset < data.Length)
                {
                    byte[] buffer = br.ReadBytes(BUFFER_SIZE);
                    buffer.CopyTo(data, offset);
                    offset += buffer.Length;
                }
                stream.Close();
                //
                result = ResultRead.Success(data, state);
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Close();
                result = ResultRead.Fail(ex.Message, state);
            }
            //
            return result;
        }
        public static ResultWrite Write(string path, byte[] data, object state = null)
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
                int offset = 0;
                while (offset < data.Length)
                {
                    int count = Math.Min(BUFFER_SIZE, data.Length - offset);
                    stream.Write(data, offset, count);
                    offset += count;
                }
                stream.Flush();
                stream.Close();
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
