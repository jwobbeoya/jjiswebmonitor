async function measure() {
   var button = document.querySelector("#btnFetch");
   try {
      button.disabled = "disabled";
      var host = document.querySelector("#host").value;
      var response = await fetch(`/api/responsetime/connect?host=${host}`);
      var output = document.querySelector('#output');
      output.innerHTML = output.innerHTML + `<div>${await response.json()}</div>`;
   } finally {
      button.disabled = "";
   }
}

function clearOutput() {
   var output = document.querySelector('#output');
   output.innerHTML = "";
}