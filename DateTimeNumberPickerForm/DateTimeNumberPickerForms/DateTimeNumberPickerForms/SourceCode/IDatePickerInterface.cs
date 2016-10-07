using System;
using Xamarin.Forms;

namespace DateTimeNumberPickerForms.SourceCode
{
    public interface IDatePickerInterface
    {
        void ShowDatePicker(DateTime currentDateTime, DateTime mindate, DateTime maxdate, string message, Action<DateTime> onDateTimeChangedCallback = null);
        void ShowTimePicker(string message, DateTime currentDateTime, Action<DateTime> onDateTimeChangedCallback = null);
        void ShowNumberPicker(string message, int currentNum, int minNum, int maxNum, Action<int> actionNumChangedCallback);
        DateTime SelectedDate { get; set; }
        int SelectedNumber { get; set; }
        event EventHandler<SelectedItemChangedEventArgs> DateChanged;
        event EventHandler<SelectedItemChangedEventArgs> TimeChanged;
        event EventHandler<SelectedItemChangedEventArgs> NumberChanged;
    }

    public class CrossXDateTimeNumberPicker
    {
        private static readonly Lazy<IDatePickerInterface> Implementation = new Lazy<IDatePickerInterface>(CreateMedia,
            System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IDatePickerInterface Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
#if DEBUG
                    throw NotImplementedInReferenceAssembly();
#endif
                }
                return ret;
            }
        }

        private static IDatePickerInterface CreateMedia()
        {
#if PORTABLE
            return null;
#else
            return DependencyService.Get<IDatePickerInterface>();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
