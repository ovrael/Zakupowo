using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopApp.Utility.EmailDesigner
{
    public static class EmailDesigner
    {
        public static string PreTransactionRequestHTML = "<!DOCTYPE html><html><head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <title>Zakupowo e-mail</title> <!-- Google fonts --> <link rel=\"preconnect\" href=\"https://fonts.gstatic.com\"> <link href=\"https://fonts.googleapis.com/css2?family=Staatliches&display=swap\" rel=\"stylesheet\"> <script type=\"text/javascript\" src=\"https://gc.kis.v2.scr.kaspersky-labs.com/FD126C42-EBFA-4E12-B309-BB3FDD723AC1/main.js?attr=gmk8Y3Sx9QeMLGMlMhVANPvWz1JDBlSVmvgeUFEzLFNd_8pBbkQEkmNvyZ-9OQbysjVBfU7SxqkJRh5jgkouns9oLRDPoAHDjfTKJFKYzOKkU-xmBW4H8kr8_UiR7VXnkbEb8UiYgyuXH8si9TMB7hOTHW3lQyDOAVzaRuNCfyQ3Fc4iD0j5PjoVeqStWvLVA1W_HnmYOL0qCj7R0Oop-O9mKs8SRgq0A7jgLVJ6b-9gG3EHCoyozh2v40R9SLs2OfIW9iNPoxz1N8DtzJ_IjdiujPhal0dIoz_W-1WR5c5BswQvJ-x3k8YUrtLadeJHnIbFUg7lauKiWQOz7ql71w\" charset=\"UTF-8\"></script></head><body style=\"padding: 0; margin:0; box-sizing: border-box;\"> <table style=\"background-color:#F0EEEE; margin: 0 auto; border-collapse: collapse; width:100%; text-align: center; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif; table-layout: fixed; word-wrap: break-word;\" cellspacing=\"0\" cellpadding=\"0\"> <tr style=\"background-color: #002447;\"> <td colspan=\"8\"> <h1 style=\"font-family: Staatliches; letter-spacing:2px; color:#ffb24a; font-size:2em; font-weight: 400;\">Zakupowo</h1> </td> </tr>";
        public static string PostTransactionRequestHTML = " <tr> <td colspan=\"8\"> <p style=\"margin-top: 10px;\">Wiadomość wygenerowana automatycznie przez Zakupowo.</p> </td> </tr> <tr> <td colspan=\"8\"> <p style=\"margin-top: 5px;\">Po więcej informacji, sprawdź swoją <a href=\"http://zakupowo.azurewebsites.net/UserPanel/TransactionsHistory\">historię transakcji</a>.</p> </td> </tr> <tr> <td colspan=\"8\"> <p style=\"margin-top: 5px;\"><a href=\"http://zakupowo.azurewebsites.net/Home/Contact\">Skontaktuj się z nami</a>, jeśli coś jest niezrozumiałe lub w przypadku chęci anulowania subskrypcji</p> </td> </tr> <tr style=\"background-color: #002447;\"> <td colspan=\"8\"> <p style=\"color: white; padding: 10px;\">© 2021 Zakupowo. Aplikacja stworzona w celach edukacyjnych przez studentów informatyki Uniwerystetu Ekonomicznego w Katowicach</p> </td> </tr> </table></body></html>";

        public static string PrePasswordResetCodeHTML = "";
        public static string PostPasswordResetCodeHTML = "";
    }
}