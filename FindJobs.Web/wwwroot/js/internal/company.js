var required = '$1$';
var notmached = '$2$';
var notvalidphonenumber = '$3$';
function saveCompany(phoneNotValid, updateSuccessfully, notUpdate) {
    var myPhoneRegex = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    let Name = document.getElementById("Name").value;
    let CompanyRegistrationId = document.getElementById("CompanyRegistrationId").value;
    let VatNumber = document.getElementById("VatNumber").value;
    let TaxNumber = document.getElementById("TaxNumber").value;
    let Street = document.getElementById("Street").value;
    let PostalCode = document.getElementById("PostalCode").value;
    let Country = document.getElementById("ContactCountry").value;
    let CityId = document.getElementById("ContactCity").value;
    let WebSite = document.getElementById("WebSite").value;
    let Phone = document.getElementById("Phone").value;


    let Logo = document.getElementById("imgLogo").src;
    let FirstName = document.getElementById("FirstName").value;
    let LastName = document.getElementById("LastName").value;
    let ContactPersonEmail = document.getElementById("ContactPersonEmail").value;
    let ContactPersonPhone = document.getElementById("ContactPhone").value;
    let NumberOfEmployee = document.getElementById("NumberOfEmployee").value;
    let AboutCompany = document.getElementById("AboutCompany").value;

    /* ********************* validators*************************/
    let checkvalidation = false;
    if (Name == "") {
        document.getElementById('validateName').innerText = `${required}`;
        checkvalidation = true;

    } else {
        document.getElementById('validateName').innerText = '';
    }

    if (Street == "") {
        document.getElementById('validateStreet').innerText = `${required}`;
        checkvalidation = true;
    }
    else {
        document.getElementById('validateStreet').innerText = '';
    }

    if (PostalCode == "") {
        document.getElementById('validatePostalCode').innerText = `${required}`;
        checkvalidation = true;
    } else {
        document.getElementById('validatePostalCode').innerText='';
    }

    if (Country == "") {
        document.getElementById('validateContactCountry').innerText = `${required}`;
        checkvalidation = true;
    }else {
        document.getElementById('validateContactCountry').innerText = '';
    }

    if (Phone == "") {
        document.getElementById('validatePhone').innerText = `${required}`;
        checkvalidation = true;
    } else {
        document.getElementById('validatePhone').innerText = '';
    }

    if (ContactPersonPhone == "") {
        document.getElementById('validateContactPhone').innerText = `${required}`;
        checkvalidation = true;
    } else {
        document.getElementById('validateContactPhone').innerText = '';
    }

    if (FirstName == "") {
        document.getElementById('validatefirstName').innerText = `${required}`;
        checkvalidation = true;
    } else {
        document.getElementById('validatefirstName').innerText = '';
    }

    if (LastName == "") {
        document.getElementById('validateLastName').innerText = `${required}`;
        checkvalidation = true;
    } else {
        document.getElementById('validateLastName').innerText = '';
    }

    if (ContactPersonEmail == "") {
        document.getElementById('validateContactPersonEmail').innerText = `${required}`;
        checkvalidation = true;
    } else {
        document.getElementById('validateContactPersonEmail').innerText = '';
    }

    if (Phone != ContactPersonPhone) {
        document.getElementById('validatePhone').innerText = `${notmached}`;
        document.getElementById('validateContactPhone').innerText = `${notmached}`;
        checkvalidation = true;
    }

    if (!myPhoneRegex.test(Phone)) {
        document.getElementById('validatePhone').innerText = `${notvalidphonenumber}`;
        checkvalidation = true;
    }

    if (!myPhoneRegex.test(ContactPersonPhone)) {
        document.getElementById('validateContactPhone').innerText = `${notvalidphonenumber}`;
        checkvalidation = true;
    }

    if (checkvalidation == true) {
        return false;
    }
    ///********************** validators *************************/

    let formData = new FormData();

    formData.append("Name", Name);
    formData.append("CompanyRegistrationId", CompanyRegistrationId);
    formData.append("VatNumber", VatNumber);
    formData.append("TaxNumber", TaxNumber);
    formData.append("Street", Street);
    formData.append("PostalCode", PostalCode);
    formData.append("CountryCode", Country);
    formData.append("CityId", CityId);
    formData.append("WebSite", WebSite);
    formData.append("Phone", Phone);
    formData.append("Logo", Logo);
    formData.append("FirstName", FirstName);
    formData.append("LastName", LastName);
    formData.append("ContactPersonEmail", ContactPersonEmail);
    formData.append("ContactPersonPhone", ContactPersonPhone);
    formData.append("NumberOfEmployee", NumberOfEmployee);
    formData.append("AboutCompany", AboutCompany);
    fetch("SaveCompany", {
        method: "post",
        body: formData
    }).then((data) => { return data.text() })
        .then((data) => {
            var msg = JSON.parse(data);
            if (msg.success == 'success') {
                MessageShow("Success", updateSuccessfully, "success");
                setTimeout(() => {

                }, 3500);
                

            } else {
                MessageShow("Error", notUpdate, "error");
            }


        });
    return false;
};

var input = document.querySelector('input[type=file]');
input.onchange = function () {
    var file = input.files[0],
        reader = new FileReader();
    reader.onloadend = function () {
        var b64 = reader.result.replace(/^data:.+;base64,/, '');
        var datasrc = "data: image/png;base64, " + b64;
        var phoneVar = document.getElementById("imgLogo");
        var imgVar = document.getElementById("imgLogoLebel");
        phoneVar.src = datasrc;
        imgVar.style.visibility = null;
        phoneVar.style.visibility = null;
    };
    reader.readAsDataURL(file);
};



function companyName(div) {
    var img = div.getElementsByTagName('img');
    var name = div.getElementsByTagName('p');
    img[0].style.visibility = 'hidden';
    name[0].style.display = 'block';
}

function companyLogo(div) {
    var img = div.getElementsByTagName('img');
    var name = div.getElementsByTagName('p');
    img[0].style.visibility = 'visible';
    name[0].style.display = 'none';
}



async function selectCity() {
    var countryName = document.getElementById("ContactCountry").value;
    var actionName = "../Culture/GetCitiesByCountryCode";
    var resultOfRequest = await SendPostAjaxRequestToServer(countryName, actionName);
    var contactCity = document.querySelectorAll("#ContactCity option");
    var citiesSelectElement = document.getElementById("ContactCity");


    contactCity.forEach(o => o.remove());
    var citiesSelectElement = document.getElementById("ContactCity");
    var opt = document.createElement('option');
    opt.value = "";
    opt.innerHTML = "choice the city";
    citiesSelectElement.prepend(opt);
    for (var i = 0; i < resultOfRequest[0].data.length; i++) {
        var option = document.createElement('option');
        option.value = resultOfRequest[0].data[i].id;
        option.innerHTML = resultOfRequest[0].data[i].name;
        citiesSelectElement.appendChild(option);
    }
   
}

async function SendPostAjaxRequestToServer(data, actionName) {
    var resultOfRequest = [];
    const subscribe = await fetch(actionName, {
        method: "Post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then((response) => response.json())
        .then((data) => { resultOfRequest.push(data) });
    return resultOfRequest;
};




