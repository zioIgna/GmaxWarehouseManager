let formColl = document.getElementsByClassName('inlineReceiverForm');
Array.from(formColl).forEach(el => el.addEventListener("submit", populatePartialView));

async function populatePartialView(event) {
    event.preventDefault();

    const response = await fetch(
        event.target.action,
        {
            method: "post",
            body: new FormData(event.target)
        }
    );
    const view = await response.text();
    //const ordineProduzioneNumber = parseInt(view.split('#ordineProduzione_')[1].split('" href=')[0]);
    //const ordineProduzioneId = "ordineProduzione_" + ordineProduzioneNumber;
    const ordineProdCompCKTipoArticolo = event.target.dataset.tipoarticolo;
    const ordineProdCompCKCodArticolo = event.target.dataset.codicearticolo;
    const ordineProduzioneNroLancio = event.target.dataset.nrolancio;
    const ordineProduzioneNroSottolancio = event.target.dataset.nrosottolancio;
    const opcId = "opc_" + ordineProdCompCKTipoArticolo + "_" + ordineProdCompCKCodArticolo + "_" + ordineProduzioneNroLancio + "_" + ordineProduzioneNroSottolancio;
    document.getElementById(opcId).innerHTML = view;
};

document.addEventListener('DOMContentLoaded', function () {
    // 1. Recupera gli elementi
    var magSelect = document.getElementById('magSelect');
    var codSelectedMag = document.getElementById('codSelectedMag');
    if (!magSelect || !codSelectedMag) return;

    // 2. Inizializza codSelectedMag con il data-codmag della prima option
    if (magSelect.options.length > 0) {
        codSelectedMag.value = magSelect.options[0].dataset.codmag || '';
    }

    // 3. Aggiorna codSelectedMag ogni volta che cambia la select
    magSelect.addEventListener('change', function () {
        var selectedOpt = this.options[this.selectedIndex];
        codSelectedMag.value = selectedOpt.dataset.codmag || '';
    });
});

//function updateField(obj) {
//    var refSelectId = obj.getAttribute('data-select-id');
//    var refInputField = obj.getAttribute('data-input-field');
//    var selectedValue = document.getElementById(refSelectId).value;
//    document.getElementById(refInputField).value = selectedValue;
//}

function testFunc() {
    alert("messaggio di test");
}

//document.getElementById('magSelect')
//    .addEventListener('change', function () {
//        var opt = this.options[this.selectedIndex];
//        document.getElementById('codSelectedMag').value = opt.dataset.codmag;
//    });

//document.addEventListener("click", testFunc);

//document.addEventListener("DOMContentLoaded", function () {
//    // Seleziona l'elemento <select> per ID
//    const selectElement = document.getElementById("disponibilitaSelect");
//    // Seleziona il campo <input> da aggiornare
//    const inputField = document.getElementById("QtaDisponibileOrigine");

//    // Aggiunge l'event listener all'elemento <select>
//    selectElement.addEventListener("change", function () {
//        // Assegna al campo di input il valore dell'opzione selezionata
//        inputField.value = this.value;
//    });
//});