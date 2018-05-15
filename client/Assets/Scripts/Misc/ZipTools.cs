using UnityEngine;
using System.Collections;

using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System;
using ICSharpCode.SharpZipLib.Checksums;
using System.Text;


public delegate void ZipProgressHandler(string filename, long loadedsize, long toltalsize);
public delegate void ZipCompleteHandler();
public delegate void ZipErrorHandler(string error);

public class ZipTools
{
    /// <summary>
    /// 压缩多层目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="zipedFile">The ziped file.</param>
    public static void ZipFileDirectory(string strDirectory, string zipedFile, ZipProgressHandler progressHandler = null, ZipCompleteHandler completeHandler = null)
    {
        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream s = new ZipOutputStream(ZipFile))
            {
                ZipSetp(strDirectory, s, "", progressHandler, completeHandler);
                if (completeHandler != null)
                {
                    completeHandler();
                }
            }
        }
    }

    /// <summary>
    /// 递归遍历目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="s">The ZipOutputStream Object.</param>
    /// <param name="parentPath">The parent path.</param>
    private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath, ZipProgressHandler progressHandler = null, ZipCompleteHandler completeHandler = null)
    {
        if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
        {
            strDirectory += Path.DirectorySeparatorChar;
        }

        string[] filenames = Directory.GetFileSystemEntries(strDirectory);
        int i = 0;
        int count = filenames.Length;
        foreach (string file in filenames)// 遍历所有的文件和目录
        {

            if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
                string pPath = parentPath;
                pPath += file.Substring(file.LastIndexOf("\\") + 1);
                pPath += "\\";
                ZipSetp(file, s, pPath, progressHandler, completeHandler);
            }

            else // 否则直接压缩文件
            {
                //打开压缩文件
                using (FileStream fs = File.OpenRead(file))
                {

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(fileName);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;

                    fs.Close();
                    i++;

                    if (progressHandler != null)
                    {
                        progressHandler(fileName, i, count);
                    }
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }

    public static long GetFileCRC(string filename)
    {
        Crc32 crc = new Crc32();
        using (FileStream fs = File.OpenRead(filename))
        {
            byte[] buffer = new byte[fs.Length];
            crc.Reset();
            crc.Update(buffer);
            return crc.Value;
        }
    }

    #region 解压文件夹压缩包
    /// <summary>
    /// 解压缩文件(压缩文件中含有子目录)
    /// </summary>
    /// <param name="zipfilepath">待解压缩的文件路径</param>
    /// <param name="unzippath">解压缩到指定目录</param>
    /// <returns>解压后的文件列表</returns>
    public static void UnZipDir(string zipfilepath, string unzippath, ZipProgressHandler progressHandler = null, ZipCompleteHandler completeHandler = null, ZipErrorHandler errorHandler = null, bool replace = true)
    {
        try
        {
            using (FileStream filestream = File.OpenRead(zipfilepath))
            {
                string zipfilename = Path.GetFileName(zipfilepath);

                //检查输出目录是否以“\\”结尾
                if (unzippath.EndsWithNonAlloc("/") == false || unzippath.EndsWithNonAlloc(":/") == false)
                {
                    unzippath += "/";
                }
                ZipInputStream s = new ZipInputStream(filestream);

                string directoryName = Path.GetDirectoryName(unzippath);
                //生成解压目录【用户解压到硬盘根目录时，不需要创建】
                if (!string.IsNullOrEmpty(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }


                string filePath;
                string fileName;
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    
                    fileName = Path.GetFileName(theEntry.Name);
                    long read_size = 0;
                   
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        //如果文件的压缩后大小为0那么说明这个文件是空的,因此不需要进行读出写入
                        if (theEntry.CompressedSize == 0)
                            break;

                        filePath = string.Format("{0}{1}", unzippath, theEntry.Name);
                        filePath = filePath.Replace("\\", "/");

                        if(replace == false && File.Exists(filePath))
                        {
                            continue;
                        }

                        //建立下面的目录和子目录
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        FileStream streamWriter = File.Create(filePath);

                        int size = 1024;
                        byte[] data = new byte[1024];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                read_size += size;
                                streamWriter.Write(data, 0, size);
                                if (progressHandler != null)
                                {
                                    string message = string.Format("{0}({1})", zipfilename, Path.GetFileNameWithoutExtension(theEntry.Name));
                                    progressHandler(message, read_size, s.Length);
                                }
                            }
                            else
                            {

                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
                s.Dispose();
                s.Close();
                //GC.Collect();
                if (completeHandler != null)
                    completeHandler();
            }
        }
        catch (Exception e)
        {
            if (errorHandler != null)
                errorHandler(e.Message);
            DebugLogger.Log(e);
        }

    }
    #endregion
}


