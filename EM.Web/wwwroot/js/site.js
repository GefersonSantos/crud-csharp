
const exampleModal = document.getElementById('exampleModal')
exampleModal.addEventListener('show.bs.modal', event => {

    const button = event.relatedTarget

    const recipient = button.getAttribute('data-bs-whatever')
    const buttonConfirmar = document.getElementById('buttonConfirmar')

    buttonConfirmar.setAttribute("href", "aluno/Delete/" + recipient)
})

