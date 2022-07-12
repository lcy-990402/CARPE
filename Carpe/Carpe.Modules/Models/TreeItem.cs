using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Carpe.Modules.Models
{
    public class TreeItem
    {
        #region Member
        public int? Id { get; set; }

        public int FileId { get; set; }
        public int? ParentId { get; set; }
        public string PartitionId { get; set; }
        public string Name { get; set; }

        public string Path { get; set; }

        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime AccessedTime { get; set; }
        public long LongSize { get; set; }
        public string Size
        {
            get
            {
                if(LongSize <= 0) return string.Empty;
                int cnt = 0;
                double tmp = LongSize;
                while (tmp >= 1024)
                {
                    tmp /= 1024;
                    cnt++;
                }
                return String.Format("{0:F2}", tmp) + Constant.BYTE_UNIT[cnt];
            }
        }
        public string Extension { get; set; }
        public TreeItem ParentItem { get; set; }
        
        public bool IsDirectory { get; set; }
        public ImageSource Source
        {
            get
            {
                if (IsDirectory)
                {
                    var extension = new SvgImageSourceExtension() { Uri = new Uri("pack://application:,,,/DevExpress.Images.v21.2;component/SvgImages/Icon Builder/Actions_FolderClose.svg"), Size = new Size(16, 16) };
                    return (ImageSource)extension.ProvideValue(null);
                }
                else
                {
                    var extension = new SvgImageSourceExtension() { Uri = new Uri("pack://application:,,,/DevExpress.Images.v21.2;component/SvgImages/Icon Builder/Actions_New.svg"), Size = new Size(16, 16) };
                    return (ImageSource)extension.ProvideValue(null);
                }
            }
        }

        public bool IsExpanded { get; set; }
        public bool IsTagged { get; set; }

        public ObservableCollection<TreeItem> SubDirectory { get; set; }
        public ObservableCollection<TreeItem> SubFile { get; set; }
        #endregion

        #region Constructor
        public TreeItem(int? id, int fileId, int? parentId, string partitionId, string name, DateTime createdTime, DateTime modifiedTime, DateTime accessedTime, long size, string path, string extension, TreeItem parentItem, bool isTagged = false, bool isDirectory = false, bool isExpanded = false)
        {
            Id = id;
            FileId = fileId;
            ParentId = parentId;
            PartitionId = partitionId;
            Name = name;
            CreatedTime = createdTime;
            ModifiedTime = modifiedTime;
            AccessedTime = accessedTime;
            LongSize = size;
            Path = path;
            Extension = extension;
            ParentItem = parentItem;
            SubDirectory = new ObservableCollection<TreeItem>();
            SubFile = new ObservableCollection<TreeItem>();
            IsDirectory = isDirectory;
            IsExpanded = isExpanded;
            IsTagged = isTagged;
        }
        #endregion
    }
}
