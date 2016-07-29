using System;

using Xamarin.Forms;

namespace MasterDetailTest
{
	public class MDTest : MasterDetailPage
	{
		public MDTest() {
			base.Icon = "";

			var sl = new StackLayout { 
				Children = { 
					new Label { Text = "MD master" }
				}
			};
			base.Master = new ContentPage { Icon = "hamburger", Title = "MD", Content = sl };

			var sl2 = new StackLayout {
				Children = {
					new Label { Text = "MD detail" }
				}
			};
			base.Detail = new ContentPage { Title = "Detail", Content = sl2 };
		}
	}

	public class App : Application
	{
		public App()
		{
			var page = new MDTest();
			MainPage = page;
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

