using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Notifications;
using RememberMe.Ui.WPF.Services;

namespace RememberMe.Ui.WPF.Components;

public partial class Administration : ComponentBase
{
    
    [Inject] public required ILogger<Administration> Logger { get; set; }
    [Inject] ConfigDataContext ConfigDataContext { get; set; }
    
    private bool _debugExpanded;

    private void SendToast()
    {
        new ToastContentBuilder()
            .AddText("Hello World!")
            .AddText("A Message from your App")
            .AddButton("Acknowledged", ToastActivationType.Background, "ButtonTest")
            .Show();
    }
}