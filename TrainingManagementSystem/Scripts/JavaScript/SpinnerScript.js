
document.addEventListener('DOMContentLoaded', function () { displaySpinner(); })
function displaySpinner() {
    let bodyElement = document.body;

    let containerElement = document.createElement("div");
    containerElement.setAttribute("id", "spinnerContainer");
    containerElement.setAttribute("class", "spinner-container");

    let spinnerElement = document.createElement("div");
    spinnerElement.setAttribute("class", "spinner");

    containerElement.append(spinnerElement);
    bodyElement.appendChild(containerElement);

    setTimeout(function () {
        containerElement.remove();
    }, 2000);
}


