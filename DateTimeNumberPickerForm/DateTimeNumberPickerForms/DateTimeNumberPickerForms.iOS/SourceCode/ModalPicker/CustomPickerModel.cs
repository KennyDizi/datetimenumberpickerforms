using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace DateTimeNumberPickerForms.iOS.SourceCode.ModalPicker
{
    public class CustomPickerModel : UIPickerViewModel
    {
        private readonly List<string> _itemsList;

        public CustomPickerModel(List<string> itemsList)
        {
            _itemsList = itemsList;
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return _itemsList.Count;
        }

        public override UIView GetView(UIPickerView picker, nint row, nint component, UIView view)
        {
            var label = new UILabel(new CGRect(0, 0, 300, 37))
            {
                BackgroundColor = UIColor.Clear,
                Text = _itemsList[(int)row],
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.BoldSystemFontOfSize(22.0f)
            };

            return label;
        }
    }
}

