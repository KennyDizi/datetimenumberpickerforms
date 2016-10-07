using System;
using System.Collections.Generic;
using CoreGraphics;
using DateTimeNumberPickerForms.iOS.SourceCode.DPServices;
using DateTimeNumberPickerForms.iOS.SourceCode.ModalPicker;
using DateTimeNumberPickerForms.SourceCode;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(DatePickerInterface))]
namespace DateTimeNumberPickerForms.iOS.SourceCode.DPServices
{
    public class DatePickerInterface : IDatePickerInterface
    {
        public void ShowDatePicker(DateTime currentDateTime, DateTime mindate, DateTime maxdate, string message, Action<DateTime> onDateTimeChangedCallback = null)
        {
            var modalPicker = new ModalPickerViewController(ModalPickerType.Date, message,
                UIApplication.SharedApplication.KeyWindow.RootViewController)
            {
                HeaderBackgroundColor = UIColor.FromRGB(25, 118, 210),
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom,
                DatePicker =
                {
                    Mode = UIDatePickerMode.Date,
                    MaximumDate = DateTimeToNsDate(maxdate),
                    MinimumDate = DateTimeToNsDate(mindate),
                    Date = DateTimeToNsDate(currentDateTime),
                    TimeZone = new NSTimeZone("UTC"),
                    Locale = NSLocale.FromLocaleIdentifier("NL")
                }
            };

            modalPicker.OnModalPickerDismissed += (s, ea) =>
            {
                SelectedDate = NsDateToDateTime(modalPicker.DatePicker.Date);
                DateChanged?.Invoke(this, new SelectedItemChangedEventArgs(SelectedDate));
                onDateTimeChangedCallback?.Invoke(SelectedDate);
            };

            //show
            modalPicker.PresentUsingRootViewController();
        }

        public void ShowTimePicker(string message, DateTime currentDateTime, Action<DateTime> onDateTimeChangedCallback = null)
        {
            var modalPicker = new ModalPickerViewController(ModalPickerType.Date, message,
                UIApplication.SharedApplication.KeyWindow.RootViewController)
            {
                HeaderBackgroundColor = UIColor.FromRGB(25, 118, 210),
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom,
                DatePicker = new UIDatePicker(CGRect.Empty)
                {
                    Mode = UIDatePickerMode.Time,
                    Date = currentDateTime.ToNSDate(),
                    TimeZone = new NSTimeZone("UTC"),
                    Locale = NSLocale.FromLocaleIdentifier("NL")
                }
            };

            modalPicker.OnModalPickerDismissed += (s, ea) =>
            {
                var datetime = modalPicker.DatePicker.Date.ToDateTime() - new DateTime(1, 1, 1);
                SelectedDate = new DateTime(1970, 1, 1).AddSeconds(datetime.TotalSeconds);
                TimeChanged?.Invoke(this, new SelectedItemChangedEventArgs(SelectedDate));
                onDateTimeChangedCallback?.Invoke(SelectedDate);
            };

            //show
            modalPicker.PresentUsingRootViewController();
        }

        public void ShowNumberPicker(string message, int currentNum, int minNum, int maxNum, Action<int> actionNumChangedCallback)
        {
            //init list number
            var customNumbersList = new List<string>();
            for(var i = minNum; i <= maxNum; i++)
            {
                customNumbersList.Add(i.ToString());
            }

            var cindex = customNumbersList.IndexOf(currentNum.ToString());

            //Create the modal picker and style it as you see fit
            var modalPicker = new ModalPickerViewController(ModalPickerType.Custom, message,
                UIApplication.SharedApplication.KeyWindow.RootViewController)
            {
                HeaderBackgroundColor = UIColor.FromRGB(25, 118, 210),
                HeaderTextColor = UIColor.White,
                TransitioningDelegate = new ModalPickerTransitionDelegate(),
                ModalPresentationStyle = UIModalPresentationStyle.Custom,
                PickerView = { Model = new CustomPickerModel(customNumbersList) }
            };

            if (cindex >= 0)
                modalPicker.PickerView.Select(cindex, 0, true);

            //Create the model for the Picker View
            modalPicker.OnModalPickerDismissed += (s, ea) =>
            {
                var index = modalPicker.PickerView.SelectedRowInComponent(0);
                var numberStringSelected = customNumbersList[(int)index];

                SelectedNumber = numberStringSelected.StringToInt();
                NumberChanged?.Invoke(this, new SelectedItemChangedEventArgs(SelectedNumber));

                //call action callback
                actionNumChangedCallback?.Invoke(SelectedNumber);
            };

            //show
            modalPicker.PresentUsingRootViewController();
        }

        public static DateTime NsDateToDateTime(NSDate date)
        {
            var reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }
         
        public static NSDate DateTimeToNsDate(DateTime date)
        {
            var reference = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - reference).TotalSeconds);
        }

        public DateTime SelectedDate { get; set; }
        public int SelectedNumber { get; set; }
        public event EventHandler<SelectedItemChangedEventArgs> DateChanged;
        public event EventHandler<SelectedItemChangedEventArgs> TimeChanged;
        public event EventHandler<SelectedItemChangedEventArgs> NumberChanged;
    }
}
