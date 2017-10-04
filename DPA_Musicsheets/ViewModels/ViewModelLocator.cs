using DPA_Musicsheets.Managers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DPA_Musicsheets.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<FileHandler>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LilypondViewModel>();
            SimpleIoc.Default.Register<StaffsViewModel>();
            SimpleIoc.Default.Register<MidiPlayerViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public LilypondViewModel LilypondViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LilypondViewModel>();
            }
        }

        public StaffsViewModel StaffsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StaffsViewModel>();
            }
        }

        public MidiPlayerViewModel MidiPlayerViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MidiPlayerViewModel>();
            }
        }

        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<MidiPlayerViewModel>().Cleanup();
        }
    }
}