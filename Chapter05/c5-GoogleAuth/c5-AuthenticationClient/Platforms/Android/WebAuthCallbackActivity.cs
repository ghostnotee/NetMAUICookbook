using Android.App;
using Android.Content;
using Android.Content.PM;

namespace c5_AuthenticationClient;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter([Intent.ActionView], Categories = [Intent.CategoryDefault, Intent.CategoryBrowsable], DataScheme = CallbackScheme)]
public class WebAuthCallbackActivity : WebAuthenticatorCallbackActivity
{
    private const string CallbackScheme = "myapp";
}