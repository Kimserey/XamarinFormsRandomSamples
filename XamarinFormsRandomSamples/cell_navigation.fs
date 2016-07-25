open System
open Xamarin.Forms
open System.ComponentModel

module Extensions =
    type Label with
        static member SetBinding' (name: string) (x: Label) =
            x.SetBinding(Label.TextProperty, name)
            x

open Extensions

type PageType =
    | Monthly
    | Category

type Category = {
    Title: string
    Sum: float
} 

type ViewModelBase() =
    let propertyChanged  = new Event<PropertyChangedEventHandler, PropertyChangedEventArgs>()

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member self.PropertyChanged = propertyChanged.Publish

    member self.OnPropertyChanged name =
        propertyChanged.Trigger(self, new PropertyChangedEventArgs(name))


type PageViewModel() =
    inherit ViewModelBase()

    let mutable _title = "Not set"

    member self.Title 
        with get() = _title
    
    member self.Title 
        with set title =
            _title <- title
            self.OnPropertyChanged "Title"

type Page() as self =
    inherit MasterDetailPage(IsGestureEnabled = false, IsPresented = false, MasterBehavior = MasterBehavior.Popover)
    
    do
        self.Master <- 
            new ContentPage(Title = "Master page", Content = new StackLayout())
        
        self.Detail <- 
            let viewmodel = new PageViewModel()
            
            let button = new Button(Text = "Change title")
            button.Clicked.Add(fun _ -> viewmodel.Title <- "Hello world")
            let layout = new StackLayout()
            layout.Children.Add(button)

            let page = new ContentPage(Title = "Detail page", Content = layout)
            page.BindingContext <- viewmodel
            page.SetBinding(Page.TitleProperty, "Title")
            new NavigationPage(page)


type CategoryCell() as self =
    inherit ViewCell()

    do 
        let layout = 
            new StackLayout(Padding = new Thickness(5.), Orientation = StackOrientation.Horizontal)
        
        layout.Children.Add(
            new Label()
            |> Label.SetBinding' "Title"
        )
        layout.Children.Add(
            new Label()
            |> Label.SetBinding' "Sum"
        )

        self.View <- layout

type CategoryPage(title) =
    inherit ContentPage(Title = title)


type ListCategoryPage(title, openCategory) as self =
    inherit ContentPage(Title = title)

    let categories =
        [| { Title = "Supermarket"; Sum = 100.05 }
           { Title = "Other"; Sum = 204.05 } |]

    let listView = new ListView(ItemsSource = categories, ItemTemplate = new DataTemplate(typeof<CategoryCell>))

    do
        listView.ItemTapped.Add(fun (e: ItemTappedEventArgs) -> openCategory (e.Item :?> Category))
        self.Content <- listView

type App() = 
    inherit Application()

    do
        let navPage = new NavigationPage()
        base.MainPage <- navPage 

        let listCategoryPage =
            new ListCategoryPage("Some list", (fun c -> navPage.PushAsync(new CategoryPage(c.Title)) |> Async.AwaitTask |> Async.StartImmediate ))

        navPage.PushAsync(listCategoryPage) |> Async.AwaitTask |> Async.StartImmediate
        
