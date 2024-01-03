    $(document).ready(function () {
        $(".pageKeyUpValidation").keyup(function () {
            keyUpValidationFn(this);
        });
    });

    function keyUpValidationFn(e) {
        keyUpValidation(e);
    }
