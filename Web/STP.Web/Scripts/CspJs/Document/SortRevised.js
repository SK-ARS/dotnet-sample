    $(document).on('click', '#create_pdf', function () {
        let pdf = new jsPDF();
        let section = $('form');
        let page = function () {
            pdf.save('pagename.pdf');
            pdf.addHTML(section, page);
        };
        var element = $('body');
        html2pdf(element);
        $('body').on('click', '.noPrint', function () {
            window.print();
        });
        

    })




