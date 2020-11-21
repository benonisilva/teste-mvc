$(window).on('shown.bs.modal', function () {
    if (document.getElementById("gridBeneficiarios")) {
        $('#gridBeneficiarios').jtable({
            title: 'Beneficiarios',
            paging: false, //Enable paging
            pageSize: 5, //Set page size (default: 10)
            sorting: false, //Enable sorting
            defaultSorting: 'Nome ASC', //Set default sorting
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '30%'
                },
                CPF: {
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
                Excluir: {
                    title: '',
                    width: '20%',
                    display: function (data) {
                        return '<button onclick="" class="btn btn-danger btn-sm">Exluir</button>';
                    }
                }
            }
        });


    }
    $('#btnInserirBeneficiario').on("click", function (e) {
        e.preventDefault();
        const nome = $('#NomeBeneficiario').val();
        const cpf = $('#CPFBeneficiario').val();

        $.ajax({
            url: '/Cliente/IncluirBeneficiarioTemp',
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
                                Nome: response.NomeBeneficiario,
                                CPF: response.CPFBeneficiario,
                                Excluir: response.CPFBeneficiario,
                                Alterar: response.CPFBeneficiario
                            },
                            clientOnly: true
                        });
                    }
                    $("#formBeneficiario")[0].reset();
                }
        });
    });
});