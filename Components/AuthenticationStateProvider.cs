using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using HRJobPortal.Data.Services;

namespace HRJobPortal.Components
{
    public class AuthenticationStateProvider : ComponentBase
    {
        [Inject]
        protected AuthService AuthService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            if (!AuthService.IsAuthenticated())
            {
                NavigationManager.NavigateTo("/login");
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && !AuthService.IsAuthenticated())
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
} 