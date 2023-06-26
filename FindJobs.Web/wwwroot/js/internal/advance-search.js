const search = new Map();
var arrContent = document.querySelectorAll(".item-search");
for (i = 0; i < arrContent.length; i++) {
    var child = arrContent[i];
    if (child.nodeType == 1) {
        child.onclick = function () {
            var text = this.innerText;
            var itemSearch = this.parentElement.parentElement.parentElement;
            itemSearch.style.display = 'none';
            var id = itemSearch.id;
            var str = '<a id="' + id + '" class="advance-a"><span >' + text + '</span><span class="advance-item" tabindex="-1">&times;</span > </a>';
            var para = document.getElementById('advance-show');
            var length = para.childElementCount;
            if (length == 0) {
                var deleteSelected = document.getElementById('delete-selected');
                deleteSelected.style.display = 'block';
                deleteSelected.onclick = function () {
                    search.clear();
                    var childerenAdvance = document.getElementById('advance-show').childNodes;
                    update();
                    for (c = 1; c < childerenAdvance.length; i++) {
                        var idRemove = childerenAdvance[c];
                        idRemove.remove();
                        document.getElementById(idRemove.id).style.display = null;
                    }
                    document.getElementById('delete-selected').style.display = 'none';
                };
            }
            para.insertAdjacentHTML('beforeend', str);
            fillParameters(id, text);
            var advanceSearch = document.getElementById(id);
            advanceSearch.onclick = function () {
                var idAdvance = this.id;
                this.remove();
                search.delete(idAdvance);
                removeElementParams(idAdvance)
                document.getElementById(idAdvance).style.display = null;
                var para = document.getElementById('advance-show');
                var length = para.childElementCount;
                if (length == 0) {
                    document.getElementById('delete-selected').style.display = 'none';
                    search.clear();
                    update();

                }
            };
        }
    }
}
document.getElementById('serv-btn').onclick = function () {
    document.querySelector('nav ul .serv-show').classList.toggle("show1");
    var img = this.getElementsByClassName('column-image')[0];
    if (img.style.display == 'block') {
        img.style.display = 'none';
    } else {
        img.style.display = 'block';
    }
};
document.getElementById('position-btn').onclick = function () {
    document.querySelector('nav ul .position-show').classList.toggle("show3");
    var img = this.getElementsByClassName('column-image')[0];
    if (img.style.display == 'block') {
        img.style.display = 'none';
    } else {
        img.style.display = 'block';
    }
};
document.getElementById('employ-btn').onclick = function () {
    document.querySelector('nav ul .employ-show').classList.toggle("show4");
    var img = this.getElementsByClassName('column-image')[0];
    if (img.style.display == 'block') {
        img.style.display = 'none';
    } else {
        img.style.display = 'block';
    }
};
document.getElementById('language-btn').onclick = function () {
    document.querySelector('nav ul .language-show').classList.toggle("show6");
    var img = this.getElementsByClassName('column-image')[0];
    if (img.style.display == 'block') {
        img.style.display = 'none';
    } else {
        img.style.display = 'block';
    }
};
document.getElementById('jobOffer-btn').onclick = function () {
    document.querySelector('nav ul .jobOffer-show').classList.toggle("show7");
    var img = this.getElementsByClassName('column-image')[0];
    if (img.style.display == 'block') {
        img.style.display = 'none';
    } else {
        img.style.display = 'block';
    }
};
document.getElementById('company-btn').onclick = function () {
    document.querySelector('nav ul .company-show').classList.toggle("show10");
    var img = this.getElementsByClassName('column-image')[0];
    if (img.style.display == 'block') {
        img.style.display = 'none';
    } else {
        img.style.display = 'block';
    }
};
document.getElementsByClassName('title').onclick = function () {
    document.getElementsByClassName(this).classList.add("active").siblings().classList.remove("active");
};

const rangeInput = document.querySelectorAll(".range-input input"),
    priceInput = document.querySelectorAll(".price-input input"),
    range = document.querySelector(".slider .progress");
let priceGap = 1000;

priceInput.forEach(input => {
    input.addEventListener("input", e => {
        let minPrice = parseInt(priceInput[0].value),
            maxPrice = parseInt(priceInput[1].value);

        if ((maxPrice - minPrice >= priceGap) && maxPrice <= rangeInput[1].max) {
            if (e.target.className === "input-min") {
                rangeInput[0].value = minPrice;
                range.style.left = ((minPrice / rangeInput[0].max) * 100) + "%";
                fillParameters('min', minPrice);

            } else {
                rangeInput[1].value = maxPrice;
                range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
                fillParameters('max', minPrice);
            }
        }
    });
});

rangeInput.forEach(input => {
    input.addEventListener("input", e => {
        let minVal = parseInt(rangeInput[0].value),
            maxVal = parseInt(rangeInput[1].value);

        if ((maxVal - minVal) < priceGap) {
            if (e.target.className === "range-min") {
                rangeInput[0].value = maxVal - priceGap
            } else {
                rangeInput[1].value = minVal + priceGap;
            }
        } else {
            priceInput[0].value = minVal;
            priceInput[1].value = maxVal;
            range.style.left = ((minVal / rangeInput[0].max) * 100) + "%";
            range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";
        }
    });
});



function update() {
    var searchOfferUrl = window.location.pathname;
    searchOfferUrl = searchOfferUrl.replace('ListOffer', 'ListOfferAdvance');
    var jobCategories = document.querySelector("#jobCategories-multiselect").value;
    var location = document.querySelector("#single-select").value;
    var url = searchOfferUrl + "?jobCategories=" + jobCategories + "&location=" + location;
    search.forEach((values, key) => {
        url += '&' + key + '=' + values;
    });

    advancerefresh(url, 1);

}

function fillParameters(id, text) {
    if (id === 'work-from-home') {
        search.set('workPlace', text);
    }
    else {
        search.set(id, text);

    }
    update();
}

function removeElementParams(id) {
    search.delete(id);
    update();
}

function refresh(getListUrl, pageNumber) {
    var searchOfferUrl = window.location.pathname;
    searchOfferUrl = searchOfferUrl.replace('ListOffer', 'ListOfferAdvance');
    var jobCategories = document.querySelector("#jobCategories-multiselect").value;
    var location = document.querySelector("#single-select").value;
    var url = searchOfferUrl + "?jobCategories=" + jobCategories + "&location=" + location;
    search.forEach((values, key) => {
        url += '&' + key + '=' + values;
    });
    url += "&currentPage=" + pageNumber;
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
