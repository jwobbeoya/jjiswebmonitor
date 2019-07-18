async function measure() {
   event.preventDefault();
   var button = document.querySelector("#btnFetch");
   try {
      button.disabled = "disabled";
      button.value = "Fetching ...";
      var uri = document.querySelector("#uri").value;
      var response = await fetch(`/api/responsetime/http?uri=${uri}`);
      var output = document.querySelector('#output');
      output.innerHTML = output.innerHTML + `<div>${await response.json()} - ${uri}</div>`;
   } finally {
      button.disabled = "";
      button.value = "Fetch";
   }
}

function pageLoaded() {
   setInputFromQuery("uri");
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