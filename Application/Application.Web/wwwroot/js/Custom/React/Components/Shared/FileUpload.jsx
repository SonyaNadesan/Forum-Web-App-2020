export class FileUpload extends React.Component {

    constructor(props) {
        super(props);

        let imgSrc = this.props.defaultImgSrc;

        if (this.props.imageSrc == undefined || this.props.imageSrc == null || this.props.imageSrc == "") {
            imgSrc = this.props.imageSrc;
        }

        this.state = {
            currentImageSrc: imgSrc
        };
    }

    render() {
        return (
            <div>
                <img src={this.state.currentImageSrc} id={this.props.imageContainerId} width="10%" height="10%" />
                <br />
                <div className="row">
                    <div className="col-lg-4">
                        <button type="button" className="btn btn-default" data-toggle="modal" data-target="#imagePreviewModal">
                            <span className="glyphicon glyphicon-edit"></span> Change Profile Picutre
                        </button>
                        <br />
                    </div>
                <div className="col-lg-8"></div>
                </div>
                <div id="imagePreviewModal" className="modal fade" role="dialog">
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal">&times;</button>
                                <h4 className="modal-title">Profile Picture Preview</h4>
                                <label style={{ display: "inline-block" }} className="btn btn-default btn-file">
                                    <span className="glyphicon glyphicon-upload"></span>
                                    Select file to upload
                                    <input type="file"
                                        name="profilePicture"
                                        id="btnFile"
                                        onChange={this.displayProfilePicturePreview}
                                        style={{ display: "none" }}
                                    />
                                </label>
                            </div>
                            <div className="modal-body">
                                <div id="imagePreviewBlock"></div>
                            </div>
                            <div className="modal-footer">
                                <button type="submit" id="btnFileUploadSubmit" onClick={this.props.uploadFile}>Submit</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    displayProfilePicturePreview() {
        let fileUploadBtnId = "btnFile";
        let elementIdOfPreviewBlockId = "imagePreviewBlock";

        document.getElementById(elementIdOfPreviewBlockId).innerHTML = '';

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

    uploadFile(event) {
        let files = document.getElementById("btnFile").files;

        if (files.length == 0) {
            throw new Error("No file selected");
        }

        let data = new FormData();
        data.append('file', files[0]);

        fetch(this.props.formAction, {
            method: 'POST',
            credentials: 'same-origin',
            body: data
        });
    }
}