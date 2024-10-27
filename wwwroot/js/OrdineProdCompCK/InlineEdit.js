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
}