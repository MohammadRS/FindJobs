

function licenseType() {
    var drivingCheckBox = document.getElementById("DrivingLicenseCheckBox");

    if (drivingCheckBox.checked == true) {
        document.getElementById("SelectLicenseType").style.display = "block";
    } else {
        document.getElementById("SelectLicenseType").style.display = "none";
    }
};
licenseType();
function changeLicenseType() {
    var drivingCheckBox = document.getElementById("DrivingLicenseCheckBox");
    if (drivingCheckBox.checked == true) {
        document.getElementById("SelectLicenseType").style.display = "block";
    } else {
        document.getElementById("SelectLicenseType").style.display = "none";
    }

}

var imageSubmitButton = document.getElementById("applicant-image").addEventListener("onclick", imageClicked);
async function imageClicked() {
    var inputImageElement = document.getElementById("image-input");
    inputImageElement.click();
};
function SendImageFile(imageSizeError, fileNotImage, uploadSuccessfully, notUpload) {
    var inputImageElement = document.getElementById("image-input");
    var file = inputImageElement.files;

    if (file[0].size > 10485760) {
        MessageShow("Error", imageSizeError, "error");
        return;
    }


    var fileClone = Object.assign({}, file);
    var extensions = ["jpg", "jpeg", "png", "pdf"];
    var extension = fileClone[0].name.replace(/.*\./, '').toLowerCase();

    if (extensions.indexOf(extension) < 0) {
        MessageShow("Error", fileNotImage, "error");
        return;
    }
    let formData = new FormData();
    formData.append("inputFile", file[0]);
    var actionName = "UploadImage";
    if (file.length > 0) {
        fetch(actionName, {
            method: "post",
            body: formData
        }).then((data) => data.text())
            .then((data) => {
                if (data != "") {
                    var showImage = document.getElementById("applicant-image");
                    showImage.src = "data:image/png;base64," + data;
                    MessageShow("Success", uploadSuccessfully, "success");
                } else {
                    MessageShow("Error", notUpload, "error");
                }

            });
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


function showModal(modalId) {
    document.getElementById(modalId).style.display = "block";


    if (modalId == 'WorkExperienceModal') {
        currentlyWorkChecked();
    }
}

function closeModal(modalId) {
    document.getElementById(modalId).style.display = 'none';

}
function displayRateInputs() {
    let isChecked = document.getElementById("HourlyrateCheckbox").checked;
    let tds = document.getElementsByClassName("tdHourly");
    if (!isChecked) {
        document.getElementById('rateAvarage').style.display = 'table-cell';

        [].forEach.call(tds, function (td) { td.style.display = 'none'; });

    } else {
        document.getElementById('rateAvarage').style.display = 'none';
        [].forEach.call(tds, function (td) { td.style.display = 'table-cell'; });
    }
}

(function () {
    displayRateInputs();
    document.getElementById("HourlyrateCheckbox").onchange = function () {
        displayRateInputs();
    }
})();


var readyToWork = document.getElementById('ReadyToWorkStatus').value;
if (readyToWork == 1) {
    document.getElementById("availability").style.display = 'none';

} else {
    if (document.getElementById("availability") != undefined) {
        document.getElementById("availability").style.display = 'block';
    }

}
function availabilityChange() {

    var readyToWork = document.getElementById('ReadyToWorkStatus').value;
    if (readyToWork == 1) {
        document.getElementById("availability").style.display = 'none';
    } else {
        document.getElementById("availability").style.display = 'block';
    }
}


function currentlyWorkChecked() {
    let currentlyWorkchecked = document.getElementById('IsCurrentlyWorking').checked;
    if (currentlyWorkchecked) {
        document.getElementById('workExperienceEndWorkDate').style.visibility = 'hidden';
    } else {
        document.getElementById('workExperienceEndWorkDate').style.visibility = 'visible';
    }
    document.getElementById('IsCurrentlyWorking').onchange = function () {
        let currentlyWorkchecked = document.getElementById('IsCurrentlyWorking').checked;
        if (currentlyWorkchecked) {
            document.getElementById('workExperienceEndWorkDate').style.visibility = 'hidden';
        } else {
            document.getElementById('workExperienceEndWorkDate').style.visibility = 'visible';
        }
    }
};


function saveDocument(divList, selectDocumentfile, documentSizeNotValid, fileNotImage, UploadSuccessfully, NotUploadMoreThanSix) {

    var inputImageElement = document.getElementById("DocumentFile");

    var file = inputImageElement.files[0];
    if (file == undefined) {
        MessageShow("info", selectDocumentfile, "error");
        return false;
    }
    let documentType = document.getElementById("documentType").value;
    let formData = new FormData();
    if (file.size > 10485760) {
        MessageShow("info", documentSizeNotValid, "info");
        return;
    }
    var extensions = ["jpg", "jpeg", "png", "pdf"];
    var extension = file.name.substring(file.name.lastIndexOf('.') + 1, file.length) || file;

    if (extensions.indexOf(extension) < 0) {
        MessageShow("Error", fileNotImage, "error");
        return;
    }
    closeModal('documentModal', 'buttonShowDocumentModal');
    formData.append("DocumentFile", file);
    formData.append("Type", documentType);
    formData.append("ExtensionFile", "");
    let url = 'UploadDocument';

    fetch(url, {
        method: "post",
        body: formData
    }).then((data) => { return data.text() })
        .then((data) => {
            if (isJsonString(data)) { var result = JSON.parse(data) }
            else {
                var result = { "result": { "success": "Error" } };
            }
            if (result.success != "Error") {

                MessageShow("Success", UploadSuccessfully, "success");
                var divTable = document.getElementById(divList);
                calculateApplicantProgreess();
                divTable.innerHTML = data;
              

            } else {
                MessageShow("Error", NotUploadMoreThanSix, "error");
            }

        });

    return false;
};

function saveEntity(form, url, divList, message, modalName, saveSuccessfully, notSaveSuccessfully) {
    document.getElementById("btnSaveForm").setAttribute("disabled", "disabled");
    closeModal(modalName);
    fetch(url, {
        method: "post",
        body: new FormData(form)
    }).then((data) => { return data.text() })
        .then((data) => {
            if (isJsonString(data)) { var result = JSON.parse(data) }
            else {
                var result = { "result": { "success": "Error" } };
            }

            if (result.success != "Error") {
                MessageShow("Success", message, "success");
                var divTable = document.getElementById(divList);
                divTable.innerHTML = "";
                calculateApplicantProgreess();
                divTable.innerHTML = data;
                if (url == 'CreateOrUpdateApplicantContactDetail') {
                    document.location.reload();
                }
            } else {
                MessageShow("Error", notSaveSuccessfully, "error");
            }

        });
    document.getElementById("btnSaveForm").removeAttribute("disabled");
    return false;
};

function getEntity(id, url, showDiv, modalName, cantFetchData) {
    fetch(url + "?id=" + id).then((data) => { return data.text() })
        .then((data) => {
            if (isJsonString(data)) { var result = JSON.parse(data) }
            else {
                var result = { "result": { "success": "Error" } };
            }

            if (result.success != "Error") {
                var divInserUpdate = document.getElementById(showDiv);
                divInserUpdate.innerHTML = data;
                showModal(modalName);
            } else {
                MessageShow("Error", cantFetchData, "error");

            }
        });
};

function deleteEntity(id, url, entityName, divList, modalName) {
    showModal('DeleteModal');
    document.getElementById("btnDeleteModal").onclick = function () {
        closeModal('DeleteModal');
        fetch(url + "?id=" + id).then((data) => { return data.text() })
            .then((data) => {
                if (isJsonString(data)) { var result = JSON.parse(data) }
                else {
                    var result = { "result": { "success": "Error" } };
                }

                if (result.success != "Error") {
                    MessageShow("Success", "Your " + entityName + " Deleted Successfully", "success");
                    var divTable = document.getElementById(divList);
                    closeModal(modalName);
                    divTable.innerHTML = "";
                    divTable.innerHTML = data;
                    calculateApplicantProgreess();
                } else {
                    MessageShow("Error", "Your " + entityName + "Not Deleted Successfully", "error");
                }
            });
    }
};

//save 3 form with one save button
function SaveAdditionalSection(updateMessage, NotUpdateSuccessfully) {
    var form = document.forms.additionalSectionForm;
    var url = 'CreateOrUpdateApplicantAdditionalSection';
    var message = updateMessage;
    var divList = "ApplicantAdditionalSection";
    fetch(url, {
        method: "post",
        body: new FormData(form)
    }).then((data) => { return data.text() })
        .then((data) => {
            if (isJsonString(data)) { var result = JSON.parse(data) }
            else {
                var result = { "result": { "success": "Error" } };
            }

            if (result.success != "Error") {
                var divTable = document.getElementById(divList);
                divTable.innerHTML = "";
                divTable.innerHTML = data;
                calculateApplicantProgreess();
                MessageShow("Success", message, "success");
                displayRateInputs();
                document.getElementById("HourlyrateCheckbox").onchange = function () {
                    displayRateInputs();
                }
                changeLicenseType();
                licenseType();

            } else {
                MessageShow("Error", NotUpdateSuccessfully, "error");
            }

        })

    return false;
}
function SavePersonalInformation(updateMessage, notSaveSuccessfully) {
    var form = document.forms.personalInformationForm;
    var url = 'CreateOrUpdateApplicantPersonalInformation';
    var divList = "PersonalInformationDiv";
    var message = updateMessage;
    fetch(url, {
        method: "post",
        body: new FormData(form)
    }).then((data) => { return data.text() })
        .then((data) => {
            if (isJsonString(data)) { var result = JSON.parse(data) }
            else {
                var result = { "result": { "success": "Error" } };
            }


            if (result.success != "Error") {
                MessageShow("Success", message, "success");
                var divTable = document.getElementById(divList);
                divTable.innerHTML = "";
                divTable.innerHTML = data;
                calculateApplicantProgreess();
                var readyToWork = document.getElementById('ReadyToWorkStatus').value;
                if (readyToWork == 1) {
                    document.getElementById("availability").style.display = 'none';

                } else {
                    if (document.getElementById("availability") != undefined) {
                        document.getElementById("availability").style.display = 'block';
                    }
                }
            } else {
                MessageShow("Error", notSaveSuccessfully, "error");
            }

        });

    return false;
}
function SaveApplicationContactDetail(updateMessage, phoneNumberNotValid, notSaveSuccessfully) {
    var form = document.forms.SaveApplicantContactDetail;
    var url = 'CreateOrUpdateApplicantContactDetail';
    var divList = "ApplicantContactDetailDiv";
    var phoneVar = document.getElementById("ContactPhone").value;
    var message = updateMessage;
    var myPhoneRegex = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    if (!myPhoneRegex.test(phoneVar)) {
        MessageShow("Error", phoneNumberNotValid, "error");
        return false;
    }
    fetch(url, {
        method: "post",
        body: new FormData(form)
    }).then((data) => { return data.text() })
        .then((data) => {
            if (isJsonString(data)) { var result = JSON.parse(data) }
            else {
                var result = { "result": { "success": "Error" } };
            }

            if (result.success != "Error") {
                MessageShow("Success", message, "success");

                var divTable = document.getElementById(divList);
                divTable.innerHTML = "";
                divTable.innerHTML = data;
                calculateApplicantProgreess();
                loadCountry();
                checkStateCity();
            } else {
                MessageShow("Error", notSaveSuccessfully, "error");
            }

        });

    return false;
}

function calculateApplicantProgreess() {
    fetch('CalculationProgressbar').then((data) => { return data.text() }).then((data) => {
        document.getElementById("skill-progressbar").innerHTML = data;
    });
};

calculateApplicantProgreess();

function isJsonString(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}










