function filter(lData) {
    var inputFornecedores, filter1, filter2, filter3, filter4, filter5, table, tr, td, td2, td3, td4, i, txtValue, txtValue2, txtValue3, txtValue4, inputStatus, inputData, inputData1, inputFilial;

    //Fornecedores
    inputFornecedores = document.getElementById("Fornecedores");
    filter1 = inputFornecedores.value.toUpperCase();

    //Status
    var is = document.getElementById("Status");
    inputStatus = is.options[is.selectedIndex].text;
    filter2 = inputStatus.toUpperCase();

    //Data
    inputData = document.getElementById("data");
    filter3 = Date.parse(inputData.value);
    inputData1 = document.getElementById("data1");
    filter5 = Date.parse(inputData1.value);

    //Filial
    var is2 = document.getElementById("Filial");
    inputFilial = is2.options[is2.selectedIndex].text;
    filter4 = inputFilial.toUpperCase();

    if (lData) {
        if (isNaN(filter3) || isNaN(filter5)) {
            return false;
        }
    }

    table = document.getElementById("dataTable");
    tr = table.getElementsByTagName("tr");

    for (i = 0; i < tr.length; i++) {
        //CardCode
        td = tr[i].getElementsByTagName("td")[5];
        //Status
        td2 = tr[i].getElementsByTagName("td")[2];
        //Data
        td3 = tr[i].getElementsByTagName("td")[1];
        //Filial
        td4 = tr[i].getElementsByTagName("td")[0];

        if (td) {
            //CardCode
            txtValue = td.textContent || td.innerText;
            //Status
            txtValue2 = td2.textContent || td2.innerText;
            //Data
            txtValue3 = td3.textContent || td3.innerText;
            //Filial
            txtValue4 = td4.textContent || td4.innerText;

            if (filter4 == "") {
                //Se não tiver filtro de data
                if (isNaN(filter3) || isNaN(filter5)) {

                    //Se não tiver filtro de fornecedor
                    if (filter1 == "") {
                        if (txtValue2.toUpperCase().indexOf(filter2) > -1) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }

                        //Se tiver filtro de fornecedor
                    } else {
                        if (txtValue.toUpperCase().indexOf(filter1) > -1 && txtValue2.toUpperCase().indexOf(filter2) > -1) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }
                    }
                    //Se tiver filtro de data.
                } else {

                    //Formata a data.
                    //var d = txtValue3.split('-');
                    //txtValue3 = d[2] + "-" + d[1] + "-" + d[0];
                    txtValue3 = Date.parse(txtValue3);

                    //Se não tiver filtro de fornecedor
                    if (filter1 == "") {
                        if (txtValue2.toUpperCase().indexOf(filter2) > -1 && txtValue3 >= filter3 && txtValue3 <= filter5) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }

                        //Se tiver filtro de fornecedor
                    } else {
                        if (txtValue.toUpperCase().indexOf(filter1) > -1 && txtValue2.toUpperCase().indexOf(filter2) > -1 && txtValue3 >= filter3 && txtValue3 <= filter5) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }
                    }
                }
            } else {
                //Se não tiver filtro de data
                if (isNaN(filter3) || isNaN(filter5)) {

                    //Se não tiver filtro de fornecedor
                    if (filter1 == "") {
                        if (txtValue2.toUpperCase().indexOf(filter2) > -1 && txtValue4.toUpperCase().indexOf(filter4) > -1) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }

                        //Se tiver filtro de fornecedor
                    } else {
                        if (txtValue.toUpperCase().indexOf(filter1) > -1 && txtValue2.toUpperCase().indexOf(filter2) > -1 && txtValue4.toUpperCase().indexOf(filter4) > -1) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }
                    }
                    //Se tiver filtro de data.
                } else {

                    //Formata a data.
                    //var d = txtValue3.split('-');
                    //txtValue3 = d[2] + "-" + d[1] + "-" + d[0];
                    txtValue3 = Date.parse(txtValue3);

                    //Se não tiver filtro de fornecedor
                    if (filter1 == "") {
                        if (txtValue2.toUpperCase().indexOf(filter2) > -1 && txtValue3 >= filter3 && txtValue3 <= filter5 && txtValue4.toUpperCase().indexOf(filter4) > -1) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }

                        //Se tiver filtro de fornecedor
                    } else {
                        if (txtValue.toUpperCase().indexOf(filter1) > -1 && txtValue2.toUpperCase().indexOf(filter2) > -1 && txtValue3 >= filter3 && txtValue3 <= filter5 && txtValue4.toUpperCase().indexOf(filter4) > -1) {
                            tr[i].style.display = "";
                        } else {
                            tr[i].style.display = "none";
                        }
                    }
                }
            }
        }
    }
}

window.onload = filter(false);