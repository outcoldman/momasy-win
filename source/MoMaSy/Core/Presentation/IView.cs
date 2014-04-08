using System.Windows.Threading;
namespace outcold.MoMaSy.Core.Presentation
{
    internal interface IView
    {
        object DataContext { get; set; }
        bool IsBusy { get; set; }
        bool IsInDesignTool { get; }

        Dispatcher Dispatcher { get; }
    }
}
