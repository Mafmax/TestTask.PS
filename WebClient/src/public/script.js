const form = document.getElementById("uploadFileForm");
const fileInput = document.getElementById("fileInput");
const fileIdToDownload = document.getElementById("fileIdToDownload");
const downloadButton = document.getElementById("downloadButton");
const filesApi = '/api/Files';
hideDownloadButton();

form.onsubmit = async (e) => {
    e.preventDefault();
    let formData = new FormData();
    formData.append("file", fileInput.files[0]);

    let convertResult = await tryConvertFile(formData);
    handleError(convertResult);
}

fileInput.onchange = async (e) => {
    e.preventDefault();
    hideDownloadButton();
}

async function tryConvertFile(formData) {
    convertingStarted();
    let fileId;
    try {
        let uploadFileResponse = await fetch(`${filesApi}/upload`, {
            method: 'POST',
            body: formData
        }).then(x => x.json());
        handleError(uploadFileResponse);

        let convertFileResponse = await fetch(`${filesApi}/convert/${uploadFileResponse.fileId}`, {
                method: 'GET'
            }
        ).then(x => x.json());
        handleError(uploadFileResponse);
        fileId = convertFileResponse.fileId;
    } finally {
        convertingCompleted(fileId)
    }
}

async function downloadFile() {
    try {
        const fileId = fileIdToDownload.value;
        console.log(`FileId: ${fileId}`);
        const fileNameResponse = await fetch(`${filesApi}/name/${fileId}`, {
            method: 'GET'
        }).then(x => x.json());
        handleError(fileNameResponse);
        const fileName = fileNameResponse.fileName;
        console.log(`File name: ${fileName}`);
        const downloadFileResponse = await fetch(`${filesApi}/download/${fileId}`, {
            method: 'GET'
        });
        handleError(downloadFileResponse);
        const fileHandle = await window.showSaveFilePicker(
            {
                suggestedName: fileName,
                types: [{
                    accept: {'application/pdf': ['.pdf']},
                }],
            });

        const fileStream = await fileHandle.createWritable();

        await downloadFileResponse.body.pipeTo(fileStream);
    } catch (error) {
        console.log(error);
    }
}

function handleError(response) {
    if (response !== undefined && response.errors) {
        for (let i = 0; i < response.errors.length; i++) {
            console.log(`Error ${i}: ${response.errors[i]}`);
        }
        throw new Error("An error occured while request.");
    }
}

