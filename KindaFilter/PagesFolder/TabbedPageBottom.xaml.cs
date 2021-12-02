using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageBottom : TabbedPage
    {
        public TabbedPageBottom()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }


    }
}