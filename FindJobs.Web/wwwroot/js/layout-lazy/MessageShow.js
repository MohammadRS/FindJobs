
function MessageShow(title, message, type) {
    let MessagedivContainer = document.createElement("div");
    MessagedivContainer.style.position = "fixed";
    MessagedivContainer.style.top = 0;
    MessagedivContainer.style.right = 0;
    MessagedivContainer.style.left = 0;
    MessagedivContainer.style.right = 0;
    MessagedivContainer.style.bottom = 0;
    MessagedivContainer.style.backgroundColor = "rgba(0, 0, 0, 0.4)";
    MessagedivContainer.style.zIndex = 99999;
    MessagedivContainer.style.display = "none";


    let MessagedivMessage = document.createElement("div");
    MessagedivMessage.style.width = "280px";
    MessagedivMessage.style.height = "150px";
   
    if (type == "success") {
        MessagedivMessage.style.backgroundColor = "lightblue";
    }

    if (type == "warning") {
        MessagedivMessage.style.backgroundColor = "#e0a800";
    }

    if (type == "error") {
        MessagedivMessage.style.backgroundColor = "#c82333";
        MessagedivMessage.style.color = "white";
    }

    MessagedivMessage.style.position = "fixed";
    MessagedivMessage.style.top = "10px";
    MessagedivMessage.style.right = "10px";
    MessagedivMessage.style.borderRadius = "4px";
    MessagedivMessage.style.boxShadow = "4px 4px 10px gray";
    document.body.append(MessagedivContainer);
    MessagedivContainer.append(MessagedivMessage);
    MessagedivContainer.style.display = "block";




    let MessagedivTitle = document.createElement("div");
    MessagedivTitle.style.backgroundColor = "azure";
    MessagedivTitle.style.height = "50px";
    MessagedivTitle.style.padding = "10px";
    MessagedivTitle.style.textAlign = "center";
    MessagedivTitle.style.fontSize = "20px";
    MessagedivTitle.style.color = "black";
    MessagedivTitle.style.borderRadius = "4px 4px 0 0";
    MessagedivTitle.innerText = title;
    MessagedivTitle.style.fontWeight = "bold";
    MessagedivMessage.append(MessagedivTitle);

    let MesssagedivBody = document.createElement("div");
    MesssagedivBody.style.marginTop = "20px";
    MesssagedivBody.style.padding = "20px";
    MesssagedivBody.innerHTML = message;
    MesssagedivBody.style.borderRadius = "4px";
    MessagedivMessage.append(MesssagedivBody);

    var MessagedivTimeLine = document.createElement("div");
    MessagedivTimeLine.style.height = "4px";
    MessagedivTimeLine.style.width = "280px";
    MessagedivTimeLine.style.backgroundColor = "darkblue";
    MessagedivTimeLine.style.position = "absolute";
    MessagedivTimeLine.style.bottom = 0;
    MessagedivTimeLine.style.borderRadius = "4px";
    MessagedivMessage.append(MessagedivTimeLine);
    var timeout = setTimeout(() => {

        MessagedivContainer.style.display = "none";
    }, 2500);

    clearTimeout("timeout");
}

