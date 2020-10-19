$().ready(function() {
  $("#signinForm").validate({
    errorPlacement: function(error, element){
      error.appendTo($("#" + element.attr('id') + "_validation"));
      },
    rules: {
      emailAddressInput: "required",
      passwordInput: {
        required: true,
        minlength: 8
      },
    },
    messages: {
      emailAddressInput: "Please enter your e-mail",
      passwordInput: {
        required: "Please enter your password",
        minlength: "Password must be at least 8 characters"
      },
    },
  });
});