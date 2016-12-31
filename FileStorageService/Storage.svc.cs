using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
namespace FileStorageService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Storage”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Storage.svc 或 Storage.svc.cs，然后开始调试。
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Storage : IStorage
    {
        Random rnd = new Random();

        public enum Result
        {
            Success,
            FolderCreateFailed,
            FileNameAlreadyExist,
            File2Big,
            EmptyByteArray,
            InvaildExtName,
            InvaildName,
            StoreFailed
        }

        public Result StoreByteArray(string appName, FileGenarationOption FGO, byte[] fileContent, string postedFileName, string filter, long maxFileLength, out string urlStr)
        {
            urlStr = "";


            if (maxFileLength == 0)
            {
                maxFileLength = int.MaxValue;
            }

            if (fileContent.LongLength > maxFileLength)
            {
                return Result.File2Big;
            }

            if (fileContent.LongLength == 0)
            {
                return Result.EmptyByteArray;
            }

            if (postedFileName == "")
            {
                return Result.InvaildName;
            }

            postedFileName = postedFileName.ToLower();
            string extName = Path.GetExtension(postedFileName).ToLower();
            string dirName = "D:\\CityNews\\Photo\\";

            if (!Directory.Exists(dirName))
            {
                try
                {
                    Directory.CreateDirectory(dirName);
                }
                catch
                {
                    if (!Directory.Exists(dirName))
                    {
                        return Result.FolderCreateFailed;
                    }
                }
            }

            //是否有子文件夹
            if (FGO.needSubDir == true)
            {
                dirName += FGO.subDir + "\\";
                if (!Directory.Exists(dirName))
                {
                    try
                    {
                        Directory.CreateDirectory(dirName);
                    }
                    catch
                    {
                        if (!Directory.Exists(dirName))
                        {
                            return Result.FolderCreateFailed;
                        }
                    }
                }

            }

            //扩展名检查
            if (filter != "")
            {
                filter = filter.ToLower();
                if (filter.Contains("!"))  //黑名单
                {
                    filter = filter.Replace("!", "");
                    List<string> extNameArray = filter.Split('|').ToList();
                    if (extNameArray.Contains(extName))
                    {
                        return Result.InvaildExtName;
                    }
                }
                else //白名单
                {
                    List<string> extNameArray = filter.Split('|').ToList();
                    if (!extNameArray.Contains(extName))
                    {
                        return Result.InvaildExtName;
                    }
                }
            }
            else
            {

            }

            string timeStr = DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "");

            //文件名
            string fileName = "";
            if (FGO.useDefaultName == true)
            {
                fileName += timeStr;
                fileName += rnd.Next(10000000, 99999999).ToString();
                fileName += extName;
            }
            else if (FGO.useDefaultName == false)
            {
                fileName += FGO.fileName;
            }

            //文件存放路径
            string filePath = "";

            //选择文件夹存放方式
            switch (FGO.StoreOption)
            {
                //useRoot
                case FileGenarationOption._StoreOption.useRootDir:
                    filePath += dirName;
                    filePath += fileName;

                    break;

                //byDate
                case FileGenarationOption._StoreOption.ByDate:
                    string todayStr = DateTime.Today.ToString("yyyyMMdd");
                    dirName += todayStr + "\\";
                    if (!Directory.Exists(dirName))
                    {
                        try
                        {
                            Directory.CreateDirectory(dirName);
                        }
                        catch
                        {
                            if (!Directory.Exists(dirName))
                            {
                                return Result.FolderCreateFailed;
                            }
                        }
                    }
                    filePath += dirName;
                    filePath += fileName;

                    break;

                //byWeek
                case FileGenarationOption._StoreOption.ByWeek:
                    string weekStr = "WeekStart";
                    int adjust = 0;
                    switch (DateTime.Today.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            adjust = -6;
                            break;
                        case DayOfWeek.Monday:
                            adjust = 0;
                            break;
                        case DayOfWeek.Tuesday:
                            adjust = -1;
                            break;
                        case DayOfWeek.Wednesday:
                            adjust = -2;
                            break;
                        case DayOfWeek.Thursday:
                            adjust = -3;
                            break;
                        case DayOfWeek.Friday:
                            adjust = -4;
                            break;
                        case DayOfWeek.Saturday:
                            adjust = -5;
                            break;
                    }

                    weekStr += DateTime.Today.AddDays(adjust).ToString("yyyyMMdd");

                    dirName += weekStr + "\\";
                    if (!Directory.Exists(dirName))
                    {
                        try
                        {
                            Directory.CreateDirectory(dirName);
                        }
                        catch
                        {
                            if (!Directory.Exists(dirName))
                            {
                                return Result.FolderCreateFailed;
                            }
                        }
                    }
                    filePath += dirName;
                    filePath += fileName;

                    break;

                //byMonth
                case FileGenarationOption._StoreOption.ByMonth:
                    string monthStr = "";
                    monthStr += DateTime.Today.ToString("yyyyMM");
                    dirName += monthStr + "\\";
                    if (!Directory.Exists(dirName))
                    {
                        try
                        {
                            Directory.CreateDirectory(dirName);
                        }
                        catch
                        {
                            if (!Directory.Exists(dirName))
                            {
                                return Result.FolderCreateFailed;
                            }
                        }
                    }
                    filePath += dirName;
                    filePath += fileName;

                    break;
            }

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("已经存在"))
                {
                    return Result.FileNameAlreadyExist;
                }
                else
                {
                    throw ex;
                }
            }

            string rootPath = "D:\\CityNews\\Photo\\";
            filePath = filePath.Replace(rootPath, "");
            filePath = filePath.Replace("\\", "/");
            urlStr += filePath;

            return Result.Success;
        }
    }
}
