let commentsDisplaySwitcherElement = document.getElementById('comments-display-switcher');
let isCommentsDisplayed = false;

let commentsTextContainer;
let wasFocused = false;
let userText;

let commentsContainerArea;
let commentsContainer;
let getCommentsIntervalId;

function toggleComments(newsId) {

    if (commentsDisplaySwitcherElement != null) {
        if (isCommentsDisplayed == true) {
            commentsDisplaySwitcherElement.innerHTML = 'Display comments';
            document.getElementById('comments-container').innerHTML = '';

        } else {
            commentsDisplaySwitcherElement.innerHTML = 'Hide comments';
            commentsContainer = document.getElementById('comments-container'); 
            loadComments(newsId, commentsContainer);
            commentsContainerArea = document.getElementById('comments-create-area');
            loadCommentsArea(commentsContainerArea);
            getCommentsIntervalId = setInterval(function () {
                let newsIdUpdate = document.location.pathname.substring(document.location.pathname.lastIndexOf('/') + 1);
                commentsContainer = document.getElementById('comments-container');
                loadComments(newsIdUpdate, commentsContainer);
            }, 15000);

        }
        isCommentsDisplayed = !isCommentsDisplayed;
    }

    commentsDisplaySwitcherElement.addEventListener('onclose', function () {
        alert('closed');
    }, false);
}

function loadComments(newsId, commentsContainer) {
    let requestLoadList = new XMLHttpRequest();
    //create request
    requestLoadList.open('GET', `/Comments/List?newsId=${newsId}`, true);
    //create request handler

    requestLoadList.onload = function () {
        if (requestLoadList.status >= 200 && requestLoadList.status < 400) {
            let resp = requestLoadList.responseText;
            commentsContainer.innerHTML = resp;
        }
    }
    //send request
    requestLoadList.send();
}

function loadCommentsArea(commentsContainerArea) {
    let requestLoadCommentsPartial = new XMLHttpRequest();
    //create request
    requestLoadCommentsPartial.open('GET', `/Comments/CreateCommentPartial`, true);
    //create request handler

    requestLoadCommentsPartial.onload = function () {

        if (requestLoadCommentsPartial.status >= 200 && requestLoadCommentsPartial.status < 400) {
            let resp = requestLoadCommentsPartial.responseText;
            commentsContainerArea.innerHTML = resp;
            document.getElementById('create-comment-btn')
                .addEventListener("click", createComment);
        }
    }
    //send request
    requestLoadCommentsPartial.send();
}

function createComment() {

    let commentText = document.getElementById('commentText').value;
    let newsId = document.getElementById('newsId').value;

    var postRequest = new XMLHttpRequest();
    postRequest.open("POST", '/Comments/Create', true);
    postRequest.setRequestHeader('Content-Type', 'application/json');

    postRequest.send(JSON.stringify({
        commentText: commentText,
        newsId: newsId
    }));

    postRequest.onload = function () {
        if (postRequest.status >= 200 && postRequest.status < 400) {
            document.getElementById('commentText').value = '';
            commentsContainer = document.getElementById('comments-container');
            loadComments(newsId, commentsContainer);
        }
    }
}






