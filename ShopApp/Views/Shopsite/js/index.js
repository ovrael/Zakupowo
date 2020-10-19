$("#accountLink").on("click", function(){
    if (sessionStorage.getItem('status') != null)
    {
        window.location.href = "account.html";
    }
    else
    {
        window.location.href = "login.html";
    }
});