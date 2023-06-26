function checkStateCity() {
    var cityValue = document.getElementById('city').value;
    var countryValue = document.getElementById('country').value;
    var stateValue = document.getElementById('state').value;
   
    if (stateValue != undefined && stateValue.length > 0) {
        document.getElementById('stateBox').style.display = "inline-flex";
        getStates(countryValue);
        showCityState();

    } else {
        document.getElementById('stateBox').style.display = "none";
    }
    if (cityValue != undefined && cityValue.length > 0) {
        document.getElementById('cityBox').style.display = "inline-flex";
        getCities(countryValue);
    } else {
        document.getElementById('cityBox').style.display = "none";
    }

    if (cityValue == undefined || cityValue.length == 0) {
        document.getElementById('CityNameBox').style.display = "inline-flex";
    } else {
        document.getElementById('CityNameBox').style.display = "none";
    }
}
checkStateCity();



function loadCountry() {
    var value = document.getElementById('country')
    fetch('getCountry?value=' + value)
        .then((data) => data.text())
        .then((data) => {
            var result = JSON.parse(data);
            var names = [];


            for (var i = 0; i < result.countries.length - 1; i++) {
                names.push(result.countries[i].name);
            }
            autocomplete("country", names);
        })
}


loadCountry();


function showCityState() {
    var stateVal = document.getElementById("state").value;
    if (document.getElementById("state").value.length > 1) {
        document.getElementById("stateBox").style.display = 'inline-flex';
    } else {
        document.getElementById("stateBox").style.display = 'none';
    }

    if (document.getElementById("city").value.length > 1) {
        document.getElementById("cityBox").style.display = 'inline-flex';
        document.getElementById("CityNameBox").style.display = 'none';
    } else {
        document.getElementById("CityNameBox").style.display = 'inline-flex';
        document.getElementById("cityBox").style.display = 'none';
    }
}
showCityState();





function getCities(value) {
    var urlCities = "getCities";
    fetch(urlCities + "?value=" + value)
        .then((data) => data.text())
        .then((data) => {
            var result = JSON.parse(data);
            var names = [];


            for (var i = 0; i < result.cities.length - 1; i++) {
                names.push(result.cities[i].name);
            }
            autocomplete("city", names);
        })
}
function getStates(value) {
    var urlState = "GetStates";
    fetch(urlState + "?value=" + value)
        .then((data) => data.text())
        .then((data) => {
            var result = JSON.parse(data);
            var names = [];


            for (var i = 0; i < result.states.length - 1; i++) {
                names.push(result.states[i]);
            }
            autocomplete("state", names);
        })
}

function getCitiesByState(value) {
    var urlCityByState = "getCitiesByState";
    fetch(urlCityByState + "?value=" + value)
        .then((data) => data.text())
        .then((data) => {
            var result = JSON.parse(data);
            var names = [];


            for (var i = 0; i < result.cities.length - 1; i++) {
                names.push(result.cities[i].name);
            }
            autocomplete("city", names);
        })
}




function autocomplete(id, arr) {
    var inp = document.getElementById(id);
    var currentFocus;
    inp.addEventListener("input", function (e) {
        var a, b, i, val = this.value;
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        this.parentNode.appendChild(a);
        for (i = 0; i < arr.length; i++) {
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                b = document.createElement("DIV");
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                b.addEventListener("click", function (e) {
                    inp.value = this.getElementsByTagName("input")[0].value;
                    if (id == "country") {
                        if (inp.value == "United States" || inp.value == "Canada") {
                            document.getElementById("stateBox").style.display = 'inline-flex';
                            document.getElementById("state").value = "";
                            document.getElementById("cityBox").style.display = 'none';
                            document.getElementById("city").value = "";
                            getStates(inp.value);
                        } else {
                            document.getElementById("stateBox").style.display = 'none';
                            document.getElementById("state").value = "";
                            document.getElementById("cityBox").style.display = 'inline-flex';
                            document.getElementById("city").value = "";
                            getCities(inp.value);
                        }

                    } else if (id == "state") {
                        document.getElementById("cityBox").style.display = 'inline-flex';
                        document.getElementById("city").value = "";
                        getCitiesByState(inp.value);
                    }

                    closeAllLists();
                });

                a.appendChild(b);
            }
        }
    });
    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            currentFocus++;
            addActive(x);
        } else if (e.keyCode == 38) { //up
            currentFocus--;
            addActive(x);
        } else if (e.keyCode == 13) {
            e.preventDefault();
            if (currentFocus > -1) {
                if (x) x[currentFocus].click();
            }
        }

    });
    function addActive(x) {
        if (!x) return false;
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        x[currentFocus].classList.add("autocomplete-active");

    }
    function removeActive(x) {
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);

    });
}