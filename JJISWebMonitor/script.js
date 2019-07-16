async function measure() {
   var host = document.querySelector("#host").value;
   var response = await fetch(`/api/responsetime/connect?host=${host}`);
   var output = document.querySelector('#output');
   output.innerHTML = output.innerHTML + `<div>${await response.json()}</div>`;
}

function clearOutput() {
   var output = document.querySelector('#output');
   output.innerHTML = "";
}