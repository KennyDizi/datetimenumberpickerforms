using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using DateTimeNumberPickerForms.Droid.SourceCode.DPServices;
using DateTimeNumberPickerForms.SourceCode;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatePickerInterface))]
namespace DateTimeNumberPickerForms.Droid.SourceCode.DPServices
{
    public class DatePickerInterface : Fragment, IDatePickerInterface, NumberPicker.IOnValueChangeListener
    {
        private Action<int> _onNumChangedCallback;
        private Action<DateTime> _onDateTimeChangedCallback;
        public void ShowDatePicker(DateTime currentDateTime, DateTime mindate, DateTime maxdate, string message, Action<DateTime> onDateTimeChangedCallback = null)
        {
            _onDateTimeChangedCallback = onDateTimeChangedCallback;
            var c = CrossCurrentActivity.Current.Activity;
            var today = DateTime.Today;
            var dialog = new DatePickerDialog(c, DateChangeCallBack, today.Year, today.Month - 1, today.Day);
            dialog.SetCanceledOnTouchOutside(false);
            dialog.DatePicker.SetMinDate(mindate);
            dialog.DatePicker.SetMaxDate(maxdate);
            
            dialog.UpdateDate(currentDateTime);
            dialog.Show();
        }

        private void DateChangeCallBack(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            SelectedDate = e.Date;
            DateChanged?.Invoke(this, new SelectedItemChangedEventArgs(SelectedDate));
            _onDateTimeChangedCallback?.Invoke(SelectedDate);
        }

        public void ShowTimePicker(string message, DateTime currentDateTime, Action<DateTime> onDateTimeChangedCallback = null)
        {
            _onDateTimeChangedCallback = onDateTimeChangedCallback;
            var c = CrossCurrentActivity.Current.Activity;
            var dialog = new TimePickerDialog(c, TimeChangeCallBack, currentDateTime.Hour, currentDateTime.Minute, true);
            dialog.Show();
        }

        public void ShowNumberPicker(string message, int currentNum, int minNum, int maxNum, Action<int> actionNumChangedCallback)
        {
            _onNumChangedCallback = actionNumChangedCallback;
            var numDialog = new NumberPickerDialogFragment(CrossCurrentActivity.Current.Activity, minNum, maxNum, currentNum, message, this);
            numDialog.Dialog.SetCanceledOnTouchOutside(false);
            numDialog.Show(CrossCurrentActivity.Current.Activity.FragmentManager, message);
        }
        
        public void OnValueChange(NumberPicker picker, int oldVal, int newVal)
        {
            _onNumChangedCallback?.Invoke(newVal);
            NumberChanged?.Invoke(this, new SelectedItemChangedEventArgs(newVal));
        }

        private void TimeChangeCallBack(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            var selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, e.HourOfDay, e.Minute, 0);
            SelectedDate = selectedDate;
            TimeChanged?.Invoke(this, new SelectedItemChangedEventArgs(SelectedDate));
            _onDateTimeChangedCallback?.Invoke(SelectedDate);
        }

        public DateTime SelectedDate { get; set; }
        public int SelectedNumber { get; set; }
        public event EventHandler<SelectedItemChangedEventArgs> DateChanged;
        public event EventHandler<SelectedItemChangedEventArgs> TimeChanged;
        public event EventHandler<SelectedItemChangedEventArgs> NumberChanged;
    }

    public class NumberPickerDialogFragment : DialogFragment
    {
        private readonly Context _context;
        private readonly int _min, _max, _current;
        private readonly NumberPicker.IOnValueChangeListener _listener;
        private readonly string _numPickerTitle;

        public NumberPickerDialogFragment(Context context, int min, int max, int current, string numPickerTitle, NumberPicker.IOnValueChangeListener listener)
        {
            _context = context;
            _min = min;
            _max = max;
            _current = current;
            _listener = listener;
            _numPickerTitle = numPickerTitle;
        }

        public override Dialog OnCreateDialog(Bundle savedState)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.numberpickerdialog, null);
            var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
            numberPicker.MaxValue = _max;
            numberPicker.MinValue = _min;
            numberPicker.Value = _current;
            numberPicker.SetOnValueChangedListener(_listener);

            var dialog = new AlertDialog.Builder(_context);
            dialog.SetTitle(_numPickerTitle);
            dialog.SetView(view);
            dialog.SetNegativeButton(Resource.String.dialog_cancel, (s, a) => { });
            dialog.SetPositiveButton(Resource.String.dialog_ok, (s, a) => { });
            return dialog.Create();
        }
    }

    public static class Extensions
    {
        public static void SetMinDate(this Android.Widget.DatePicker picker, DateTime dt)
        {
            var javaMinDt = new DateTime(1970, 1, 1);
            if (dt.CompareTo(javaMinDt) < 0)
#if DEBUG
                throw new ArgumentException("Must be >= Java's min DateTime of 1/1970");
#else
            return;
#endif

            var longVal = dt - javaMinDt;
            picker.MinDate = (long)longVal.TotalMilliseconds;
        }

        public static void SetMaxDate(this Android.Widget.DatePicker picker, DateTime dt)
        {
            var javaMinDt = new DateTime(1970, 1, 1);
            if (dt.CompareTo(javaMinDt) < 0)
#if DEBUG
                throw new ArgumentException("Must be >= Java's min DateTime of 1/1970");
#else
            return;
#endif

            var longVal = dt - javaMinDt;
            picker.MaxDate = (long)longVal.TotalMilliseconds;
        }
    }
}