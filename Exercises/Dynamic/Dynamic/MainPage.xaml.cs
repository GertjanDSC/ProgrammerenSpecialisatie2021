using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dynamic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DynamicMethod();
        }

        private static void DynamicMethod()
        {
            object obj = "Mosh";
            var hc = obj.GetHashCode();
            var methodInfo = obj.GetType().GetMethod("GetHashCode");
            methodInfo.Invoke(null, null);

            // DLR on top of CLR: magic
            dynamic name = "Luc";
            name = 10; // ok, not an exception! Different runtime types ...
            // name++; // generates exception though
            // JIT:
            dynamic a = 10;
            dynamic b = 5;
            var x = ""; // var is not dynamic
            var c = a + b; // c is dynamic
            int i = 5;
            dynamic d = i;
            long l = d; // implicit casting
        }
    }
}
