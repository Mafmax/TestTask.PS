const baseUrl = "http://localhost:5128/api/Files/"
const form = document.getElementById("uploadFileForm");
const fileInput = document.getElementById("fileInput");
const fileIdToDownload = document.getElementById("fileIdToDownload");
const downloadButton = document.getElementById("downloadButton");
form.onsubmit = async (e) => {
    e.preventDefault();
    let formData = new FormData();
    formData.append("file", fileInput.files[0]);

    let convertResult = await tryConvertFile(formData);
}

function getUploadFileUrl() {
    return baseUrl + "upload";
}

function getConvertFileUrl(fileId) {
    return baseUrl + "convert/" + fileId;
}

function getDownloadFileUrl(fileId) {
    return baseUrl + "download/" + fileId;
}

function getFileNameUrl(fileId) {
    return baseUrl + "name/" + fileId;
}

async function tryConvertFile(formData) {
    try {
        let uploadFileResponse = await (await fetch(getUploadFileUrl(), {
            method: 'POST',
            body: formData
        })).json();

        let convertFileResponse = await (await fetch(getConvertFileUrl(uploadFileResponse.fileId), {
            method: 'GET'
        })).json();

        showDownloadButton(convertFileResponse.fileId);

    } catch (error) {
        console.error("error" + error)
    }
}

async function downloadFile() {
    const fileId = fileIdToDownload.value;
    console.log(fileId)
    const fileNameResponse = await (await fetch(getFileNameUrl(fileId), {
        method: 'GET'
    })).json();
    const fileName = fileNameResponse.fileName;
    const downloadFileResponse = await fetch(getDownloadFileUrl(fileId), {
        method: 'GET'
    });
    const fileHandle = await window.showSaveFilePicker(
        {
            suggestedName: fileName,
            types: [{
                accept: {'application/pdf': ['.pdf']},
            }],
        });
    
    const fileStream = await fileHandle.createWritable();
    
    await downloadFileResponse.body.pipeTo(fileStream);
}

function showDownloadButton(fileId) {
    downloadButton.hidden = false;
    fileIdToDownload.value = fileId
}