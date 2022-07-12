using Carpe.Modules.Models;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Carpe.Modules.Selectors
{
    public class SolutionNodeImageSelector : TreeListNodeImageSelector
    {
        #region Constructor
        #endregion

        #region Method

        public override ImageSource Select(TreeListRowData rowData)
        {
            TreeItem treeItem = (rowData.Row as TreeItem);
            if (treeItem == null) return null;
            if (!treeItem.IsExpanded)
            {
                var extension = new SvgImageSourceExtension() { Uri = new Uri("pack://application:,,,/DevExpress.Images.v21.2;component/SvgImages/Icon Builder/Actions_FolderClose.svg"), Size = new Size(16, 16) };
                return (ImageSource)extension.ProvideValue(null);
            }
            else
            {
                var extension = new SvgImageSourceExtension() { Uri = new Uri("pack://application:,,,/DevExpress.Images.v21.2;component/SvgImages/Icon Builder/Actions_FolderOpen.svg"), Size = new Size(16, 16) };
                return (ImageSource)extension.ProvideValue(null);
            }
        }

        #endregion

    }
}
