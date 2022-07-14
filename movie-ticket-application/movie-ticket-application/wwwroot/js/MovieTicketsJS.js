

$("#sortButtonAscending").click(function () {

    function convertDate(d) {
        var p = d.split(".");
        const date = new Date(p[2], p[1], p[0]);


        return +(date.getTime());
    }
    
        var tbody = document.querySelector("#ticketsTableBody");
        // get trs as array for ease of use
        var rows = [].slice.call(tbody.querySelectorAll("tr"));

        rows.sort(function (a, b) {
            return convertDate(a.cells[2].innerHTML) - convertDate(b.cells[2].innerHTML);
        });

        rows.forEach(function (v) {
            tbody.appendChild(v); // note that .appendChild() *moves* elements
        });
})

$("#sortButtonDescending").click(function () {

    function convertDate(d) {
        var p = d.split(".");
        const date = new Date(p[2], p[1], p[0]);


        return +(date.getTime());
    }

    var tbody = document.querySelector("#ticketsTableBody");
    // get trs as array for ease of use
    var rows = [].slice.call(tbody.querySelectorAll("tr"));

    rows.sort(function (a, b) {
        return convertDate(b.cells[2].innerHTML) - convertDate(a.cells[2].innerHTML);
    });

    rows.forEach(function (v) {
        tbody.appendChild(v); // note that .appendChild() *moves* elements
    });
})