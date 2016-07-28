using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CS.MasterPage.XamarinForm.Sample
{
	public class MasterPageItem
	{
		public string Title { get; set; }

		public string IconSource { get; set; }

		public Type TargetType { get; set; }
	}

	public class ContactsPageCS : ContentPage
	{
		public ContactsPageCS()
		{
			Title = "Contacts Page";
			Content = new StackLayout
			{
				Children = {
					new Label {
						Text = "Contacts data goes here",
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.CenterAndExpand
					}
				}
			};
		}
	}

	public class TodoListPageCS : ContentPage
	{
		public TodoListPageCS()
		{
			Title = "TodoList Page";
			Content = new StackLayout
			{
				Children = {
					new Label {
						Text = "Todo list data goes here",
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.CenterAndExpand
					}
				}
			};
		}
	}

	public class ReminderPageCS : ContentPage
	{
		public ReminderPageCS()
		{
			Title = "Reminder Page";
			Content = new StackLayout
			{
				Children = {
					new Label {
						Text = "Reminder data goes here",
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.CenterAndExpand
					}
				}
			};
		}
	}

	public class MasterPageCS : ContentPage
	{
		public ListView ListView { get { return listView; } }

		ListView listView;

		public MasterPageCS()
		{
			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Contacts",
				IconSource = "contacts.png",
				TargetType = typeof(ContactsPageCS)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "TodoList",
				IconSource = "todo.png",
				TargetType = typeof(TodoListPageCS)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Reminders",
				IconSource = "reminders.png",
				TargetType = typeof(ReminderPageCS)
			});

			listView = new ListView
			{
				ItemsSource = masterPageItems,
				ItemTemplate = new DataTemplate(() =>
				{
					var imageCell = new ImageCell();
					imageCell.SetBinding(TextCell.TextProperty, "Title");
					imageCell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
					return imageCell;
				}),
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None
			};

			Padding = new Thickness(0, 40, 0, 0);
			Icon = "hamburger.png";
			Title = "Personal Organiser";
			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
				listView
			}
			};
		}
	}

	public class MainPageCS : MasterDetailPage
	{
		MasterPageCS masterPage;

		public MainPageCS()
		{
			masterPage = new MasterPageCS();
			Master = masterPage;
			Detail = new NavigationPage(new ContactsPageCS());

			masterPage.ListView.ItemSelected += OnItemSelected;

			if (Device.OS == TargetPlatform.Windows)
			{
				Master.Icon = "swap.png";
			}
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
				masterPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}
	}

	public class App : Application
	{
		public App()
		{
			base.MainPage = new MainPageCS();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

