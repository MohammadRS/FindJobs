

var offer = document.getElementById('company-offer');
var pub = document.getElementById('company-publication');
var rev = document.getElementById('Review');
var pay = document.getElementById('payment');
var lnkOffer = document.getElementById('lnkOffer');
var lnkPub = document.getElementById('lnkPub');
var lnkRev = document.getElementById('lnkRev');
var lnkPay = document.getElementById('lnkPay');

/*mobile-step-wizard*/
var c_profile = document.getElementById('c_profile');
var c_offer = document.getElementById('c_offer');
var c_pub = document.getElementById('c_pub');
var c_rev = document.getElementById('c_rev');
var c_pay = document.getElementById('c_pay');


offer.style.display = 'block';
pub.style.display = 'none';
rev.style.display = 'none';
pay.style.display = 'none';

lnkOffer.classList.add('done');

function goToPage(page) {
    if (page == "offer") {
        pageOfferShow();
        return;
    }
    if (page == "publication") {

        pagePublicationShow();
        return;
    }
    if (page == "review") {
        pageReviewShow();
        return;
    }
    if (page == "payment") {
        pagePaymentShow();
        return;
    }
}

function nextDiv(required, primaryEducationText, highSchoolEducationText, secondarySchoolWithGraduageText, secondarySchoolWithOutGraduageText, postSecondaryText, universityStudentText, highEducationLevel1, highEducationLevel2, highEducationLevel3, fulltimeText, parttimeText, agrementText, freelancerText, intershipText) {

    if (offer.style.display == 'block') {
        var basicprice = document.getElementById('OfferDto_BasicSalary').value;
        var upperprice = document.getElementById('OfferDto_UpperLimit').value;
        if (basicprice != "" && upperprice != "") {
            if (parseInt(basicprice) > parseInt(upperprice)) {
                document.getElementById('basicvalidation').style.display = 'block';
                document.getElementById('uppervalidation').style.display = 'block';
                window.scrollTo(0, 150);
                return;
            }
        }
        if (document.getElementById("OfferDto_JobTitle").value == "") {
            document.getElementById('jobTitleValidate').style.display = 'block';
            window.scrollTo(0, 150);
            return;
        } else {
            document.getElementById('jobTitleValidate').style.display = 'none';
        }

        jobOfferPreview(required, primaryEducationText, highSchoolEducationText, secondarySchoolWithGraduageText, secondarySchoolWithOutGraduageText, postSecondaryText, universityStudentText, highEducationLevel1, highEducationLevel2, highEducationLevel3, fulltimeText, parttimeText, agrementText, freelancerText, intershipText);
        pagePublicationShow();
        return;
    }
    if (pub.style.display == 'block') {
        pageReviewShow();
        return;
    }
    if (rev.style.display == 'block') {

        pagePaymentShow();
        return;
    }

}
function preDiv() {
    if (pay.style.display == 'block') {
        pageReviewShow();
        removePayWizard();
        return;
    }
    if (rev.style.display == 'block') {

        pagePublicationShow();
        removeRevWizard();
        return;
    }
    if (pub.style.display == 'block') {
        pageOfferShow();
        removePubWizard();
        return;
    }

}

