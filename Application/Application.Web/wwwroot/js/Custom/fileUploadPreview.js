function previewImageOnSelectingFile(fileUploadBtnId, elementIdOfPreviewBlockId) {

    let fileUploadBtn = document.getElementById(fileUploadBtnId);
    let previewBlock = document.getElementById(elementIdOfPreviewBlockId);

    for (let i = 0; i < fileUploadBtn.files.length; i++) {
        if (fileUploadBtn.files != null && fileUploadBtn.files[i]) {
            let reader = new FileReader();

            reader.onload = function (e) {
                let image = document.createElement("img");
                image.id = elementIdOfPreviewBlockId + "_img" + i;
                image.width = 180;
                image.height = 100;
                image.src = e.target.result;
                previewBlock.appendChild(image);
            };

            reader.readAsDataURL(fileUploadBtn.files[i]);
        }
    }

}