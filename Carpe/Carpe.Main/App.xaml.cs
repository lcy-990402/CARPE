using Carpe.Common;
using Carpe.Main.Properties;
using Carpe.Main.ViewModels;
using Carpe.Main.Views;
using Carpe.Modules.ViewModels;
using Carpe.Modules.ViewModels.Analyze;
using Carpe.Modules.Views;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ModuleInjection;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using System.ComponentModel;
using System.Windows;
using AppModules = Carpe.Common.Modules;

namespace Carpe.Main
{
    public partial class App : Application
    {
        public App()
        {
            ApplicationThemeHelper.UpdateApplicationThemeName();
            //SplashScreenManager.CreateThemed().ShowOnStartup();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper.Run();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            ApplicationThemeHelper.SaveApplicationThemeName();
            base.OnExit(e);
        }
    }
    public class Bootstrapper
    {
        public static Bootstrapper Default { get; protected set; }
        public static void Run()
        {
            Default = new Bootstrapper();
            Default.RunCore();
        }
        protected Bootstrapper() { }

        const string StateVersion = "1.0";
        public virtual void RunCore()
        {
            ConfigureTypeLocators();
            RegisterModules();
            if (!RestoreState())
                InjectModules();
            ConfigureNavigation();
            ShowMainWindow();
        }

        protected IModuleManager Manager { get { return ModuleManager.DefaultManager; } }
        protected virtual void ConfigureTypeLocators()
        {
            var mainAssembly = typeof(MainViewModel).Assembly;
            var modulesAssembly = typeof(CarpeModuleViewModel).Assembly;
            var assemblies = new[] { mainAssembly, modulesAssembly };
            ViewModelLocator.Default = new ViewModelLocator(assemblies);
            ViewLocator.Default = new ViewLocator(assemblies);
        }
        protected virtual void RegisterModules()
        {
            Manager.GetRegion(Regions.Documents).VisualSerializationMode = VisualSerializationMode.PerKey;
            Manager.Register(Regions.MainWindow, new Module(AppModules.Main, MainViewModel.Create, typeof(MainView)));
            Manager.Register(Regions.MainWindow, new Module("StartUp", StartUpViewModel.Create, typeof(StartUpView)));
        }
        protected virtual bool RestoreState()
        {
#if !DEBUG
            if (Settings.Default.StateVersion != StateVersion) return false;
            return Manager.Restore(Settings.Default.LogicalState, Settings.Default.VisualState);
#else
            return false;
#endif
        }
        protected virtual void InjectModules()
        {
            Manager.Inject(Regions.MainWindow, "StartUp");
            Manager.Inject(Regions.MainWindow, AppModules.Main);           
        }
        protected virtual void ConfigureNavigation()
        {
            Manager.GetEvents(Regions.MainWindow).Navigation += OnMainWindowNavigation;
            Manager.GetEvents(Regions.Overview).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Filesystem).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Modules).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Documents).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Navigation).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Report).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Tags).Navigation += OnNavigation;
            Manager.GetEvents(Regions.Visualization).Navigation += OnNavigation;
            
        }
        protected virtual void ShowMainWindow()
        {
            App.Current.MainWindow = new MainWindow();
            App.Current.MainWindow.Show();
            App.Current.MainWindow.Closing += OnClosing;
        }
        void OnNavigation(object sender, NavigationEventArgs e)
        {
            if (e.NewViewModelKey == null) return;
            Manager.InjectOrNavigate(Regions.Documents, e.NewViewModelKey);
        }
        void OnMainWindowNavigation(object sender, NavigationEventArgs e)
        {
            Manager.Navigate(Regions.MainWindow, e.NewViewModelKey);
        }
        void OnClosing(object sender, CancelEventArgs e)
        {
            string logicalState;
            string visualState;
            //Manager.Save(out logicalState, out visualState);
            //Settings.Default.StateVersion = StateVersion;
            //Settings.Default.LogicalState = logicalState;
            //Settings.Default.VisualState = visualState;
            //Settings.Default.Save();
        }
    }
}