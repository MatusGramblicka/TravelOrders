//https://learn.microsoft.com/en-us/aspnet/core/blazor/file-downloads?view=aspnetcore-8.0&preserve-view=true#download-from-a-url
function triggerFileDownload(url) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    // fileName is not needed, it looks like this info is obtained by url
    //anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
}
