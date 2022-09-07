using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace formica.Pages;

public partial class Index
{
    const string DOC_ID = "i";
    [Inject] protected ProtectedSessionStorage ProtectedSessionStore { get; set; } = null!;
    bool _initialLoading = true;
    FileInfo _file = new(C.DataFor(Guid.NewGuid()));
    Data _data = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _initialLoading = false;
        var guid = await ProtectedSessionStore.GetAsync<Guid>(DOC_ID);
        if (guid.Success)
        {
            _file = new(C.DataFor(guid.Value));
            if (_file.Exists)
            {
                using var reader = _file.OpenRead();
                var data = JsonSerializer.Deserialize<Data>(reader);
                if (data != null)
                    _data = data;
            }
        }

        StateHasChanged();
    }
    async Task Start()
    {
        using var stream = _file.Create();
        JsonSerializer.Serialize(stream, _data);

        _file.Refresh();
        await ProtectedSessionStore.SetAsync(DOC_ID, Path.GetFileNameWithoutExtension(_file.Name));
    }
}