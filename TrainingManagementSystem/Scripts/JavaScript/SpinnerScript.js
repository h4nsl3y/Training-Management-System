function DisplaySpinner() {
    let bodyElement = document.body;

    let containerElement = document.createElement("div");
    containerElement.setAttribute("id", "spinnerContainer");
    containerElement.setAttribute("class", "spinner-container");

    let spinnerElement = document.createElement("div");
    spinnerElement.setAttribute("class", "spinner");

    containerElement.append(spinnerElement);
    bodyElement.appendChild(containerElement);
}
function RemoveSpinner() {
    let containerElement = document.getElementById("spinnerContainer")
    containerElement.remove();
}