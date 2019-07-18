async function measure() {
   event.preventDefault();
   var button = document.querySelector("#btnFetch");
   try {
      button.disabled = "disabled";
      button.value = "Connecting ...";
      var host = document.querySelector("#host").value;
      var port = document.querySelector("#port").value;
      var timeout = document.querySelector("#timeout").value;
      var response = await fetch(`/api/responsetime/connect?host=${host}&port=${port}&timeout=${timeout}`);
      var output = document.querySelector('#output');
      output.innerHTML = output.innerHTML + `<div>${await response.json()} - ${host}</div>`;
   } finally {
      button.disabled = "";
      button.value = "Connect";
   }
}

function pageLoaded() {
   setInputFromQuery("host");
   setInputFromQuery("port");
}

function setInputFromQuery(name) {
   var value = new URLSearchParams(document.location.search).get(name);
   if (value !== null && value !== '')
      document.querySelector("#" + name).value = value;
   
}

function clearOutput() {
   var output = document.querySelector('#output');
   output.innerHTML = "";
}