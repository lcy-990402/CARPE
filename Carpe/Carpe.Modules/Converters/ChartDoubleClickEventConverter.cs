using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Carpe.Modules.Converters
{
    public class ChartDoubleClickEventConverter : EventArgsConverterBase<MouseButtonEventArgs>
    {
        protected override object Convert(object sender, MouseButtonEventArgs args)
        {
            ChartControl chartControl = (ChartControl)args.Source;
            var hitInfo = chartControl.CalcHitInfo(args.GetPosition(chartControl));
            return hitInfo.SeriesPoint;
            
        }
    }
}
