function loadMorePosts() {

    let url = '/Forum/GetRepliesOnPost';

    var postId = this.value; 
    let paramFrom = document.getElementById('hasMoreFrom_' + postId).value;
    let paramTake = document.getElementById('hasMoreTake_' + postId).value;

    url = url + '?postId=' + postId + '&from=' + paramFrom + '&take=' + paramTake; 

    fetch(url)
    .then(data => data.json())
    .then(response => new function () {
            alert(JSON.stringify(response));
    });
    
}