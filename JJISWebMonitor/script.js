async function measure() {
   var button = document.querySelector("#btnFetch");
   try {
      button.disabled = "disabled";
      var host = document.querySelector("#host").value;
      var port = document.querySelector("#port").value;
      var timeout = document.querySelector("#timeout").value;
      var response = await fetch(`/api/responsetime/connect?host=${host}&port=${port}&timeout=${timeout}`);
      var output = document.querySelector('#output');
      output.innerHTML = output.innerHTML + `<div>${await response.json()} - ${host}</div>`;
   } finally {
      button.disabled = "";
   }
}

function pageLoaded() {
   var host = new URLSearchParams(document.location.search).get("host");
   if (host !== null && host !== '')
      document.querySelector("#host").value = host;
}

function clearOutput() {
   var output = document.querySelector('#output');
   output.innerHTML = "";
}