import { arrayToCommaSeperatedLString } from '../../Common/StringifyHelpers.js';

export class LikeFeature extends React.Component {

    render() {

        if (this.props.itemId == undefined || this.props.itemId == null || this.props.itemId == "") {
            return (
                <div className="display: flex;"></div>
            );
        }

        let users = (this.props.usersWhoHaveReacted == undefined || this.props.usersWhoHaveReacted == null) ? [] : this.props.usersWhoHaveReacted;

        if (this.props.hasLoggedOnUserReactedToThread == true) {
            let currentUser = this.props.loggedOnUser;
            currentUser.name = "You";

            if (users.includes(currentUser) == false) {
                users.unshift(currentUser);
            }
        }

        let userNames = users.map(function (user) {
            return user.name
        });

        let label = arrayToCommaSeperatedLString(userNames, true);
        let displayPics = [];

        if (users.length == 0) {
            label = "Be the first to react to this!"
        }

        users.map((user, index) => {
            let num = index + 1;
            user.avatarSrc = user.avatarSrc == "" ? "/Images/defaultProfilePic.jpg" : "/Images/" + user.avatarSrc;

            displayPics.push(
                <span id={"reactionAvatarDisplaySpan" + num + "_" + this.props.itemId} key={index}>
                    <img src={user.avatarSrc} id={"reactionAvatarDisplay" + num + "_" + this.props.itemId} style={{ borderRadius: '50%', width: '30px', height: '30px', zIndex: '1' }} />
                </span>
            );
        });

        label = users.length != 0 ? label + " reacted to this!" : label;

        return (
            <div className="display: flex;">
                <button name="btnReaction" id={"btnReaction_" + this.props.itemId} value={this.props.itemId} onClick={this.props.likeHandler}>
                    <span className="glyphicon glyphicon-thumbs-up"></span>
                </button>
                {displayPics}
                <span name="spnReactionCount" id={"spnReactionsCount_" + this.props.itemId} value={this.props.itemId}>{label}</span>
            </div>
        );
    }
}