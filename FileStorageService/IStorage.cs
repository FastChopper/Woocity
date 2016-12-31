using System.Runtime.Serialization;
using System.IO;
using System.ServiceModel;
namespace FileStorageService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IStorage”。
    [ServiceContract]
    public interface IStorage
    {
        [OperationContract]
        Storage.Result StoreByteArray(string appName, FileGenarationOption FGO, byte[] fileContent, string postedFileName, string filter, long maxFileLength, out string urlStr);
    }

    [DataContract]
    public class FileGenarationOption
    {
        //是否需要自定义名称
        [DataMember]
        public bool useDefaultName { get; set; }
        //文件名称
        [DataMember]
        public string fileName { get; set; }
        //是否要自定义子文件夹
        [DataMember]
        public bool needSubDir { get; set; }
        //子文件夹名称
        [DataMember]
        public string subDir { get; set; }
        //使用推荐设置

        public enum _StoreOption
        {
            //不新建文件夹
            [EnumMember]
            useRootDir,
            //根据日期存放
            [EnumMember]
            ByDate,
            //根据星期存放
            [EnumMember]
            ByWeek,
            //根据月份存放
            [EnumMember]
            ByMonth
        }
        [DataMember]
        public _StoreOption StoreOption { get; set; }

        public FileGenarationOption()
        {

        }
    }
}
