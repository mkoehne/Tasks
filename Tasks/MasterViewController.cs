using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using Tasks.Models;
using Realms;
using System.Linq;

namespace Tasks
{
    public partial class MasterViewController : UITableViewController
    {
        public DetailViewController DetailViewController { get; set; }

        DataSource dataSource;

        protected MasterViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Title = "Aufgaben";

            // Perform any additional setup after loading the view, typically from a nib.
            NavigationItem.LeftBarButtonItem = EditButtonItem;

            var addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, AddNewItem);
            addButton.AccessibilityLabel = "addButton";
            NavigationItem.RightBarButtonItem = addButton;

            DetailViewController = (DetailViewController)((UINavigationController)SplitViewController.ViewControllers[1]).TopViewController;
            var realm = Realm.GetInstance();
            var tasks = realm.All<Task>().ToList();
            dataSource = new DataSource(this);
            dataSource.Tasks = tasks;
            TableView.Source = dataSource;

            var token = realm.All<Task>().SubscribeForNotifications((sender, changes, error) =>
            {
                realm = Realm.GetInstance();
                tasks = realm.All<Task>().ToList();
                ((DataSource)TableView.Source).Tasks = tasks;
                TableView.ReloadData();
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            ClearsSelectionOnViewWillAppear = SplitViewController.Collapsed;
            base.ViewWillAppear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        void AddNewItem(object sender, EventArgs args)
        {
            UIAlertView alert = new UIAlertView("Erstelle eine neue Aufgabe", "", null, "Abbrechen", "Ok");
            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            UITextField alertText = alert.GetTextField(0);
            alertText.KeyboardType = UIKeyboardType.Default;
            alertText.Placeholder = "Aufgabe";
            alert.Show();
            alert.Clicked += (object sender1, UIButtonEventArgs e) =>
            {
                if (e.ButtonIndex == 1)
                {
                    var realm = Realm.GetInstance();

                    realm.Write(() =>
                    {
                        Task newTask = new Task();
                        newTask.Id = Guid.NewGuid().ToString();
                        newTask.Name = alertText.Text;
                        newTask.CreatedAt = DateTimeOffset.Now;

                        realm.Add(newTask);
                    });
                }
            };
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "showDetail")
            {
                var controller = (DetailViewController)((UINavigationController)segue.DestinationViewController).TopViewController;
                var indexPath = TableView.IndexPathForSelectedRow;
                var item = dataSource.Tasks[indexPath.Row];

                controller.SetDetailItem(item);
                controller.NavigationItem.LeftBarButtonItem = SplitViewController.DisplayModeButtonItem;
                controller.NavigationItem.LeftItemsSupplementBackButton = true;
            }
        }

        class DataSource : UITableViewSource
        {
            static readonly NSString CellIdentifier = new NSString("Cell");
            List<Task> tasks = new List<Task>();
            readonly MasterViewController controller;

            public DataSource(MasterViewController controller)
            {
                this.controller = controller;
            }

            public List<Task> Tasks
            {
                get { return tasks; }
                set
                {
                    tasks = value;
                }
            }

            // Customize the number of sections in the table view.
            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return tasks.Count;
            }

            // Customize the appearance of table view cells.
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(CellIdentifier, indexPath);

                cell.TextLabel.Text = tasks[indexPath.Row].Name;

                return cell;
            }

            public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
            {
                // Return false if you do not want the specified item to be editable.
                return true;
            }

            public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                if (editingStyle == UITableViewCellEditingStyle.Delete)
                {
                    // Delete the row from realm.
                    var task = Tasks.ElementAt(indexPath.Row);

                    var realm = Realm.GetInstance();
                    realm.Write(() =>
                    {
                        var item = realm.Find<Task>(Tasks.ElementAt(indexPath.Row).Id);
                        realm.Remove(item);
                    });
                }
                else if (editingStyle == UITableViewCellEditingStyle.Insert)
                {
                    // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
                }
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                    controller.DetailViewController.SetDetailItem(tasks[indexPath.Row]);
            }
        }
    }
}
