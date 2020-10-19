

(function () {

    //Flights index view variables
    const date1 = document.querySelector("#date1Div");
    const date2 = document.querySelector("#date2Div");
    const checkBoxDateRange = document.querySelector("#dateRange");
    const daySelect = document.querySelector("#day");
    const dayRadio = document.querySelector("#dayRadio");
    const weekdaysRadio = document.querySelector("#weekDaysRadio");
    const weekendRadio = document.querySelector("#weekendRadio");
    const airportsDiv = document.querySelector("#airportsDiv");
    const airportsSelect = document.querySelector("#airports");
    const daysSelect = document.querySelector("#daySelect");
    const searchOptions = document.querySelector("#searchOptions");
    const airportOption = document.querySelector("#airportOption");
    const arrDepOption = document.querySelector("#arrDepOption");
    const arrDep = document.querySelector("#ar-de");
    const date1Select = document.querySelector("#date1");
    const date2Select = document.querySelector("#date2");
    const queryBtn = document.querySelector("#queryBtn");
    const arrDepSelect = document.querySelector("#arrDep");
    const generalTable = document.querySelector("#generalTable");
    const noTuples = document.querySelector("#noTuples");

    //Create Flights view variables
    const realPassengers = document.querySelector("#realPassengers");
    const routeCode = document.querySelector("#routeCode");
    const createFBtn = document.querySelector("#createFBtn");
    const passengersError = document.querySelector("#passengersError");

    const hideElements = () => {

        if (date1) {
            date1.style.display = 'none';
            date2.style.display = 'none';
            daySelect.style.display = 'none';
            airportsDiv.style.display = 'none';
            arrDep.style.display = 'none';
        }

        else if (passengersError) {
            passengersError.style.display = 'none';
        }

    };

    const displayStyle = (option, style) => {

        switch (option) {
            case 'check':
                date1.style.display = style;
                date2.style.display = style;
                break;

            case 'checkAirport':
                airportsDiv.style.display = style;
                break;

            case 'checkDepArr':
                arrDep.style.display = style;
                break;

            case 'radio':
                daySelect.style.display = style;
                break;

            default:
                break;
        }
    };

    //-----------------------------------------------------------------------------------
    const getAirports = () => {

        $.get("/Airport/getAirports", function (data) {
            data.forEach((airport) => {
                createAirportOption(airport);
            });
        });
    };

    //Uses object desestructuration in parameters
    const createAirportOption = ({ airportCode, airportName }) => {
        const newSelectOption = document.createElement('option');
        newSelectOption.value = airportCode;
        newSelectOption.text = airportName;
        airportsSelect.appendChild(newSelectOption);
    };

    const getAvailableDays = () => {
        $.get("/Routes/getRoutesDays", function (data) {
            data.forEach((day) => {
                createDayOption(day);
            });
        });
    };

    //Uses object desestructuration in parameters
    const createDayOption = ({ weekDay }) => {
        const newSelectOption = document.createElement('option');
        newSelectOption.value = weekDay;
        newSelectOption.text = weekDay;
        daysSelect.appendChild(newSelectOption);
    };

    //Create flights view functions
    const getRoutePassengers = (routeId) => {
        //It uses array desestructuration
        $.get("/Routes/getRoutePassengers", { routeIdFK: parseInt(routeId) }, function ([maximumPassengers]) {
            validatePassengers(realPassengers.value, maximumPassengers);
        });
    };

    const validatePassengers = (passengers, maximumPassengers) => {
        if (passengers > maximumPassengers) {
            createFBtn.disabled = true;
            passengersError.innerHTML = `<small>La cantidad máxima de pasajeros para esta ruta es de ${maximumPassengers}</small>`;
            passengersError.style.display = 'block';
        }
        else {
            createFBtn.disabled = false;
            passengersError.style.display = 'none';
            passengersError.innerHTML = "";
        }
    };

    const getAvailableDates = () => {

        $.get("/Flights/getAvailableDates", function (data) {
            data.forEach((date) => {
                createDate1Option(date);
            });
        });
    };

    //Create flights view functions
    const getAvailableDatesWithDependencies = (date) => {
        //It uses array desestructuration
        $.get("/Flights/getAvailableDatesWithDependencies", { dateTime: date }, function (data) {
            data.forEach((date) => {
                createDate2Option(date);
            });
        });
    };

    //Uses object desestructuration in parameters
    const createDate1Option = ({ dateToShow }) => {
        const newSelectOption = document.createElement('option');
        newSelectOption.value = dateToShow;
        newSelectOption.text = dateToShow;
        date1Select.appendChild(newSelectOption);
    };

    const createDate2Option = ({ dateToShow }) => {
        const newSelectOption = document.createElement('option');
        newSelectOption.value = dateToShow;
        newSelectOption.text = dateToShow;
        date2Select.appendChild(newSelectOption);
    };

    const restartDate2Select = () => {
        const newOption = `<option value="NA">Elija una opcion</option>`;
        $(date2Select).empty();
        date2Select.disabled = false;
        date2Select.innerHTML = newOption;
    };

    const createDefaultOption = () => {
        const newOption = `<option value="NA">Elija una opcion</option>`;
        airportsSelect.innerHTML = newOption;
        daysSelect.innerHTML = newOption;
        date1Select.innerHTML = newOption;
        date2Select.innerHTML = newOption;
    };

    const clearOptions = () => {
        checkBoxDateRange.checked = false;
        airportOption.checked = false;
        dayRadio.checked = false;
        weekdaysRadio.checked = false;
        weekendRadio.checked = false;
        arrDepOption.checked = false;
        $(date2Select).empty();
        $(airportsSelect).empty();
        $(daysSelect).empty();
        $(date1Select).empty();
        $(date2Select).empty();
    };

    const createRows = (nombreAeropuerto, codigoAeropuertoPK, codigoVuelo, fecha, arrivalDeparture, cantidadRealPasajeros) => {
        const tBody = document.querySelector('#tBody');
        const newRow = document.createElement('tr'); //Create the row and add each td dinamically
        const newName = document.createElement('td');
        const newCode = document.createElement('td');
        const newFCode = document.createElement('td');
        const newDate = document.createElement('td');
        const newAD = document.createElement('td');
        const newPass = document.createElement('td');
        const fillSpace = document.createElement('td');
        newPass.setAttribute('class', 'passengers');
        newName.innerHTML = nombreAeropuerto;
        newCode.innerHTML = codigoAeropuertoPK;
        newFCode.innerHTML = codigoVuelo;
        newDate.innerHTML = fecha;
        newAD.innerHTML = arrivalDeparture;
        newPass.innerHTML = cantidadRealPasajeros;
        newRow.appendChild(newName);
        newRow.appendChild(newCode);
        newRow.appendChild(newFCode);
        newRow.appendChild(newDate);
        newRow.appendChild(newAD);
        newRow.appendChild(newPass);
        newRow.appendChild(fillSpace);
        tBody.appendChild(newRow);
    };

    const getFilteredFlights = (airport, arrivalDeparture, dayOption, startDate, endDate, radioOptionSelected) => {

        $.get("/Flights/getFilteredFlights", {
            airport: airport,
            arrivalDeparture: arrivalDeparture,
            dayOption: dayOption,
            startDate: startDate,
            endDate: endDate,
            radioOptionSelected: radioOptionSelected
        }, function (data) {
            console.log(data);
            if (data.length == 0) {
                setTimeout(() => {
                    noTuples.style.display = 'block';
                }, 100);

            }
            else {
                noTuples.style.display = 'none';
            }
            const newTable = `<table class="table text-center table-striped" id="generalTable">
                    <thead class="thead-dark">
                        <tr>
                            <th>
                                Nombre de Aeropuerto
                            </th>
                            <th>
                                Código de aeropuerto
                            </th>
                            <th>
                                Código del vuelo
                            </th>
                            <th>
                                Fecha
                            </th>
                             <th>
                                Llegada/Salida
                            </th>
                            <th>
                                Cantidad real de pasajeros
                            </th>

                            <th></th>
                        </tr>
                    </thead>

                    <tbody id="tBody">
                    
                    

                    </tbody>


                </table>`;

            const newTr = `<tr class="">
                        <td>
                            <h4>Total de pasajeros</h4>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <a id="totalTd" class="btn btn-success text-white"></a>
                        </td>
                        <td>
                        </td>
                    </tr>`;

            generalTable.style.display = 'none';
            newContainer.innerHTML = newTable;
            data.forEach((tuple) => {
                createRows(tuple.airportName, tuple.airportCode, tuple.flightCode, tuple.dateToShow, tuple.arrivalDeparture, tuple.realPaasengers);
            });
            tBody.innerHTML += newTr; //Add passengers tr after creating the tuples
            sum(); //Add the passengers to get total
        });
    };

    const sum = () => {
        const total = document.getElementsByClassName("passengers"); //Get all passengers with the same class
        var suma = 0;
        for (i = 0; i < total.length; i++) {
            suma += parseInt(total[i].textContent);
        }
        var tot = document.getElementById("totalTd"); //Append sum to button
        tot.innerHTML = suma;
    }

    //Function that contains applications events
    const events = () => {

        //If exists checkBoxDateRange
        if (checkBoxDateRange) {

            checkBoxDateRange.addEventListener('click', (event) => {
                if (event.target.checked) {
                    getAvailableDates();
                    displayStyle('check', 'block');
                }
                else {
                    displayStyle('check', 'none');
                }
            });

            airportOption.addEventListener('click', (event) => {
                if (event.target.checked) {
                    getAirports();
                    displayStyle('checkAirport', 'block');
                }
                else {
                    displayStyle('checkAirport', 'none');
                }
            });

            arrDepOption.addEventListener('click', (event) => {
                if (event.target.checked) {
                    displayStyle('checkDepArr', 'block');
                }
                else {
                    displayStyle('checkDepArr', 'none');
                }
            });

            dayRadio.addEventListener('click', (event) => {
                getAvailableDays();
                displayStyle('radio', 'block');
            });

            weekdaysRadio.addEventListener('click', (event) => {
                displayStyle('radio', 'none');
            });

            weekendRadio.addEventListener('click', (event) => {
                displayStyle('radio', 'none');
            });

            searchOptions.addEventListener('click', () => {
                clearOptions();
                hideElements();
                createDefaultOption();
            });

            date1Select.addEventListener('change', () => {
                restartDate2Select();
                getAvailableDatesWithDependencies(date1Select.value);

            });

            queryBtn.addEventListener('click', () => {
                const radioOptionSelected = (dayRadio.checked) ? dayRadio.value : (weekdaysRadio.checked) ? weekdaysRadio.value : (weekendRadio.checked) ? weekendRadio.value : 0;
                getFilteredFlights(airportsSelect.value, arrDepSelect.value, daysSelect.value, date1Select.value, date2Select.value, radioOptionSelected);
            });
        }

        //CREATE FLIGHTS VIEW FUNCTIONS
        if (realPassengers) {
            realPassengers.addEventListener('keyup', (event) => {
                getRoutePassengers(routeCode.value);
            });
        }
    };


    //Starts the JS script
    const init = () => {
        noTuples.style.display = 'none';
        hideElements();
        events();

    };

    init();
})();


