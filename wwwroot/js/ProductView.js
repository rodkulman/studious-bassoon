$(document).ready(function() {
    $("#addPrice").on("click", function() {
        $.ajax({
            url: "/ProductView/AddPrice",
            cache: false,
            success: function(html) { $("#prices").append(html); }
        });
    });

    $("input[type='radio']").on("input", function() {
        var sender = this;
    
        $("input[type='radio']").each(function(index) {
            if (this.name != sender.name && $(this).data('RadioGroup') == $(this).data('RadioGroup')) {
                this.checked = false;
            }
        });
    });
});