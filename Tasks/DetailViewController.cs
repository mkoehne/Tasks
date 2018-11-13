using System;
using Cirrious.FluentLayouts.Touch;
using Tasks.Models;
using UIKit;

namespace Tasks
{
    public partial class DetailViewController : UIViewController
    {
        UILabel detailDescriptionLabel;
        UILabel createdAtLabel;
        UIImageView imageView;
        UIButton button;

        public Task DetailItem { get; set; }

        protected DetailViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        void SetupView()
        {
            detailDescriptionLabel = new UILabel();
            detailDescriptionLabel.TextColor = UIColor.Black;
            detailDescriptionLabel.Font = UIFont.SystemFontOfSize(26);
            detailDescriptionLabel.Text = "Description";
            detailDescriptionLabel.TextAlignment = UITextAlignment.Center;
            this.View.Add(detailDescriptionLabel);

            createdAtLabel = new UILabel();
            createdAtLabel.TextColor = UIColor.Black;
            createdAtLabel.Font = UIFont.SystemFontOfSize(16);
            createdAtLabel.Text = "Date";
            createdAtLabel.Lines = 0;
            createdAtLabel.TextAlignment = UITextAlignment.Center;
            this.View.Add(createdAtLabel);

            imageView = new UIImageView(UIImage.FromFile("Checkmark.png"));
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            this.View.Add(imageView);

            button = new UIButton(UIButtonType.System);
            button.SetTitle("Mark as done", UIControlState.Normal);
            button.TouchUpInside += Button_TouchUpInside;
            this.View.Add(button);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            View.AddConstraints(
                detailDescriptionLabel.AtTopOf(View, 20),
                detailDescriptionLabel.AtLeftOf(View, 20),
                detailDescriptionLabel.Height().EqualTo(44),
                detailDescriptionLabel.WithSameWidth(this.View).Minus(40),

                createdAtLabel.Below(detailDescriptionLabel),
                createdAtLabel.AtLeftOf(View, 20),
                createdAtLabel.WithSameWidth(this.View).Minus(40),

                imageView.Below(createdAtLabel, 20),
                imageView.WithSameCenterX(View),
                imageView.Height().EqualTo(44),
                imageView.Width().EqualTo(44),

                button.Below(imageView),
                button.WithSameCenterX(View),
                button.Height().EqualTo(44),
                button.WithSameWidth(this.View).Minus(40)
            );
        }

        public void SetDetailItem(Task newDetailItem)
        {
            if (DetailItem != newDetailItem)
            {
                DetailItem = newDetailItem;

                // Update the view
                ConfigureView();
            }
        }

        void ConfigureView()
        {
            // Update the user interface for the detail item
            if (IsViewLoaded && DetailItem != null)
            {
                detailDescriptionLabel.Text = DetailItem.Name;
                var createdAt = new DateTime(DetailItem.CreatedAt.Ticks);
                createdAtLabel.Text = createdAt.ToLongDateString() + " \n" + createdAt.ToLongTimeString();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            SetupView();
            ConfigureView();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        void Button_TouchUpInside(object sender, EventArgs e)
        {
            //Button was clicked
            if (button.Selected)
            {
                button.Selected = !button.Selected;
                button.SetTitle("Mark as done", UIControlState.Normal);
            }
            else
            {
                button.Selected = !button.Selected;
                button.SetTitle("Mark as not done", UIControlState.Normal);
            }

        }
    }
}

