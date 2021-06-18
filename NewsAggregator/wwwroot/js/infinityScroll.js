let container = document.getElementById('infinity-scroll');
let pageCounter = 1;

window.addEventListener("scroll", function () {

    let contentHeight = container.offsetHeight;                         // 1) высота блока контента вместе с границами
    let placeToLoading = contentHeight - contentHeight / 4;
    let yOffset = window.pageYOffset;                                   // 2) текущее положение скролбара

    // если пользователь достиг конца
    if (yOffset >= placeToLoading) {
        gerMoreNews(container);
    }
});


function gerMoreNews(container) {

    let lastDate = searchLastGottenDate();
    let requestGetNews = new XMLHttpRequest();
    //create request
    requestGetNews.open('GET', `/News/InfinityPaggination?lastGottenDate=${lastDate}`, true);
    //create request handler

    requestGetNews.onload = function () {
        if (requestGetNews.status >= 200 && requestGetNews.status < 400) {
            let resp = requestGetNews.responseText;
            container.innerHTML = container.innerHTML + resp;
            pageCounter = pageCounter + 1;
        }
    }
    //send request
    requestGetNews.send();
}

function searchLastGottenDate() {

    let collectionDate = document.querySelectorAll('[id="date"]');
    let lastDateNode = collectionDate[collectionDate.length - 1];
    console.log(lastDateNode);
    let lastGottenDate = lastDateNode.innerText;

    return lastGottenDate;
}
