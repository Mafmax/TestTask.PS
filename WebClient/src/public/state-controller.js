function convertingStarted() {
    hideDownloadButton();
}

function convertingCompleted(fileId) {
    if (fileId) {
        showDownloadButton(fileId);
    }
}

function hideDownloadButton() {
    downloadButton.hidden = true;
}

function showDownloadButton(fileId) {
    downloadButton.hidden = false;
    fileIdToDownload.value = fileId
}