function pageOfferShow() {
    lnkOffer.classList.add('done');
    lnkPay.classList.remove('done');
    lnkPub.classList.remove('done');
    lnkRev.classList.remove('done');
    offer.style.display = 'block';
    pub.style.display = 'none';
    rev.style.display = 'none';
    pay.style.display = 'none';
    c_offer.className = 'text_circle done';
    document.getElementById('btn_next').style.display = 'initial';
}
function pagePublicationShow() {
    if (document.getElementById("OfferDto_JobTitle").value == "") {
        document.getElementById('jobTitleValidate').style.display = 'block';
        window.scrollTo(0, 150);
        return;
    } else {
        document.getElementById('jobTitleValidate').style.display = 'none';
    }

    lnkOffer.classList.remove('done');
    lnkPay.classList.remove('done');
    lnkPub.classList.add('done');
    lnkRev.classList.remove('done');

    lnkPub.classList.add('done');
    offer.style.display = 'none';
    pub.style.display = 'block';
    rev.style.display = 'none';
    pay.style.display = 'none';


    c_pub.classList.add('done');
    document.getElementById('btn_next').style.display = 'initial';

}
function pageReviewShow() {
    if (document.getElementById("OfferDto_JobTitle").value == "") {
        document.getElementById('jobTitleValidate').style.display = 'block';
        window.scrollTo(0, 150);
        return;
    } else {
        document.getElementById('jobTitleValidate').style.display = 'none';
    }

    lnkOffer.classList.remove('done');
    lnkPay.classList.remove('done');
    lnkPub.classList.remove('done');
    lnkRev.classList.add('done');

    lnkRev.classList.add('done');
    offer.style.display = 'none';
    pub.style.display = 'none';
    rev.style.display = 'block';
    pay.style.display = 'none';

    c_rev.classList.add('done');
    document.getElementById('btn_next').style.display = 'initial';


}
function pagePaymentShow() {
    if (document.getElementById("OfferDto_JobTitle").value == "") {
        document.getElementById('jobTitleValidate').style.display = 'block';
        window.scrollTo(0, 150);
        return;
    } else {
        document.getElementById('jobTitleValidate').style.display = 'none';
    }

    lnkOffer.classList.remove('done');
    lnkPay.classList.add('done');
    lnkPub.classList.remove('done');
    lnkRev.classList.remove('done');

    lnkPay.classList.add('done');
    offer.style.display = 'none';
    pub.style.display = 'none';
    rev.style.display = 'none';
    pay.style.display = 'block';


    c_pay.classList.add('done');
    document.getElementById('btn_next').style.display = 'none';
}
/*wizard-step-mobile*/
function removePayWizard() {
    c_pay.classList.remove('done');
}
function removePubWizard() {
    c_pub.classList.remove('done');
}
function removeRevWizard() {
    c_rev.classList.remove('done');
}



function getListValue(myArray, id) {
    var list = document.getElementById(id).selectedOptions;
    if (list.length > 0) {
        for (let i = 0; i < list.length; i++) {
            myArray.push(list[i].value)
        }
    }
    return myArray;
}

function getListTextContent(myArray, id) {
    var list = document.getElementById(id).selectedOptions;
    if (list.length > 0) {
        for (let i = 0; i < list.length; i++) {
            myArray.push(list[i].textContent)
        }
    }
    return myArray;
}
function save(successMessage, errorMessage) {

    var RequiredJuniorKnowledges = [];
    var RequiredSeniorKnowledges = [];
    var OptionalKnowledges = [];
    var LanguageSkillsRequireds = [];
    var LanguageSkillsOptionals = [];
    var JobCategories=[];
    var requiredJuniorKnowledges = getListValue(RequiredJuniorKnowledges, 'RequiredJuniorKnowledges');
    var requiredSeniorKnowledges = getListValue(RequiredSeniorKnowledges, 'RequiredSeniorKnowledges');
    var optionalKnowledges = getListValue(OptionalKnowledges, 'OptionalKnowledges');
    var languageSkillsRequireds = getListValue(LanguageSkillsRequireds, 'LanguageSkillsRequireds');
    var languageSkillsOptionals = getListValue(LanguageSkillsOptionals, 'LanguageSkillsOptionals');
    var jobCategories = getListValue(JobCategories, 'JobCategories');
    var form = new FormData(offerForm);
    form.append("RequiredJuniorKnowledges", requiredJuniorKnowledges);
    form.append("requiredSeniorKnowledges", requiredSeniorKnowledges);
    form.append("optionalKnowledges", optionalKnowledges);
    form.append("languageSkillsRequireds", languageSkillsRequireds);
    form.append("languageSkillsOptionals", languageSkillsOptionals);
    form.append("JobCategories", jobCategories);
    SaveOffer(form, successMessage, errorMessage);
}
function SaveOffer(form, successMessage, errorMessage) {
    let url = "SaveOffer";
    fetch(url, {
        method: "post",
        body: form
    }).then((data) => { return data.text() })
        .then((data) => {
            var msg = JSON.parse(data);
            if (msg.success == 'success') {
                MessageShow("Success", successMessage, "success");
                document.getElementById('OfferId').value = msg.offerId;
            } else {
                MessageShow("Error", errorMessage, "error");
            }
        });
}

