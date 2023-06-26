
//*****************************paging**********************

function next(pageCount, url) {
    var pagenumber = document.getElementById('pagenumber');
    var currentpage = parseInt(pagenumber.innerText);
    document.getElementById('pagenumber').innerText = Math.min(currentpage + 1, pageCount);
    var pageNum = Math.min(currentpage + 1, pageCount)

    var searchOfferUrl = window.location.pathname;
    searchOfferUrl = searchOfferUrl.replace('ListOffer', 'ListOfferAdvance');
    refresh(searchOfferUrl, pageNum);
}



function back(url) {
    var pagenumber = document.getElementById('pagenumber');
    var currentpage = parseInt(pagenumber.innerText);
    pagenumber.innerText = Math.max(currentpage - 1, 1);
    var pageNum = Math.max(currentpage - 1, 1);
    refresh(url,pageNum);

}


function end(pageCount, url) {
    var pagenumber = document.getElementById('pagenumber');
    pagenumber.innerText = pageCount;
    refresh(url, pageCount);
}


function start(url) {
    var pagenumber = document.getElementById('pagenumber');
    pagenumber.innerText = 1;
    refresh(url,1);
}


function advancerefresh(getListUrl, pageNumber) {
    var url = getListUrl + "&currentPage=" + pageNumber;
    fetch(url).then((data) => { return data.text() })
        .then((data) => {
            var searchbox = document.getElementById('offerListDiv');
            if (data != null) {
                searchbox.innerHTML = "";
                searchbox.innerHTML = data;
            } else {
                searchbox.innerHTML = "";
            }
            window.scrollTo(0, 150);
        });
}
