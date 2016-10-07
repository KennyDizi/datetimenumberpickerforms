using UIKit;

namespace DateTimeNumberPickerForms.iOS.SourceCode.ModalPicker
{
    public class ModalPickerTransitionDelegate : UIViewControllerTransitioningDelegate
    {
        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(UIViewController presented, UIViewController presenting, UIViewController source)
        {
            var controller = new ModalPickerAnimatedTransitioning {IsPresenting = true};

            return controller;
        }

        public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController(UIViewController dismissed)
        {
            var controller = new ModalPickerAnimatedDismissed {IsPresenting = false};

            return controller;
        }

        public override IUIViewControllerInteractiveTransitioning GetInteractionControllerForPresentation(IUIViewControllerAnimatedTransitioning animator)
        {
            return null;
        }

        public override IUIViewControllerInteractiveTransitioning GetInteractionControllerForDismissal(IUIViewControllerAnimatedTransitioning animator)
        {
            return null;
        }
    }
}