function jobOfferPreview(required, primaryEducationText, highSchoolEducationText, secondarySchoolWithGraduageText,
    secondarySchoolWithOutGraduageText, postSecondaryText, universityStudentText, highEducationLevel1, highEducationLevel2,
    highEducationLevel3, fulltimeText, parttimeText, agrementText, freelancerText, intershipText) {
    //offer preview
    var jobTitle = document.getElementById("OfferDto_JobTitle").value;
    document.getElementById("jobTitleSpan").innerText = jobTitle;
   
    var companyName = document.getElementById("companyName").value;
    document.getElementById("companyNameSpan").innerText = companyName;


    var addressOfWork = document.getElementById("OfferDto_WorkPlaceAddress").value;
    document.getElementById("addressOfWorkP").innerText = addressOfWork;
    //wage Condition
    var baseOfSalary = document.getElementById("OfferDto_BasicSalary").value;
    document.getElementById("baseOfSalarySapn").innerText = baseOfSalary;
    var jobDescription = document.getElementById("OfferDto_JobDescription").value;
    document.getElementById("jobDescriptionP").innerText = jobDescription;
    var jobBenefit = document.getElementById("OfferDto_Benifits").value;
    document.getElementById("benefitsSpan").innerText = jobBenefit;
    //education
    var educationList = [];

    var primaryEducation = document.getElementById("OfferDto_HasPrimaryEducation").checked;
    var highSchoolEducation = document.getElementById("OfferDto_HasHighSchoolStudent").checked;
    var SecondarySchoolWithGraduation = document.getElementById("OfferDto_HasSecondarySchoolWithGraduation").checked;
    var SecondarySchoolWithOutGraduation = document.getElementById("OfferDto_HasSecondarySchoolWithOutGraduation").checked;
    var postSeconday = document.getElementById("OfferDto_HasPostSecondary").checked;
    var universityStudent = document.getElementById("OfferDto_HasUniversityStudent").checked;
    var higherEducationLevel1 = document.getElementById("OfferDto_HasHigherEducationLevel1").checked;
    var higherEducationLevel2 = document.getElementById("OfferDto_HasHigherEducationLevel2").checked;
    var higherEducationLevel3 = document.getElementById("OfferDto_HasHigherEducationLevel3").checked;
    if (primaryEducation) {
        educationList.push(primaryEducationText);
    }
    if (highSchoolEducation) {
        educationList.push(highSchoolEducationText);
    }
    if (SecondarySchoolWithGraduation) {
        educationList.push(secondarySchoolWithGraduageText);
    }
    if (SecondarySchoolWithOutGraduation) {
        educationList.push(secondarySchoolWithOutGraduageText);
    }
    if (postSeconday) {
        educationList.push(postSecondaryText);
    }
    if (universityStudent) {
        educationList.push(universityStudentText);
    }
    if (higherEducationLevel1) {
        educationList.push(highEducationLevel1);
    }
    if (higherEducationLevel2) {
        educationList.push(highEducationLevel2);
    }
    if (higherEducationLevel3) {
        educationList.push(highEducationLevel3);
    }
    document.getElementById("educationP").innerText = educationList;
    //languageSkill
    var languageSkillsRequirementArray = [];
    var languageSkillRequirementText = getListTextContent(languageSkillsRequirementArray, 'LanguageSkillsRequireds');
    var languageSkillOptionalArray = [];
    var languageSkillOptionalText = getListTextContent(languageSkillOptionalArray, 'LanguageSkillsOptionals');
    document.getElementById("requiredLanguageSkillP").innerText = languageSkillRequirementText;
    document.getElementById("optionalLanguageSkillP").innerText = languageSkillOptionalText;

    //knowledge
    var requiredJuniorKnowledgesArray = [];
    var requiredJuniorKnowledgesText = getListTextContent(requiredJuniorKnowledgesArray, 'RequiredJuniorKnowledges');
    var requiredSeniorKnowledgesArray = [];
    var requiredSeniorKnowledgesText = getListTextContent(requiredSeniorKnowledgesArray, 'RequiredSeniorKnowledges');
    var optionalKnowledgesArray = [];
    var optionalKnowledgesText = getListTextContent(optionalKnowledgesArray, 'OptionalKnowledges');
    document.getElementById("joniorKnowledgeP").innerText = requiredJuniorKnowledgesText;
    document.getElementById("seniorKnowledgeP").innerText = requiredSeniorKnowledgesText;
    document.getElementById("optionalKnowledgeP").innerText = optionalKnowledgesText;

    //JobCategories
    var jobCategories = [];
    var jobCategoriesList = getListTextContent(jobCategories, 'JobCategories');
    document.getElementById("positionSpan").innerText =" "+jobCategoriesList+" ";


    //contract type
    var contractType = [];
    var fullTime = document.getElementById("OfferDto_IsFullTime").checked;
    var partTime = document.getElementById("OfferDto_IsPartTime").checked;
    var agrement = document.getElementById("OfferDto_Agreement").checked;
    var freelancer = document.getElementById("OfferDto_Freelancer").checked;
    var intership = document.getElementById("OfferDto_IsInternShip").checked;
    if (fullTime) contractType.push(fulltimeText);
    if (partTime) contractType.push(parttimeText);
    if (agrement) contractType.push(agrementText);
    if (freelancer) contractType.push(freelancerText);
    if (intership) contractType.push(intershipText);
    document.getElementById("typeOfContractP").innerText = contractType;

    //driving License
    var HasDriverLicenceA = document.getElementById("OfferDto_HasDriverLicenceA").checked;
    var HasDriverLicenceB = document.getElementById("OfferDto_HasDriverLicenceB").checked;
    var HasDriverLicenceC = document.getElementById("OfferDto_HasDriverLicenceC").checked;
    var HasDriverLicenceD = document.getElementById("OfferDto_HasDriverLicenceD").checked;
    var HasDriverLicenceE = document.getElementById("OfferDto_HasDriverLicenceE").checked;
    var drivingLicenseArray = [];
    if (HasDriverLicenceA) {
        drivingLicenseArray.push("A");
    }
    if (HasDriverLicenceB) {
        drivingLicenseArray.push("B");
    }
    if (HasDriverLicenceC) {
        drivingLicenseArray.push("C");
    }
    if (HasDriverLicenceD) {
        drivingLicenseArray.push("D");
    }
    if (HasDriverLicenceE) {
        drivingLicenseArray.push("E");
    }
    document.getElementById("drivingLicenseP").innerHTML = drivingLicenseArray;

    //numberOfEmployees
    var numberOfEmployees = document.getElementById("numberOfEmployees").value;
    //contact
    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("lastName").value;
    var phone = document.getElementById("phone").value;
    var email = document.getElementById("contactPersonEmail").value;
    document.getElementById("firstNameSpan").innerText = firstName;
    document.getElementById("lastNameSpan").innerText = lastName;
    document.getElementById("phoneSpan").innerText = phone;
    document.getElementById("emailSpan").innerText = email;
    document.getElementById("numberEmploymentSpan").innerText = numberOfEmployees;

}
 const getSelectedText = (el) => {
        if (el.selectedIndex === -1) {
            return null;
        }
        return el.options[el.selectedIndex].text;
    }















