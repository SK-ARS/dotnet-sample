        $(document).ready(function () {

            $(".closebtn").on('click', closeFiltersAnnotationPopup); 
            $("#clearAnnotationTextFilter").on('click', clearAnnotationTextFilter);
            $("#searchAnnotationTextPopUp").on('click', searchAnnotationTextPopUp);
            $("#CloseAnnotationPopUp").on('click', CloseAnnotationPopUp);
            $("#openAnnotationFilter").on('click', openAnnotationFilter);
            $(".ViewContact").on('click', ViewAnnotationContactDetail);
            $(".ImportAnnotationText").on('click', ImportAnnotationTextFn);
            
        });

        $('#annotationTextLibraryPopupPaginator').on('click', 'a', function (e) {
            if (this.href == '') {
                return false;
            }
            else {
                $.ajax({
                    url: this.href,
                    type: 'GET',
                    cache: false,
                    success: function (result) {
                        $('#annotationtextpopup').html(result);
                        $('#annotationTextmodal').modal('show');
                    },

                    complete: function () {
                        //stopAnimation();

                    }
                });
                return false;
            }
        });

        function openAnnotationFilter() {
            debugger;
            $('#popupFilter').html($('#filterAnnotationTextPopup').html());
            $('#popupFilter').find("#filters").css("width", "350px");
            $("#annotationTextmodal").css('display', 'block');

        }

        function CloseAnnotationPopUp() {
            $("#annotationTextmodal").css('display', 'none');
            $("#annotationtextpopup").css('display', 'none');
            $("#annottaiondiv").css('display', 'block');
        }

        function closeFiltersAnnotationPopup() {
            $("#filterAnnotationTextPopup").find("#filters").css("width", 0);
            $('#popupFilter').find("#filters").css("width", 0);
            var div = document.getElementById("vehicles");
            if (div != null) {
                document.getElementById("banner-container").style.filter = "unset"
                document.getElementById("banner-container").style.background = "linear-gradient(0deg, rgba(255, 255, 255, 1) 0%, rgba(201, 205, 229, 0.44) 100%)";
            }
            else {
                document.getElementById("banner").style.filter = "unset"
                document.getElementById("navbar").style.filter = "unset";
            }
        }

        function ImportAnnotationText(annotationtext) {
            debugger;
            CloseAnnotationPopUp();
           /* $("#annottaiondiv").css('display', 'block');*/
            document.getElementById("annotText").value = annotationtext;
            //$("#annotationTextmodal").css('display', 'none');
            //$("#annotationtextpopup").css('display', 'none');
        }

        function searchAnnotationTextPopUp() {

            debugger;

            var options = { "backdrop": "static", keyboard: true };
            var annotationText = $('#popupFilter').find("#AnnotationTextSearchValue").val();
            var id = 0;
            $.ajax({
                type: "GET",
                url: '/Annotation/GetAnnotationsFromLibrary',
                contentType: "application/json; charset=utf-8",
                data: { "pageNumber": 1, "pageSize": 10, "annotationType": 0, "annotationText": annotationText, "structureId": 0 },
                datatype: "json",
                success: function (data) {
                    debugger;
                    $("#annotationtextpopup").css('display', 'block');
                    $('#annotationtextpopup').html(data);
                    $('#annotationtextpopup').modal(options);
                    $("#annottaiondiv").css('display', 'none');
                    $("#annotationTextmodal").css('display', 'block');
                    $('#popupFilter').find("#AnnotationTextSearchValue").val(annotationText);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Dynamic content load failed." + errorThrown);
                }
            });
            closeFiltersAnnotationPopup();
        }

        function clearAnnotationTextFilter() {
            debugger;
            $('#popupFilter').find("#AnnotationTextSearchValue").val("");
            searchAnnotationTextPopUp();
            closeFiltersAnnotationPopup();
        }
        function ViewAnnotationContactDetail(e) {
            var annotationTextId = e.currentTarget.dataset.AnnotationTextId;
            ViewContactDetail(annotationTextId);            
        }
        function ImportAnnotationTextFn(e) {
            var annotationText = e.currentTarget.dataset.AnnotationText;
            ImportAnnotationText(annotationText);
        }
