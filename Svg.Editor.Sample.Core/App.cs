using MvvmCross.Platform.IoC;

namespace Svg.Droid.SampleEditor.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.EditorViewModel>();
        }
    }
}
