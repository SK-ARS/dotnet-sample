var hf_Count;
var hf_Sonumber;

function SORTSpecialOrderViewInit() {
    hf_Count = $('#hf_Count').val();
    hf_Sonumber = $('#hf_Sonumber').val();
    $('#SpecialorderCount').val(hf_Count);
    $('#SONumber').val(hf_Sonumber);

    if (typeof isSpecialOrderApiCallCompleted != 'undefined')
        isSpecialOrderApiCallCompleted = true;
}
