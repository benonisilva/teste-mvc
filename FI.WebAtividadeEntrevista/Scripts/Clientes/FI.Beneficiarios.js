let lastIndex = 0;
console.warn(urlBeneficiariosList);
console.warn(urlExlusaoBeneficiario);
$(window).on('shown.bs.modal', function () {
    if (document.getElementById("gridBeneficiarios")) {
        $('#gridBeneficiarios').jtable({
            title: 'Beneficiarios',
            paging: false, //Enable paging
            pageSize: 5, //Set page size (default: 10)
            sorting: false, //Enable sorting
            defaultSorting: 'Nome ASC', //Set default sorting
            actions: {
                listAction: urlBeneficiariosList
            },
            fields: {
                NomeBeneficiario: {
                    title: 'Nome',
                    width: '30%'
                },
                CPFBeneficiario: {
                    title: 'CPF',
                    width: '30%'
                },
                Alterar: {
                    title: '',
                    width: '20%',
                    display: function (data) {
                        return '<button onclick="" class="btn btn-primary btn-sm">Alterar</button>';
                    }
                },
                Id: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                Excluir: {
                    title: '',
                    width: '20%',
                    display: function (data) {
                        console.warn(data);
                        return '<button onclick="removeBeneficiarioByIndex(' + data.record.Id +')"'+ 'class="btn btn-danger btn-sm">Exluir</button>';
                    }
                }
            }
        });

    }

    if (document.getElementById("gridBeneficiarios")) {
        $('#gridBeneficiarios').jtable('load');
    }

    $('#btnInserirBeneficiario').on("click", function (e) {
        e.preventDefault();
        const nome = $('#NomeBeneficiario').val();
        const cpf = $('#CPFBeneficiario').val();

        $.ajax({
            url: urlIncluirNovoBeneficiario,
            type: 'POST',
            dataType: 'json',
            data: {
                "NomeBeneficiario": nome,
                "CPFBeneficiario": unmask(cpf)
            },
            error:
                function (r) {
                    if (r.status == 400) {
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    }
                    else if (r.status == 500) {
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    }
                        
                },
            success:
                function (response) {
                    if (!!response) {
                        $('#gridBeneficiarios').jtable("addRecord", {
                            record: {
                                Id: !!response.Id ? response.Id : lastIndex,
                                NomeBeneficiario: response.NomeBeneficiario,
                                CPFBeneficiario: response.CPFBeneficiario,
                                Excluir: response.CPFBeneficiario,
                                Alterar: response.CPFBeneficiario
                            },
                            clientOnly: true
                        });
                        lastIndex = lastIndex + 1;
                    }
                    $("#formBeneficiario")[0].reset();
                }
        });
    });
});

function removeBeneficiarioByIndex(index) {
    console.warn(index);
    $.ajax({
        url: urlExlusaoBeneficiario + index,
        type: 'POST',
        error:
            function (r) {
                if (r.status == 400) {
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                }
                else if (r.status == 500) {
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                }

            },
        success:
            function (response) {
                console.warn(response);
                if (!!response) {
                    $('#gridBeneficiarios').jtable("deleteRecord", {
                        key: index,
                        clientOnly: true
                    });
                }
            }
    });

}