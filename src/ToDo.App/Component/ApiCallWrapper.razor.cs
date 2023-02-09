using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace ToDo.App.Component;

public partial class ApiCallWrapper<TService>
{
    [Inject] public TService Service { get; private set; }
    [Parameter] public EventCallback<Exception> OnException { get; set; }
    
    public bool IsLoading { get; set; }

    public async Task Execute(Func<TService, Task> action)
    {
        IsLoading = true;

        try
        {
            await action(Service);
        }
        catch (Exception e) when(OnException is {})
        {
            await OnException.InvokeAsync(e);
        }
        finally
        {
            IsLoading = false;
        }
    }
    public async Task<TResult> Execute<TResult>(Func<TService, Task<TResult>> action) where TResult: class
    {
        IsLoading = true;

        try
        {
            return await action(Service);
        }
        catch (Exception e) when(OnException is {})
        {
            await OnException.InvokeAsync(e);
        }
        finally
        {
            IsLoading = false;
        }

        return null;
    }
}