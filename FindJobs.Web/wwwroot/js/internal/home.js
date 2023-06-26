var serchOfferUrl = '$1$';

function searchOffer(url) {
  

    var jobCategories = document.querySelector("#jobCategories-multiselect").value;
    var location = document.querySelector("#single-select").value;
    window.open(url +"?jobCategories=" + jobCategories+"&location="+location, '_self');
}


VirtualSelect.init({
    ele: '#jobCategories-multiselect',
    search: true,
    placeholder: '',
    noOfDisplayValues: 2,
    maxValues: 10,
    showDropboxAsPopup: false,
})

var vi = VirtualSelect.init({
    ele: '#single-select',

    search: true,
    placeholder: '',
    onServerSearch: onSampleSelectServerSearch,
    showDropboxAsPopup: false,
});

function onSampleSelectServerSearch(searchValue, virtualSelect) {
    const options = [];

    var url = serchOfferUrl;
    fetch(url + '?value=' + searchValue)
        .then((data) => data.text())
        .then((data) => {
            var result = JSON.parse(data);
            for (var i = 0; i < result.cities.length - 1; i++) {
                var obj = {
                    'label': result.cities[i].name,
                    'value': result.cities[i].id
                }
                options.push(obj);
            }
            return options;
        }).then(function (options) {
            virtualSelect.setServerOptions(options)
        })

};


function set_searchbox_keyword(id) {
    document.querySelector('#jobCategories-multiselect').setValue(id);
}

function set_searchbox_location(id,text) {
    document.querySelector('#single-select').addOption({
        value: text,
        label: text,
    });
    document.querySelector('#single-select').setValue(text);
}