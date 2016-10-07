using CoreGraphics;
using UIKit;

namespace DateTimeNumberPickerForms.iOS.SourceCode.ModalPicker
{
    public class ModalPickerAnimatedTransitioning : UIViewControllerAnimatedTransitioning
    {
        public bool IsPresenting { get; set; }

        private const float _transitionDuration = 0.25f;

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext)
        {
            return _transitionDuration;
        }

        public override void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var inView = transitionContext.ContainerView;

            var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);
            var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);

            inView.AddSubview(toViewController.View);

            toViewController.View.Frame = CGRect.Empty;

            var startingPoint = GetStartingPoint(fromViewController.InterfaceOrientation);
            if (fromViewController.InterfaceOrientation == UIInterfaceOrientation.Portrait)
            {
                toViewController.View.Frame = new CGRect(startingPoint.X, startingPoint.Y, 
                                                             fromViewController.View.Frame.Width,
                                                             fromViewController.View.Frame.Height);
            }
            else
            {
                toViewController.View.Frame = new CGRect(startingPoint.X, startingPoint.Y, 
                                                             fromViewController.View.Frame.Height,
                                                             fromViewController.View.Frame.Width);
            }

            UIView.AnimateNotify(_transitionDuration,
                                 () =>
            {
                var endingPoint = GetEndingPoint(fromViewController.InterfaceOrientation);
                toViewController.View.Frame = new CGRect(endingPoint.X, endingPoint.Y, fromViewController.View.Frame.Width,
                                                                 fromViewController.View.Frame.Height);
                fromViewController.View.Alpha = 0.5f;
            },
                                 (finished) => transitionContext.CompleteTransition(true)
                                );
        }

        private static CGPoint GetStartingPoint(UIInterfaceOrientation orientation)
        {
            var screenBounds = UIScreen.MainScreen.Bounds;
            var coordinate = CGPoint.Empty;
            switch(orientation)
            {
                case UIInterfaceOrientation.Portrait:
                    coordinate = new CGPoint(0, screenBounds.Height);
                    break;
                case UIInterfaceOrientation.LandscapeLeft:
                    coordinate = new CGPoint(screenBounds.Width, 0);
                    break;
                case UIInterfaceOrientation.LandscapeRight:
                    coordinate = new CGPoint(screenBounds.Width * -1, 0);
                    break;
                default:
                    coordinate = new CGPoint(0, screenBounds.Height);
                    break;
            }

            return coordinate;
        }

        private static CGPoint GetEndingPoint(UIInterfaceOrientation orientation)
        {
            var screenBounds = UIScreen.MainScreen.Bounds;
            var coordinate = CGPoint.Empty;
            switch(orientation)
            {
                case UIInterfaceOrientation.Portrait:
                    coordinate = new CGPoint(0, 0);
                    break;
                case UIInterfaceOrientation.LandscapeLeft:
                    coordinate = new CGPoint(0, 0);
                    break;
                case UIInterfaceOrientation.LandscapeRight:
                    coordinate = new CGPoint(0, 0);
                    break;
                default:
                    coordinate = new CGPoint(0, 0);
                    break;
            }

            return coordinate;
        }
    }
}

