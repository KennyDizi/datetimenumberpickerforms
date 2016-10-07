using System;
using System.Windows.Input;
using ReactiveUI;
using Xamarin.Forms;

namespace DateTimeNumberPickerForms.SourceCode
{
    public class PickerPageViewModel : ReactiveObject
    {
        public PickerPageViewModel()
        {
            ShowDatePikcerCommand  = new Command(ShowDatePikcerAction);
            ShowTimePickerCommand  = new Command(ShowTimePickerAction);
            ShowNumberPickerCommand = new Command(ShowNumberPickerAction);
        }

        #region commands

        public ICommand ShowDatePikcerCommand { get; }

        private void ShowDatePikcerAction()
        {
            var now = DateTime.Now;
            var minDate = DateTime.Now.AddYears(-3);
            var maxDate = DateTime.Now.AddYears(5);

            CrossXDateTimeNumberPicker.Current.ShowDatePicker(now, minDate, maxDate, "Choose a date", selectedDate =>
            {
                SelectedDate = $"Date selected is: {selectedDate:yy-MM-dd}";
            });
        }


        public ICommand ShowTimePickerCommand { get; }

        private void ShowTimePickerAction()
        {
            CrossXDateTimeNumberPicker.Current.ShowTimePicker("Choose a time", DateTime.Now, seletedTime =>
            {
                SelectedDate = $"Time selected is: {seletedTime.Hour}:{seletedTime.Minute}";
            });
        }

        public ICommand ShowNumberPickerCommand { get; }

        private void ShowNumberPickerAction()
        {
            CrossXDateTimeNumberPicker.Current.ShowNumberPicker("Choose a number", 69, 0, 96, selectedNum =>
            {
                SelectedNumber = $"Number selected is: {selectedNum}";
            });
        }

        #endregion

        #region properties

        private string _selectedDate;

        public string SelectedDate
        {
            get { return _selectedDate; }
            set { this.RaiseAndSetIfChanged(ref _selectedDate, value); }
        }

        private string _selectedTime;

        public string SelectedTime
        {
            get { return _selectedTime; }
            set { this.RaiseAndSetIfChanged(ref _selectedTime, value); }
        }

        private string _selectedNumber;

        public string SelectedNumber
        {
            get { return _selectedNumber; }
            set { this.RaiseAndSetIfChanged(ref _selectedNumber, value); }
        }

        #endregion
    }
}
