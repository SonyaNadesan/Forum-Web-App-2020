function previewImageOnSelectingFile(fileUploadBtnId, elementIdOfPreviewBlockId) {

    var fileUploadBtn = document.getElementById(fileUploadBtnId);
    var previewBlock = document.getElementById(elementIdOfPreviewBlockId);

    for (var i = 0; i < fileUploadBtn.files.length; i++) {
        if (fileUploadBtn.files != null && fileUploadBtn.files[i]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                var image = document.createElement("img");
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