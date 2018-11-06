// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Tasks
{
    [Register ("DetailViewController")]
    partial class DetailViewController
    {
        [Outlet]
        UIKit.UILabel createdAtLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel detailDescriptionLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (createdAtLabel != null) {
                createdAtLabel.Dispose ();
                createdAtLabel = null;
            }

            if (detailDescriptionLabel != null) {
                detailDescriptionLabel.Dispose ();
                detailDescriptionLabel = null;
            }
        }
    }
}