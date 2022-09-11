using System.Text.Json;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MimeKit;

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
        await JsonSerializer.SerializeAsync(stream, _data);

        _file.Refresh();
        await ProtectedSessionStore.SetAsync(DOC_ID, Path.GetFileNameWithoutExtension(_file.Name));
    }
    async Task Update()
    {
        _file.Delete();
        using var stream = _file.Create();
        await JsonSerializer.SerializeAsync(stream, _data);

        _file.Refresh();
        StateHasChanged();

        if (_data.Step == Steps.Sending)
            await Send();
    }
    async Task Send()
    {
        _data.FinishedAt = DateTime.UtcNow;
        var email = new MimeMessage();
        email.Subject = C.Settings.Subject;
        email.From.Add(MailboxAddress.Parse(C.Settings.From));
        email.To.Add(MailboxAddress.Parse(C.Settings.To));

        var bb = new BodyBuilder();
        bb.TextBody = _data.GetText();
        email.Body = bb.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(C.Settings.Host, C.Settings.Port, SecureSocketOptions.Auto);
        await client.AuthenticateAsync(C.Settings.Username, C.Settings.Password);
        await client.SendAsync(email);
        await client.DisconnectAsync(true);

        _data.Step = Steps.ThankYou;
        await Update();
    }
}