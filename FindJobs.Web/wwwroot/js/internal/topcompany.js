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


