$().ready(function() {
  $("#signupForm").validate({
    errorPlacement: function(error, element){
      error.appendTo($("#" + element.attr('id') + "_validation"));
      },
    rules: {
      firstNameRegisterInput: "required",
      lastNameRegisterInput: "required",
      emailAddressRegisterInput: "required",
      passwordRegisterInput: {
        required: true,
        minlength: 8
      },
      passwordRegisterInput2: {
        required: true,
        equalTo: "#passwordRegisterInput"
      }
    },
    messages: {
      firstNameRegisterInput: "Please enter your first name",
      lastNameRegisterInput: "Please enter your last name",
      emailAddressRegisterInput: "Please enter your e-mail",
      passwordRegisterInput: {
        required: "Please enter your password",
        minlength: "Password must be at least 8 characters"
      },
      passwordRegisterInput2: {
        required: "Please confirm your password",
        equalTo: "Passwords do not match"
      },
    },
  });
});