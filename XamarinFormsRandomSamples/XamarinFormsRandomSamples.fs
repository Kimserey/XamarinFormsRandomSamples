namespace XamarinFormsRandomSamples

open System.ComponentModel
open Xamarin.Forms

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

type App() = 
    inherit Application()

    do 
        let viewmodel = new PageViewModel()
            
        let button = new Button(Text = "Change title")
        button.Clicked.Add(fun _ -> viewmodel.Title <- "Hello world")
        let layout = new StackLayout()
        layout.Children.Add(button)

        let page = new ContentPage(Title = "Detail page", Content =layout)
        page.BindingContext <- viewmodel
        page.SetBinding(Page.TitleProperty, "Title")

        base.MainPage <- new NavigationPage(page)
