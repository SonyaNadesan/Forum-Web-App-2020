import { FileUpload } from '../Components/Shared/FileUpload.jsx';
import { ForumIndex } from '../Renderings/Forum_Index.jsx';
import { reactionsListener } from '../Global/Notification.js';
import { postsListener } from '../Global/Notification.js';

export class App extends React.Component {

    render() {
        return (
            <div>
                <FileUpload
                    imageContainerId="x_"
                    defaultImgSrc="http://localhost:55931/Images/defaultProfilePic.jpg"
                    imageSrc="http://localhost:55931/Images/defaultProfilePic.jpg"
                    formAction="/Profile/UploadProfilePicture"
                />
                <div>
                    <ForumIndex
                        reactionsListener={reactionsListener}
                        postsListener={postsListener}
                    />
                </div>
            </div>
        );
    }

}


ReactDOM.render(
    <App />,
    document.getElementById('content')
);