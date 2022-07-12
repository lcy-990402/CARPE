using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpe.Modules.Models
{
    public class Constant
    {
        #region Member
        public static ReadOnlyCollection<string> BYTE_UNIT = new List<string>
        {
            "B", "KB", "MB", "GB", "TB", "PB"
        }.AsReadOnly();
        #endregion
    }
}
